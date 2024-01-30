using DotDocs.IO;
using LoxSmoke.DocXml;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown
{
    public static class RenderState
    {
        const int DEFAULT_STR_BUILDER_CAPACITY = 2048;

        // The following state makes this class operate as a state machine reducing the times we must reference the StringBuilder when rendering
        #region State
        public static StringBuilder Builder { get; private set; } = new(DEFAULT_STR_BUILDER_CAPACITY);
        public static ImmutableDictionary<string, Assembly>? Assemblies { get; private set; }
        public static ImmutableDictionary<string, CommonComments>? Comments { get; private set; }
        public static IOutputable? Output { get; private set; }
        #endregion

        public static void UpdateState(
            ImmutableDictionary<string, Assembly> assemblies, 
            ImmutableDictionary<string, CommonComments> comments,
            IOutputable output)
        {
            Assemblies = assemblies;
            Comments = comments;
            Output = output;
        }                
    }
}
