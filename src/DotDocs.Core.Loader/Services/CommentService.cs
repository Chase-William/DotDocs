using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Xml.XPath;
using System.Xml;
using DotDocs.Core.Models.Mongo.Comments;
using DotDocs.Core.Models.Mongo;
using DotDocs.Core.Models;
using Microsoft.Build.Logging.StructuredLogger;
using System.Xml.Linq;
using System.Collections.Immutable;
using DotDocs.Core.Loader.Build;

namespace DotDocs.Core.Loader.Services
{
    public class CommentService
    {
        private readonly IMongoDatabase comments;

        public CommentService(IMongoDatabase comments)
            => this.comments = comments;

        public void UpdateDocumentation(Repository repo, ImmutableArray<ProjectBuildInstance> builds)
        {
            IMongoCollection<RepositoryModel> repos = GetRepositories();
            IMongoCollection<Models.Mongo.AssemblyModel> assemblies = GetAssemblies();

            /*
             * Ensure each assembly used has been loaded into our documentation database before.            
             * If not, do it now.
             */

            // REPO - The project we built

            // Determine if this repo version has been documented already via it's hash                    
            if (repos.Find(doc => doc.CommitHash == repo.CommitHash) == null)
            { // Add each repo's documentation file
                foreach (var build in builds)
                {
                    // Find the same assembly in the build as in the assembly model collection to
                    // get the Models.AssemblyModel
                    InsertComments(repo.UsedAssemblies
                        .Single(a => a.Assembly == build.Assembly), 
                            build.DocumentationFilePath,
                            GetRepoPrefixString(repo));
                }
            }

            // ASSEMBLY - supporting assemblies used by the project
            foreach (var asm in repo.UsedAssemblies)            
                if (asm.LocalProject == null // Ensure is not produced by a local project, aka is our user's project
                    && assemblies // Ensure is not already documented in the system
                    .Find(a => a.Name == asm.Name && a.Version == asm.Assembly.GetName().Version) == null) // Is not a user project, just supporting assembly
                {
                    string docFile = asm.Assembly.Location[..4] + ".xml";
                    InsertComments(asm, docFile, GetAssemblyPrefixString(asm));
                }
        }        

        /// <summary>
        /// Ensures documentation for the given assembly exists already in the database.
        /// If not, it will sanitize the documentation and insert comments.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="docPath"></param>
        public void InsertComments(Models.AssemblyModel assembly, string docPath, string prefix)
        {
            DocXmlReader docReader = GetSanitizedDocumentationFile(docPath);

            var methods = comments.GetCollection<MethodCommentsModel>(GetMethodCollectionName(prefix));
            var types = comments.GetCollection<TypeCommentsModel>(GetTypeCollectionName(prefix));
            var common = comments.GetCollection<CommonCommentsModel<CommonComments>>(GetCommonCollectionName(prefix));

            var version = assembly.Assembly.GetName().Version;

            // Process all types
            foreach (var type in assembly.Types)
            {
                // Get comments for type
                type.Comments = new TypeCommentsModel(
                    docReader.GetTypeComments(type.Info),
                    type.Info.FullName,
                    version);
                types.InsertOne(type.Comments);
                // Get comments for methods
                foreach (var method in type.Methods)
                {
                    method.Comments = new MethodCommentsModel(
                        docReader.GetMethodComments(method.Info, true),
                        method.Name,
                        version);
                    methods.InsertOne(method.Comments);
                }
                // Get comments for properties
                foreach (var property in type.Properties)
                {
                    property.Comments = new CommonCommentsModel<CommonComments>(
                        docReader.GetMemberComments(property.Info),
                        property.Name,
                        version);
                    common.InsertOne(property.Comments);
                }
                // Get comments for fields
                foreach (var field in type.Fields)
                {
                    field.Comments = new CommonCommentsModel<CommonComments>(
                        docReader.GetMemberComments(field.Info),
                        field.Name,
                        version);
                    common.InsertOne(field.Comments);
                }
                // Get comments for events
                foreach (var _event in type.Events)
                {
                    _event.Comments = new CommonCommentsModel<CommonComments>(
                        docReader.GetMemberComments(_event.Info),
                        _event.Name,
                        version);
                    common.InsertOne(_event.Comments);
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

        public IMongoCollection<RepositoryModel> GetRepositories()
            => comments.GetCollection<RepositoryModel>("repositories");

        public IMongoCollection<Models.Mongo.AssemblyModel> GetAssemblies()
            => comments.GetCollection<Models.Mongo.AssemblyModel>("assemblies");

        static string GetMethodCollectionName(string prefix)
            => $"{prefix}-methods";
        static string GetTypeCollectionName(string prefix)
            => $"{prefix}-types";
        static string GetCommonCollectionName(string prefix)
            => $"{prefix}-common";
        static string GetRepoPrefixString(Repository repo)
            => $"{repo.Name}-{repo.CommitHash}";
        static string GetAssemblyPrefixString(Models.AssemblyModel assembly)
            => $"{assembly.Name}-{assembly.Assembly.GetName().Version}";
    }
}
