using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Charp.Core.Models;
using LoxSmoke.DocXml;

namespace Charp.Core.Tree
{
    /// <summary>
    /// A tree of namespaces and types created lazily that represents the structure of one's source code.
    /// </summary>
    public class ModelTree
    {
        public NamespaceNode Root { get; private set; }

        public ModelTree() { }

        /// <summary>
        /// Adds a new type under the provided namespace.
        /// </summary>
        /// <param name="fullNamespaceToType"></param>
        public void AddType(string fullNamespaceToType, TypeMember<TypeInfo, TypeComments> member)
        {
            // We know the last item in this string[] is our type
            // Everything before it, a namespace            
            var segments = fullNamespaceToType.Split(".");

            if (Root == null)
                Root = new NamespaceNode(null, segments[0]);

            Root.AddType(segments[1..], member);
        }

        public TypeMember<TypeInfo, TypeComments> FindType(string fullName)
        {
            ArraySegment<string> segments = fullName.Split('.');
            return Root.Namespaces[segments[1]].FindType(segments[2..]);
        }

        public Member<FieldInfo, CommonComments> FindField(string fullName)
        {
            ArraySegment<string> segments = fullName.Split('.');
            return Root.Namespaces[segments[1]].FindField(segments[2..]);
        }

        public Member<PropertyInfo, CommonComments> FindProperty(string fullName)
        {
            ArraySegment<string> segments = fullName.Split('.');
            return Root.Namespaces[segments[1]].FindProperty(segments[2..]);
        }

        public Member<EventInfo, CommonComments> FindEvent(string fullName)
        {
            ArraySegment<string> segments = fullName.Split('.');
            return Root.Namespaces[segments[1]].FindEvent(segments[2..]);
        }

        public void SaveModels()
            => Root.Save(new Stack<string>(), new Stack<string>());
    }
}
