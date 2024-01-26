using DocXml.Reflection;
using DotDocs.IO;
using DotDocs.IO.Routing;
using DotDocs.Models;
using DotDocs.Render;
using LoxSmoke.DocXml;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.PortableExecutable;
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

            // Ref to .xml reader for comments
            DocXmlReader reader = null;

            // Used for getting fields differently if for enum or other types
            var getFields = (Type type, Func<IEnumerable<FieldInfo>> getFields) =>
            {
                foreach (var field in getFields())
                    Comments.Add(field.FieldId(), reader!.GetMemberComments(field));
            };

            // Loop to create all comments
            foreach (var proj in Projects.Values)
            {
                // Load XML documentation file
                reader = new DocXmlReader(proj.DocumentationFilePath);
                foreach (var type in proj.Assembly.ExportedTypes)
                {
                    // Create key/value for type comment and it's member comments
                    Comments.Add(type.TypeId(), reader.GetTypeComments(type));

                    // LOAD COMMENTS FOR TYPE

                    // If enum, load fields differently than other types and skip other members
                    if (type.IsEnum)
                    {
                        getFields(type, type.GetFieldsForEnumRendering);
                        break;
                    }                        
                    else // Load fields normally for non-enum types
                        getFields(type, type.GetFieldsForTypeRendering);
                    
                    // Load other comments too
                    foreach (var method in type.GetMethodsForRendering())
                        Comments.Add(method.MethodId(), reader.GetMethodComments(method));
                    foreach (var prop in type.GetPropertiesForRendering())
                        Comments.Add(prop.PropertyId(), reader.GetMemberComments(prop));
                    foreach (var _event in type.GetEventsForRendering())
                        Comments.Add(_event.EventId(), reader.GetMemberComments(_event));
                }
            }

            // Update render state so rendering may occur
            RenderState.UpdateState(
                new StringBuilder(DEFAULT_STR_BUILDER_CAPACITY),
                Projects.ToImmutableDictionary(k => k.Value.Assembly.FullName, v => v.Value.Assembly),
                Comments.ToImmutableDictionary(),
                Output);
        }

        public void Render()
        {
            var builder = RenderState.Builder;

            foreach (var proj in Projects.Values)
                foreach (var type in proj.Assembly.ExportedTypes)
                {
                    if (type.IsEnum)
                    { 
                        // Render enums here
                        RenderEnum(type, builder);
                    }
                    else
                    {
                        // Render other types here
                        RenderType(type, builder);
                    }                    
                    // Write buffer into file
                    Output.Write(type, builder);
                    // Clear re-used string builder
                    builder.Clear();
                }
        }

        public void RenderEnum(Type type, StringBuilder builder)
        {
            try
            {
#if DEBUG
                if (!type.IsEnum)
                    throw new Exception($"Type ${type.FullName} is not an enum and shouldn't be rendered by {nameof(RenderEnum)}.");
#endif
                RenderHeader(type, builder);

                type.GetFieldsForEnumRendering().ToMarkdown(
                    before: delegate
                    {
                        "Values:".PutMarkdownHeader(HeaderVariant.H2);
                    },
                    each: m =>
                    {
                        m.Name.AsListItem().Put();
                        ", ".Put();
                        m.PutComments(false);

                        Markdown.NEW_LINE.Put();
                    });
            }
            catch
            {
                throw;
            }
        }

        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //  -- FIGUREOUT HOW WE ARE GOING TO RE-USE LARGER RENDER METHODS WHEN DEALING WITH
        //      CLASSES, ENUMS, STRUCTS, and INTERFACES
        //
        //
        //
        //
        //  -- Devise a better system tha Markdown.****.Put(); for putting padding..
        //      Maybe use an enum so we can standarize with parameter types elsewhere
        //
        //  -- PutComments() needs options for following newlines or not
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        void RenderHeader(Type type, StringBuilder builder)
        {
            // Class Name
            type.Name.PutMarkdownHeader(HeaderVariant.H1, false);
            // Render the base type of the class when it's nearest parent is not System.Object
            if (type.BaseType is not null && type.BaseType?.BaseType is not null)
                builder.Append($" {"extends".AsItalic()} {type.BaseType.AsMaybeLink()}");
            Markdown.DOUBLE_NEW_LINE.Put();

            // Render Type's Comment
            type.PutComments();
        }

        public void RenderType(Type type, StringBuilder builder)
        {
            try
            {
#if DEBUG
                if (type.IsEnum)
                {
                    throw new Exception($"Enum ${type.FullName} was attempted to be rendered by the {nameof(RenderType)} method. It is required enumerations are rendered by {nameof(RenderEnum)} instead.");
                }
#endif
                RenderHeader(type, builder);

                // Render Exported Fields
                type.GetFieldsForTypeRendering().ToMarkdown(
                    before: delegate {
                        "Exported Fields".PutMarkdownHeader(HeaderVariant.H2);
                    }, 
                    each: m => {
                        $"{m.FieldType.AsMaybeLink()} {m.Name}".PutMarkdownHeader(HeaderVariant.H3);

                        m.PutComments();
                    });

                // Render Exported Methods                
                type.GetMethodsForRendering().ToMarkdown(
                    before: delegate
                    {
                        "Exported Methods".PutMarkdownHeader(HeaderVariant.H2);
                    },
                    each: m =>
                    {
                        $"{m.ReturnType.AsMaybeLink()} {m.Name}".PutMarkdownHeader(HeaderVariant.H3, false);

                        // Create parameter listing
                        builder.Append(m.GetParameters().AsMarkdownParams());
                        Markdown.DOUBLE_NEW_LINE.Put();

                        m.PutComments();
                    });

                // Render Exported Propertie                
                type.GetPropertiesForRendering().ToMarkdown(
                    before: delegate
                    {
                        "Exported Propertes".PutMarkdownHeader(HeaderVariant.H2);
                    }, 
                    each: m =>
                    {
                        $"{m.PropertyType.AsMaybeLink()} {m.Name}".PutMarkdownHeader(HeaderVariant.H3);
                        m.PutComments();
                    });

                // Render Events                
                type.GetEventsForRendering().ToMarkdown(
                    before: delegate
                    {
                        "Exported Events".PutMarkdownHeader(HeaderVariant.H2);
                    }, 
                    each: m =>
                    {
                        $"{m.EventHandlerType.AsMaybeLink()} {m.Name}".PutMarkdownHeader(HeaderVariant.H3);

                        m.PutComments();
                    });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }      
    
    /// <summary>
    /// Filtering functions to be used when querying <see cref="Type"/> members.
    /// </summary>
    public static class Filtering
    {
        const BindingFlags DEFAULT_SEARCH = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;

        public static IEnumerable<FieldInfo> GetFieldsForEnumRendering(this Type type)
        {
#if DEBUG
            if (!type.IsEnum)
                throw new Exception($"Method {nameof(GetFieldsForEnumRendering)} was called on non enum type {type.FullName}, use {nameof(GetFieldsForTypeRendering)} instead.");
#endif
            // Avoid the value__ field generated for enums
            return type.GetFields().Where(m => !m.Attributes.HasFlag(FieldAttributes.SpecialName));
        }

        public static IEnumerable<FieldInfo> GetFieldsForTypeRendering(this Type type)
        {
#if DEBUG
            if (type.IsEnum)
                throw new Exception($"Method {nameof(GetFieldsForTypeRendering)} was called on enum {type.FullName}, use {nameof(GetFieldsForEnumRendering)} instead.");
#endif
            // Avoid compiler generate fields for backing properties
            return type.GetFields(DEFAULT_SEARCH).Where(m => !m.Attributes.HasFlag(FieldAttributes.SpecialName));
        }

        public static IEnumerable<MethodInfo> GetMethodsForRendering(this Type type)
            // Avoid backing property setter/getters amongst others
            => type.GetMethods(DEFAULT_SEARCH).Where(m => !m.Attributes.HasFlag(MethodAttributes.SpecialName));

        public static IEnumerable<PropertyInfo> GetPropertiesForRendering(this Type type)
            => type.GetProperties(DEFAULT_SEARCH);        

        public static IEnumerable<EventInfo> GetEventsForRendering(this Type type)
            => type.GetEvents(DEFAULT_SEARCH);
    }
}