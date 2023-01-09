using LoxSmoke.DocXml;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DotDocs.Core.Models.Comments
{
    public abstract record class CommentsModel<TComment> where TComment : CommonComments
    {
        protected TComment? comments;

        /// <summary>
        /// Versions this comment has been documented for and remains the same throughout.
        /// </summary>
        public Version Version { get; init; }

        /// <summary>
        /// Id for MongoDb records.
        /// </summary>
        public ObjectId Id { get; init; }

        string? summary;
        public string? Summary
        {
            get
            {
                /*
                 * Return either the summary from the existing comment or from 
                 * the saved summary field, the result from a database query.                 
                 */
                return summary ?? comments?.Summary;
            }
            init
            {
                summary = value;
            }
        }

        public CommentsModel(TComment comments, Version version)
        {
            this.comments = comments;
            Version = version;
        }
    }
}
