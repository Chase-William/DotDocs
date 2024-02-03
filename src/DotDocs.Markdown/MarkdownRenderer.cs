using DocXml.Reflection;
using DotDocs.IO;
using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Renderers.Aggregators;
using DotDocs.Markdown.Renderers.Components;
using DotDocs.Markdown.Renderers.Members;
using DotDocs.Markdown.Renderers.Sections;
using DotDocs.Markdown.Renderers.Sections.Header;
using DotDocs.Markdown.Renderers.Types;
using DotDocs.Models;
using DotDocs.Render;
using LoxSmoke.DocXml;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;

namespace DotDocs.Markdown
{
    public class MarkdownRenderer : IRenderer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region Data Sources
        public RepositoryModel Model { get; set; }

        public ImmutableDictionary<string, ProjectModel> Projects { get; set; }

        public Dictionary<string, CommonComments> Comments { get; set; } = new();
        #endregion

        public event RenderType RenderClass;

        public event RenderType RenderStruct;

        public event RenderType RenderInterface;

        public event RenderType RenderDelegate;

        public event RenderType RenderEnum;

        public IOutputable Output { get; set; }

        #region Type Renderers
        public ITypeRenderer ClassRenderer { get; init; }
        public ITypeRenderer InterfaceRenderer { get; init; }
        public ITypeRenderer StructRenderer { get; init; }
        public ITypeRenderer EnumRenderer { get; init; }
        public ITypeRenderer DelegateRenderer { get; init; }
        #endregion

        public MarkdownRenderer(IOutputable output)
        {
            Logger.Trace("Creating an instance of {renderer} with {output}", ToString(), output.ToString());
            Output = output;           

            // How do we know if we can share an instance of a renderer or if it contains state that we don't want to share...?

            // Create re-useable member aggregators, member renderers, and their compontent renderers
            var fields = new MemberAggregator(
                new FieldRenderer(new FieldDeclaration()),
                t => t.GetFieldsForTypeRendering(),
                headerRenderer: new StaticTitle("Public Fields", AsMarkdown.H2));
            
            var methods = new MemberAggregator(
                new MethodRenderer(new MethodDeclaration()), 
                t => t.GetMethodsForRendering(),
                headerRenderer: new StaticTitle("Public Methods", AsMarkdown.H2));

            var properties = new MemberAggregator(
                new PropertyRenderer(new PropertyDeclaration(), true),
                t => t.GetPropertiesForRendering(),
                headerRenderer: new StaticTitle("Public Properties", AsMarkdown.H2));

            var events = new MemberAggregator(
                new EventRenderer(new EventDeclaration()),
                t => t.GetEventsForRendering(),
                headerRenderer: new StaticTitle("Public Events", AsMarkdown.H2));

            var typeHeader = new TypeHeader();

            // Create type renderers
            // order of given params determines render order
            ClassRenderer = new ClassRenderer(typeHeader, fields, methods, properties, events);
            InterfaceRenderer = new InterfaceRenderer(typeHeader, methods, properties, events); // Don't render interface fields (cannot exist)
            StructRenderer = new StructRenderer(typeHeader, fields, methods, properties, events);
            EnumRenderer = new EnumRenderer(
                typeHeader,
                new MemberAggregator(
                    new EnumValueRenderer(), 
                    t => t.GetFieldsForEnumRendering(), 
                    headerRenderer: new StaticTitle("Values", AsMarkdown.H2))
                );
            DelegateRenderer = new DelegateRenderer(typeHeader);

            // Wire up handlers for rendering request
            RenderClass += ClassRenderer.Render;
            RenderInterface += InterfaceRenderer.Render;
            RenderStruct += StructRenderer.Render;
            RenderEnum += EnumRenderer.Render;
            RenderDelegate += DelegateRenderer.Render;
        }        

        public void Init(
            RepositoryModel model,
            ImmutableDictionary<string, ProjectModel> projects
            ) {
            Logger.Trace("Initializing the {renderer}.", ToString());
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
                    if (type.BaseType?.Name.Equals(multicastDel.Name) ?? false)
                        RenderDelegate?.Invoke(type);
                    else if (type.IsEnum)
                        RenderEnum?.Invoke(type);
                    else if (type.IsClass)
                        RenderClass?.Invoke(type);
                    else if (type.IsInterface)
                        RenderInterface?.Invoke(type);
                    else if (type.IsValueType)
                        RenderStruct?.Invoke(type);
                    else
                        throw new Exception($"The type {type.ToNameString()} did not match any known C# construct.");
                    // Write buffer into file
                    Output.Write(type, builder);
                    // Clear re-used string builder
                    builder.Clear();
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