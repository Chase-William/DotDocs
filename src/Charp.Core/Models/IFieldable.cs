using Charp.Core.Models.Members;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Charp.Core.Models
{
    /// <summary>
    /// Represents a type that can contain fields.
    /// This was added for the single purpose of making enumerations work as
    /// they are actually a class with their values as fields behind the scenes.
    /// </summary>
    public interface IFieldable
    {
        /// <summary>
        /// Fields of this type.
        /// </summary>
        FieldModel[] Fields { get; set; }

        /// <summary>
        /// Gets all the desired fields from the type info with documentation.
        /// </summary>
        /// <param name="info">Information about the type.</param>
        /// <param name="reader">Used to get the written documentation.</param>
        /// <returns>Collection of desired fields.</returns>
        FieldModel[] GetFields(TypeInfo info, DocXmlReader reader)
        {
            var allFields = info.GetRuntimeFields();

            // Filter out backing fields
            var userFields = allFields
                .Where(field => !field.GetCustomAttributesData().Any(attr => attr.AttributeType.Name == typeof(CompilerGeneratedAttribute).Name))
                .ToArray();
            
            if (userFields.Length == 0)
                return Array.Empty<FieldModel>();

            var tempFields = new FieldModel[userFields.Length];
            for (int i = 0; i < userFields.Length; i++)
                tempFields[i] = new FieldModel(userFields[i]);
            return tempFields;
        }
    }
}
