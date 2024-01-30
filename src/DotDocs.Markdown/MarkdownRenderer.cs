using DocXml.Reflection;
using DotDocs.IO;
using DotDocs.IO.Routing;
using DotDocs.Markdown.Components;
using DotDocs.Markdown.Enums;
using DotDocs.Markdown.SubRenderers;
using DotDocs.Models;
using DotDocs.Render;
using LoxSmoke.DocXml;
using System.Collections.Immutable;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;

namespace DotDocs.Markdown
{    
    public class MarkdownRenderer : IRenderer
    {        
        #region Data Sources
        public RepositoryModel Model { get; set; }

        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }

        public Dictionary<string, CommonComments> Comments { get; set; } = new();
        #endregion

        public IOutputable Output { get; set; }

        public MethodRenderer MethodRenderer { get; init; }
        public PropertyRenderer PropertyRenderer { get; init; }
        public EventRenderer EventRenderer { get; init; }
        public FieldRenderer FieldRenderer { get; init; }

        public MarkdownRenderer(IOutputable output)
        {
            Output = output;
            MethodRenderer = new MethodRenderer(new MethodDeclaration());
            PropertyRenderer = new PropertyRenderer(new PropertyDeclaration());
            EventRenderer = new EventRenderer(new EventCodeBlockDeclaration());
            FieldRenderer = new FieldRenderer(new FieldCodeBlockDeclaration());
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
                Projects.ToImmutableDictionary(k => k.Value.Assembly.FullName!, v => v.Value.Assembly),
                Comments.ToImmutableDictionary(),
                Output);
        }

        public void Render()
        {
            var builder = RenderState.Builder;

            var multicastDel = typeof(MulticastDelegate);

            foreach (var proj in Projects.Values)
                foreach (var type in proj.Assembly.ExportedTypes)
                {
                    if (type.IsClass)
                        RenderClass(type, builder);
                    else if (type.IsEnum)
                        RenderEnum(type, builder);
                    else if (type.IsInterface)
                        RenderInterface(type, builder);
                    else if (type.IsValueType)
                        RenderStruct(type, builder);
                    else if (type.BaseType?.Name.Equals(multicastDel.Name) ?? false)
                        RenderDelegate(type, builder);
                    else
                        throw new Exception($"The type {type.ToNameString()} did not match any known C# construct.");
                    // Write buffer into file
                    Output.Write(type, builder);
                    // Clear re-used string builder
                    builder.Clear();
                }
        }        

        public void RenderClass(Type type, StringBuilder builder)
        {
            try
            {
#if DEBUG
                if (type.IsEnum)
                {
                    throw new Exception($"Enum ${type.FullName} was attempted to be rendered by the {nameof(RenderClass)} method. It is required enumerations are rendered by {nameof(RenderEnum)} instead.");
                }
#endif                
                RenderHeader(type, builder);

                // Render Exported Fields
                type.GetFieldsForTypeRendering().ToMarkdown(
                    before: delegate {
                        AsMarkdown.H2.Prefix("Public Fields", padding: Padding.DoubleNewLine);
                    }, 
                    each: FieldRenderer.Render);

                // Render Exported Methods                
                type.GetMethodsForRendering().ToMarkdown(
                    before: delegate
                    {
                        AsMarkdown.H2.Prefix("Public Methods", padding: Padding.DoubleNewLine);
                    },
                    each: MethodRenderer.Render);

                // Render Exported Properties
                type.GetPropertiesForRendering().ToMarkdown(
                    before: delegate
                    {
                        AsMarkdown.H2.Prefix("Public Properties", padding: Padding.DoubleNewLine);
                    }, 
                    each: PropertyRenderer.Render);

                // Render Events                
                type.GetEventsForRendering().ToMarkdown(
                    before: delegate
                    {
                        AsMarkdown.H2.Prefix("Public Events", padding: Padding.DoubleNewLine);
                    }, 
                    each: EventRenderer.Render);
            }
            catch (Exception ex)
            {
                throw;
            }
        }        

        public void RenderStruct(Type type, StringBuilder builder)
        {
            throw new NotImplementedException();
        }

        public void RenderInterface(Type type, StringBuilder builder)
        {
            throw new NotImplementedException();
        }

        public void RenderDelegate(Type type, StringBuilder builder)
        {
            throw new NotImplementedException();
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
                        AsMarkdown.H2.Prefix("Values", padding: Padding.DoubleNewLine);
                    },
                    each: m =>
                    {
                        // m.Name.AsListItem().Put();                        
                        // m.PutComments(", ", LinePadding.NewLine);                        
                    });
            }
            catch
            {
                throw;
            }
        }

        void RenderHeader(Type type, StringBuilder builder)
        {
            // Class Name
            AsMarkdown.H1.Prefix(type.Name);
            // Render the base type of the class when it's nearest parent is not System.Object
            if (type.BaseType is not null && type.BaseType?.BaseType is not null)
            {
                Padding.Space.Put();
                AsMarkdown.Italic.Wrap("extends", Padding.Space);
                type.BaseType.ToNameString().Put();
                // builder.Append($" {"extends".AsItalic()} {type.BaseType.AsMaybeLink()}");
            }
            Padding.DoubleNewLine.Put();

            // Put comments for type if they exist
            if (type.TryGetComments(out TypeComments? comments))
            {
                ArgumentNullException.ThrowIfNull(comments);

                type.PutSummary(comments);
                type.PutExample(comments);
                type.PutRemarks(comments);
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
        {
            // Using GetRuntimeProperties as properties with *private get* and *public set* are deemed private even though one could set the value from a another assembly (sound public enough to me)
            return type.GetRuntimeProperties();
        }  

        public static IEnumerable<EventInfo> GetEventsForRendering(this Type type)
            => type.GetEvents(DEFAULT_SEARCH);
    }
}