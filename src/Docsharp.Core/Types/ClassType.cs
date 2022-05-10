using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/*
 * 
 * 
 * 
 * 
 * 
 * 
 *  WE NEED TYPE INFO IN OUR JSON...
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */

namespace Docsharp.Core.Types
{
    public class ClassType : TypeMember<TypeInfo>
    {
        public List<PropertyMember> Properties { get; set; } = new();
        public List<FieldMember> Fields { get; set; } = new();

        public string Namespace => member.Namespace;
        public string FullName => member.FullName;
        public bool IsPublic => member.IsPublic;

        public override bool CanHaveInternalTypes => true;
        public override string Type => "Class";

        public ClassType(TypeInfo member) : base(member)
        {
            this.member = member;

            PropertyMember memProp;
            FieldMember memField;

            /*
             * Get All Properties
             */
            foreach (PropertyInfo prop in member.GetProperties())
            {
                memProp = new PropertyMember(prop);
                Properties.Add(memProp);
            }

            /*
             * Get All Fields (includes backing fields atm) 
             */
            foreach (FieldInfo field in member.GetFields())
            {
                memField = new FieldMember(field);
                Fields.Add(memField);
            }
        }
    }
}
