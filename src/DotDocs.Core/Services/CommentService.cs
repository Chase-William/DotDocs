using DotDocs.Core.Comments;
using DotDocs.Core.Language;
using LoxSmoke.DocXml;
using MongoDB.Driver;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace DotDocs.Core.Services
{
    public class CommentService
    {
        private readonly IMongoDatabase comments;

        public CommentService(IMongoDatabase comments)
            => this.comments = comments;

        public void UpdateDocumentation(Repository repo, ImmutableArray<AssemblyModel<UserTypeModel>> userAssemblies, ImmutableArray<AssemblyModel<TypeModel>> otherAssemblies)
        {
            IMongoCollection<Repository> repos = GetRepositories();
            IMongoCollection<AssemblyModel<TypeModel>> assemblies = GetAssemblies<TypeModel>();

            /*
             * Ensure each assembly used has been loaded into our documentation database before.            
             * If not, do it now.
             */

            // REPO - The project we built

            // Determine if this repo version has been documented already via it's hash
            if (repos.Find(doc => doc.CommitHash == repo.CommitHash).FirstOrDefault() == null)
            { 
                // Repo wasn't already documented, therefore:
                // Ensure each project has it's member contents documented
                foreach (var assembly in userAssemblies)
                {                    
                    // Find the same assembly in the build as in the assembly model collection to
                    // get the Models.AssemblyModel
                    InsertComments(assembly);
                }
                // Record this repo being documented for this version
                repos.InsertOne(repo);                
            }

            // ASSEMBLY - supporting assemblies used by the project
            foreach (var assembly in otherAssemblies)            
                if (assemblies // Ensure is not already documented in the system
                    .Find(a => a.Name == assembly.Assembly.GetName().Name && a.Version == assembly.Assembly.GetName().Version)
                    .FirstOrDefault() == null)
                {
                    // Take the string from the start to the end and replace dll with xml
                    var test = assembly.Assembly.GetName();
                    string docFile = assembly.Assembly.Location[0..^3] + "xml";

                    if (!File.Exists(docFile))
                        continue;

                    InsertComments(assembly, docFile);
                    // Record this assembly being documented for this version
                    assemblies.InsertOne(assembly);
                }
        }

        /// <summary>
        /// Ensures documentation for the given assembly exists already in the database.
        /// If not, it will sanitize the documentation and insert comments.
        /// </summary>
        /// <param name="assemblyModel"></param>
        /// <param name="docFilePath"></param>
        public void InsertComments<T>(AssemblyModel<T> assemblyModel, string? docFilePath = null) where T : TypeModel
        {
            string prefix = GetAssemblyPrefixString(assemblyModel);
            DocXmlReader docReader = GetSanitizedDocumentationFile(docFilePath ?? assemblyModel.Build.DocumentationFilePath);

            var methods = comments.GetCollection<MethodCommentsModel>(GetMethodCollectionName(prefix));
            var types = comments.GetCollection<TypeCommentsModel>(GetTypeCollectionName(prefix));
            var common = comments.GetCollection<CommonCommentsModel<CommonComments>>(GetCommonCollectionName(prefix));

            var version = assemblyModel.Assembly.GetName().Version;

            var tempTypes = new List<TypeCommentsModel>();
            var tempMethods = new List<MethodCommentsModel>();
            var tempCommon = new List<CommonCommentsModel<CommonComments>>();

            // Process all types
            foreach (var type in assemblyModel.Assembly.ExportedTypes)
            {
                try
                {
                    // Get comments for type
                    tempTypes.Add(new TypeCommentsModel(
                        docReader.GetTypeComments(type),
                        type,
                        version));
                    // Get comments for methods
                    foreach (var method in type
                        .GetMethods()
                        .Where(m => !m
                           .GetCustomAttributesData()
                           .Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name) &&
                               !m.Attributes.HasFlag(MethodAttributes.SpecialName) &&
                               !m.Attributes.HasFlag(MethodAttributes.RTSpecialName)))
                    {
                        var comment = docReader.GetMethodComments(method);

                        if (string.IsNullOrEmpty(comment.FullCommentText))
                            continue;

                        tempMethods.Add(new MethodCommentsModel(
                            comment,
                            method,
                            version));
                    }
                    // Get comments for properties
                    AddCommonMembers(type.GetProperties());
                    // Get comments for fields
                    AddCommonMembers(type.GetFields());
                    // Get comments for events
                    AddCommonMembers(type.GetEvents());
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }                
            }

            // Perform bulk inserts
            // Using this approach also for easier debugging, can see everything in mass before inserts
            if (tempTypes.Count > 0)
                types.InsertMany(tempTypes);
            if (tempMethods.Count > 0)
                methods.InsertMany(tempMethods);
            if (tempCommon.Count > 0)
                common.InsertMany(tempCommon);

            void AddCommonMembers<T>(T[] members) where T : MemberInfo
            {
                foreach (var member in members)
                {
                    var comment = docReader.GetMemberComments(member);
                    if (comment == null)
                        continue;

                    tempCommon.Add(new CommonCommentsModel<CommonComments>(
                        comment,
                        member,
                        version));
                }
            }
        }        

        /// <summary>
        /// Reads in an xml file containing microsofts documentation and removes any illegal tags.       
        /// </summary>
        /// <remarks>
        /// Removes HTML paragraph tags from the document.
        /// </remarks>
        /// <param name="docFilePath">File to be read and sanitized.</param>
        /// <returns>A new instance of <see cref="DocXmlReader"/>.</returns>
        private DocXmlReader GetSanitizedDocumentationFile(string docFilePath)
        {
            //string massive = "";
            MemoryStream memStream;
            using (var reader = new StreamReader(docFilePath))
            {
                memStream = new MemoryStream(
                    Encoding.UTF8.GetBytes(
                        new Regex("(<p>)|(<p .*?>)|(</?p>)|(<br/?>)").Replace(reader.ReadToEnd(), "")));
            }

            return new DocXmlReader(new XPathDocument(XmlReader.Create(memStream)));
        }

        public IMongoCollection<Repository> GetRepositories()
            => comments.GetCollection<Repository>("repositories");

        public IMongoCollection<AssemblyModel<T>> GetAssemblies<T>() where T : TypeModel
            => comments.GetCollection<AssemblyModel<T>>("assemblies");

        static string GetMethodCollectionName(string prefix)
            => $"{prefix}-methods";
        static string GetTypeCollectionName(string prefix)
            => $"{prefix}-types";
        static string GetCommonCollectionName(string prefix)
            => $"{prefix}-common";
        static string GetRepoPrefixString(Repository repo)
            => $"{repo.Name}-{repo.CommitHash}";
        static string GetAssemblyPrefixString<T>(AssemblyModel<T> assembly) where T : TypeModel
            => $"{assembly.Assembly.GetName().Name}-{assembly.Assembly.GetName().Version}";
    }
}
