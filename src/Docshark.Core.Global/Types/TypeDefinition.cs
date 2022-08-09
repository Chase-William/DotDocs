using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Docshark.Core.Global.Assemblies;

namespace Docshark.Core.Global.Types
{
    /// <summary>
    /// Represents a type in a codebase that is linked to a specific project.
    /// </summary>
    public class TypeDefinition : Definition
    {        
        /// <summary>
        /// A primary key identifying the parent type.
        /// </summary>
        public string? Parent { get; set; }
        /// <summary>
        /// A list of primary keys identifying the type arguments.
        /// </summary>
        public List<string> TypeArguments { get; set; } = new();
        /// <summary>
        /// The entire namespace leading to this types location.        
        /// </summary>
        public string Namespace { get; set; }        
        /// <summary>
        /// Comments written about this type.
        /// </summary>
        public CommonComments Comments { get; set; }        
        /// <summary>
        /// The namespace to the type with the type name and all type arguments if present.
        /// </summary>
        public string TypeDescription { get; set; }
        /// <summary>
        /// A foreign key that links to the project and assembly.
        /// </summary>
        public string AssemblyForeignKey { get; set; }

        /// <summary>
        /// Create a <see cref="TypeDefinition"/> instance from the type info given.
        /// Uses the <see cref="AssemblyMapper"/> if given to assist in building the 
        /// <see cref="AssemblyMapper.MappedDefinitions"/> map while building the 
        /// <see cref="TypeMapper.MappedDefinitions"/>.
        /// </summary>
        /// <param name="info">Type information.</param>
        /// <param name="asmMapper">Mapper to be used.</param>
        /// <returns>A new <see cref="TypeDefinition"/> instance.</returns>
        public static TypeDefinition From(Type info, AssemblyMapper? asmMapper = null)
        {
            /*
             * If provided, process the assembly for the type.
             * This would prevent the need to iterate over the dictionary
             * again once the TypeMapper is done.
             */
            asmMapper?.AddAssembly(info.Assembly);

            return new()
            {
                TypeDescription = info.GetPrimaryKey(),
                Namespace = info.Namespace,
                Parent = info.BaseType?.GetPrimaryKey(),
                AssemblyForeignKey = info.Assembly.GetPrimaryKey()
            };
        }

        public override string GetPrimaryKey() 
            => TypeDescription;

        internal static string GetPrimaryKeyMemberName()
            => nameof(TypeDescription);
    }
}
