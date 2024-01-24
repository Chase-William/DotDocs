using DocXml.Reflection;
using DotDocs.IO;
using DotDocs.IO.Routing;
using DotDocs.Models;
using DotDocs.Render;
using LoxSmoke.DocXml;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;

namespace DotDocs.Markdown
{    
    public class MarkdownRenderer : IRenderable
    {
        const int DEFAULT_STR_BUILDER_CAPACITY = 256;        

        public RepositoryModel Model { get; set; }

        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }

        public Dictionary<string, CommonComments> Comments { get; set; } = new();

        public IOutputable Output { get; set; }

        public MarkdownRenderer(IOutputable output)
        {
            Output = output;
        }

        public void Init(
            RepositoryModel model,
            ImmutableDictionary<string, ProjectModel> projects
            ) {
            Model = model;
            Projects = projects;

            RenderHelper.Output = Output;
            RenderHelper.Builder = new StringBuilder(DEFAULT_STR_BUILDER_CAPACITY);
            RenderHelper.Assemblies = Projects.ToImmutableDictionary(k => k.Value.Assembly.GetKey(), v => v.Value.Assembly);

            // Loop to create all comments
            foreach (var proj in Projects.Values)
            {
                // Load XML documentation file
                var reader = new DocXmlReader(proj.DocumentationFilePath);
                foreach (var type in proj.Assembly.ExportedTypes)
                {
                    // Create key/value for type comment and it's member comments
                    Comments.Add(type.ToNameString(), reader.GetTypeComments(type));

                    foreach (var field in type.GetFieldsForRendering())                    
                        Comments.Add(field.FieldId(), reader.GetMemberComments(field));
                    foreach (var method in type.GetMethodsForRendering())
                        Comments.Add(method.MethodId(), reader.GetMethodComments(method));
                    foreach (var prop in type.GetPropertiesForRendering())
                        Comments.Add(prop.PropertyId(), reader.GetMemberComments(prop));
                    foreach (var _event in type.GetEventsForRendering())
                        Comments.Add(_event.EventId(), reader.GetMemberComments(_event));
                }
            }
            Console.WriteLine();
        }

        public void Render()
        {
            foreach (var proj in Projects.Values)
                foreach (var type in proj.Assembly.ExportedTypes)
                {                    
                    // Begin rendering to buffer
                    RenderType(type, RenderHelper.Builder);
                    // Write buffer into file
                    Output.Write(type, RenderHelper.Builder);
                    // Clear re-used string builder
                    RenderHelper.Builder.Clear();
                }
        }

        public void RenderType(Type type, StringBuilder builder)
        {
            try
            {
                // Class Name
                type.Name.PutMarkdownHeader(HeaderVariant.H1, false);
                if (type.BaseType is not null)
                    builder.Append($" : {type.BaseType.AsMaybeLink()}");
                Markdown.DEFAULT_SPACING.Put();                

                if (Comments.TryGetValue(type.ToNameString(), out CommonComments value))
                {
                    value.Summary.Put();
                    Markdown.DEFAULT_SPACING.Put();
                }

                "Exported Fields".PutMarkdownHeader(HeaderVariant.H2);
                type.GetFieldsForRendering().ToMarkdown(m =>
                {
                    $"{m.FieldType.AsMaybeLink()} {m.Name}".PutMarkdownHeader(HeaderVariant.H3);                                          

                    return Padding.NoPadding;
                });

                "Exported Methods".PutMarkdownHeader(HeaderVariant.H2);
                type.GetMethodsForRendering().ToMarkdown(m =>
                {
                    $"{m.ReturnType.AsMaybeLink()} {m.Name}".PutMarkdownHeader(HeaderVariant.H3, false);
                   
                    // Create parameter listing
                    builder.Append(m.GetParameters().AsMarkdownParams());                    
                    return Padding.Default;
                });

                "Exported Propertes".PutMarkdownHeader(HeaderVariant.H2);
                type.GetPropertiesForRendering().ToMarkdown(m =>
                {
                    $"{m.PropertyType.AsMaybeLink()} {m.Name}".PutMarkdownHeader(HeaderVariant.H3);
                    return Padding.NoPadding;
                });

                "Exported Events".PutMarkdownHeader(HeaderVariant.H2);
                type.GetEventsForRendering().ToMarkdown(m =>
                {
                    string typeStr;

                    if (m.EventHandlerType.GenericTypeArguments.Length != 0)
                        typeStr = $"{m.EventHandlerType.ToNameString()}"
                            .AsCode();
                    else
                        typeStr = m.EventHandlerType.Name
                            .AsNameWithoutGenericInfo()
                            .AsCode();

                    $"{typeStr} {m.Name}".PutMarkdownHeader(HeaderVariant.H3);
                    return Padding.NoPadding;
                });                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }      
    
    public static class Filtering
    {
        const BindingFlags DEFAULT_SEARCH = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;

        public static IEnumerable<FieldInfo> GetFieldsForRendering(this Type type)
            => type.GetFields(DEFAULT_SEARCH);

        public static IEnumerable<MethodInfo> GetMethodsForRendering(this Type type)
            => type.GetMethods(DEFAULT_SEARCH).Where(m => !m.Attributes.HasFlag(MethodAttributes.SpecialName));

        public static IEnumerable<PropertyInfo> GetPropertiesForRendering(this Type type)
            => type.GetProperties(DEFAULT_SEARCH);        

        public static IEnumerable<EventInfo> GetEventsForRendering(this Type type)
            => type.GetEvents(DEFAULT_SEARCH);
    }
}