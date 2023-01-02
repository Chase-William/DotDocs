using DotDocs.Core.Models.Comments;
using DotDocs.Core.Models;
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

namespace DotDocs.Core.Loader.Services
{
    public class CommentService
    {
        private IMongoDatabase db;

        public CommentService(IMongoDatabase database)
            => db = database;

        public void ProcessComments(AssemblyModel assembly, string docPath)
        {
            DocXmlReader docReader = GetSanitizedDocumentationFile(docPath);
            var methods = db.GetCollection<MethodCommentsModel>("methods");
            var types = db.GetCollection<TypeCommentsModel>("types");
            var common = db.GetCollection<CommonCommentsModel<CommonComments>>("common");

            var version = assembly.Assembly.GetName().Version;

            // Process all types
            foreach (var type in assembly.Types)
            {
                // Get comments for type
                type.Comments = new TypeCommentsModel(
                    docReader.GetTypeComments(type.Info),
                    type.FullName,
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

            // TODO: Remove in prod
            //using (var writer = new StreamWriter(@"C:\Users\Chase Roth\Desktop\test.xml", false))
            //{
            //    writer.Write(memStream);
            //}

            return new DocXmlReader(new XPathDocument(XmlReader.Create(memStream)));
        }

        public IMongoCollection<ProjectDocumentationFile> GetCommentSourceFileRecords()
            => db.GetCollection<ProjectDocumentationFile>("files");
    }
}
