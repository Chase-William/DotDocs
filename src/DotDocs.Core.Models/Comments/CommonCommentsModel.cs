using LoxSmoke.DocXml;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Core.Models.Comments
{
    /// <summary>
    /// Extension of <see cref="CommonComments"/> for database interaction.
    /// </summary>
    /// <typeparam name="TComment"></typeparam>
    public record class CommonCommentsModel<TComment> where TComment : CommonComments
    {
        protected TComment? comments;

        /// <summary>
        /// Full name of assembly member used to identify member across versions.        
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Versions this comment has been documented for and remains the same throughout.
        /// </summary>
        public List<Version> Versions { get; set; } 

        /// <summary>
        /// Id for MongoDb records.
        /// </summary>
        public ObjectId Id { get;  init; }

        string? summary;
        public string? Summary { 
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

        public CommonCommentsModel() { }        
        public CommonCommentsModel(TComment comments, string fullName, Version version)
        {
            this.comments = comments;
            this.FullName = fullName;
            this.Versions = new List<Version>() { version };
        }                  
    }
}
