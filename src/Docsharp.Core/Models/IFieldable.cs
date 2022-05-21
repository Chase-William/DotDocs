using Docsharp.Core.Models.Members;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Docsharp.Core.Models
{
    /// <summary>
    /// Represents a type that can contain fields.
    /// This was added for the single purpose of making enumerations work as
    /// they are actually a class with their values as fields behind the scenes.
    /// </summary>
    public interface IFieldable
    {
        FieldModel[] Fields { get; set; }

        FieldModel[] GetFields(TypeInfo info, DocXmlReader reader)
        {
            var fields = info.GetFields();
            if (fields.Length == 0)
                return Array.Empty<FieldModel>();
            var tempFields = new FieldModel[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                tempFields[i] = new FieldModel(fields[i]);
            return tempFields;
        }
    }
}
