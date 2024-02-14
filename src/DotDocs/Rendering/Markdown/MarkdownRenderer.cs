using DocXml.Reflection;
using DotDocs.IO;
using DotDocs.Markdown.Enums;
using DotDocs.Markdown.Extensions;
using DotDocs.Markdown.Renderers.Aggregators;
using DotDocs.Markdown.Renderers.Components;
using DotDocs.Markdown.Renderers.Members;
using DotDocs.Markdown.Renderers.Sections;
using DotDocs.Markdown.Renderers.Sections.Header;
using DotDocs.Markdown.Renderers.Types;
using DotDocs.Models;
using DotDocs.Rendering;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown
{
    public class MarkdownRenderer : IRenderer
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region Data Sources
        public ImmutableArray<AssemblyReflectInfo> Assemblies { get; set; }

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

        /// <summary>
        /// Creates an instance of <see cref="MarkdownRenderer"/> with render options.
        /// </summary>
        /// <param name="output"></param>
        /// <param name="classRenderer"></param>
        /// <param name="interfaceRenderer"></param>
        /// <param name="structRenderer"></param>
        /// <param name="enumRenderer"></param>
        /// <param name="delegateRenderer"></param>
        public MarkdownRenderer(
            IOutputable output,
            ClassRenderer classRenderer,
            InterfaceRenderer interfaceRenderer,
            StructRenderer structRenderer,
            EnumRenderer enumRenderer,
            DelegateRenderer delegateRenderer
            ) {
            Logger.Trace("Params: {0}: {1}, {2}: {2}, {3}: {4}, {5}: {6}, {7}: {8}, {9}: {10}", 
                ToString(), output.ToString(),
                nameof(classRenderer), classRenderer,
                nameof(interfaceRenderer), interfaceRenderer,
                nameof(structRenderer), structRenderer,
                nameof(enumRenderer), enumRenderer,
                nameof(delegateRenderer), delegateRenderer);
            Output = output;
            ClassRenderer = classRenderer;
            InterfaceRenderer = interfaceRenderer;
            StructRenderer = structRenderer;
            EnumRenderer = enumRenderer;
            DelegateRenderer = delegateRenderer;

            // Wire up handlers for rendering request
            RenderClass += ClassRenderer.Render;
            RenderInterface += InterfaceRenderer.Render;
            RenderStruct += StructRenderer.Render;
            RenderEnum += EnumRenderer.Render;
            RenderDelegate += DelegateRenderer.Render;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MarkdownRenderer"/> with default render values.
        /// </summary>
        /// <param name="output"></param>
        public MarkdownRenderer(IOutputable output)
        {
            Logger.Trace("Params: {outputLbl}: {outputValue}", ToString(), output.ToString());
            Output = output;       

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

        /// <summary>
        /// Initializes this <see cref="MarkdownRenderer"/> with essential information gathered from the build phase.
        /// </summary>
        /// <param name="assemblies"></param>
        public void Init(ImmutableArray<AssemblyReflectInfo> assemblies) 
        {
            Logger.Trace("Initializing the {renderer}.", ToString());
            Assemblies = assemblies;

            // Ref to .xml reader for comments
            DocXmlReader? reader = null;

            // Used for getting fields differently if for enum or other types
            var getFields = (Type type, Func<IEnumerable<FieldInfo>> getFields) =>
            {
                foreach (var field in getFields())
                    Comments.Add(field.FieldId(), reader!.GetMemberComments(field));
            };

            // Loop to create all comments
            foreach (var info in Assemblies)
            {
                // Load XML documentation file
                reader = new DocXmlReader(info.AbsoluteDocPath);
                foreach (var type in info.Binary.ExportedTypes)
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
            State.UpdateState(
                Assemblies.ToImmutableDictionary(info => info.Binary.Location),
                Comments.ToImmutableDictionary(),
                Output);
        }

        /// <summary>
        /// The main render method called when rendering shall begin.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Render()
        {
            var multicastDel = typeof(MulticastDelegate);

            // Loop through all projects and all their exported types
            foreach (var info in Assemblies)
                foreach (var type in info.Binary.ExportedTypes)
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
                        throw new Exception($"The type {type} did not match any known C# construct.");
                    // Write buffer into file
                    Output.Write(type, State.Builder);
                    // Clear re-used string builder
                    State.Builder.Clear();
                }
        }
    }            
}