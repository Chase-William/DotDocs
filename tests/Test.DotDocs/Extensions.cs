using DotDocs.Markdown;
using DotDocs.Markdown.Renderers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Test.DotDocs
{
    public enum SectionType
    {
        Header,
        Footer            
    }

    public static class Extensions
    {
        public const string FILE_EX = ".md";

        public const string ASSETS_DIR = "Assets";
        public const string RENDERERS_DIR = "Renderers";
        public const string COMPONENTS_DIR = "Components";

        public const string PROPERTY_DECLARATION_COMPONENT_DIR = "PropDeclComp";
        public const string PROPERTY_CODE_BLOCK_DECLARATION_COMPONENT_DIR = "PropCodeBlockDeclComp";

        public const string FIELD_DECLARATION_COMPONENT_DIR = "FieldDeclComp";

        public const string EVENT_DECLARATION_COMPONENT_DIR = "EventDeclComp";

        public const string METHOD_DECLARATION_COMPONENT_DIR = "MethodDeclComp";

        public static string GetExpectedSectionMarkdown(this Type type, SectionType section = SectionType.Header)
        {
            return File.ReadAllText(Path.Combine(ASSETS_DIR, type.Name, section + FILE_EX));
        }

        /// <summary>
        /// Gets the expected markdown content from file using the given <paramref name="info"/> and <paramref name="renderer"/>.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="renderer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetExpectedCompMarkdown(this PropertyInfo info, IComponentRenderer<PropertyInfo> renderer)
        {
            if (renderer is PropertyDeclaration)
                return GetCompMarkdown(info, PROPERTY_DECLARATION_COMPONENT_DIR);
            else if (renderer is PropertyCodeBlockDeclaration)
                return GetCompMarkdown(info, PROPERTY_CODE_BLOCK_DECLARATION_COMPONENT_DIR);
            else
                throw new ArgumentException($"Type {renderer} is invalid or unsupported by {nameof(GetExpectedCompMarkdown)}");
        }

        public static string GetExpectedCompMarkdown(this FieldInfo info, IComponentRenderer<FieldInfo> renderer)
        {
            if (renderer is FieldDeclaration)
                return GetCompMarkdown(info, FIELD_DECLARATION_COMPONENT_DIR);
            else
                throw new ArgumentException($"Type {renderer} is invalid or unsupported by {nameof(GetExpectedCompMarkdown)}");
        }

        public static string GetExpectedCompMarkdown(this EventInfo info, IComponentRenderer<EventInfo> renderer)
        {
            if (renderer is EventDeclaration)
                return GetCompMarkdown(info, EVENT_DECLARATION_COMPONENT_DIR);
            else
                throw new ArgumentException($"Type {renderer} is invalid or unsupported by {nameof(GetExpectedCompMarkdown)}");
        }

        public static string GetExpectedCompMarkdown(this MethodInfo info, IComponentRenderer<MethodInfo> renderer)
        {
            if (renderer is MethodDeclaration)
                return GetCompMarkdown(info, METHOD_DECLARATION_COMPONENT_DIR);
            else
                throw new ArgumentException($"Type {renderer} is invalid or unsupported by {nameof(GetExpectedCompMarkdown)}");
        }        

        static string GetCompMarkdown(MemberInfo info, in string compDir)
            => File.ReadAllText(Path.Combine(ASSETS_DIR, RENDERERS_DIR, COMPONENTS_DIR, compDir, info.Name + FILE_EX));

        public static string GetExpectedMemberMarkdown<T>(
            this T info)
            where T : MemberInfo
        {
                return File.ReadAllText(
                    Path.Combine(
                        ASSETS_DIR, // base dir assets
                        info.DeclaringType!.Name, // declaring type folder
                        info.MemberType.ToString(), // member type folder
                        info.Name + FILE_EX // member instance file
                        ));
        }
    }    
}
