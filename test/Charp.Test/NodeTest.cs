using Charp.Core.Tree;
using Charp.Test.Interfaces.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charp.Test
{
    internal class NodeTest : BaseTest, ITypeNodeNestableTest
    {
        [Test(Description = "Ensures nested types accounted for.")]
        public void ContainedTypesExist()
        {
            // Boat
            TypeNodeNestable node = GetNodeType("Boat");
            Assert.IsEmpty(node.Types);

            // Canoe
            node = GetNodeType("Canoe");
            Assert.IsNotEmpty(node.Types);
            Assert.AreEqual(
                (node.Types.Values.First() as Node).GetName(),
                "Builder");
        }

        [Test(Description = "Ensures every name (key) used to search matches the type name.")]
        public void GetNameMatchesTypeName()
        {
            // Boat
            TypeNodeNestable node = GetNodeType("Boat");
            Assert.AreEqual(
                "Boat",
                node.GetName());

            // Canoe
            node = GetNodeType("Canoe");
            Assert.AreEqual(
                "Canoe",
                node.GetName());

            // Runabout
            node = GetNodeType("Runabout");
            Assert.AreEqual(
                "Runabout",
                node.GetName());

            // Sailboat
            node = GetNodeType("Sailboat");
            Assert.AreEqual(
                "Sailboat",
                node.GetName());

            // Yacht
            node = GetNodeType("Yacht");
            Assert.AreEqual(
                "Yacht",
                node.GetName());
        }

        [Test(Description = "Ensures every node has member information attached to it.")]
        public void MemberIsNotNull()
        {
            // Boat
            TypeNodeNestable node = GetNodeType("Boat");
            Assert.NotNull(node.Member);

            // Canoe
            node = GetNodeType("Canoe");
            Assert.NotNull(node.Member);

            // Runabout
            node = GetNodeType("Runabout");
            Assert.NotNull(node.Member);

            // Sailboat
            node = GetNodeType("Sailboat");
            Assert.NotNull(node.Member);

            // Yacht
            node = GetNodeType("Yacht");
            Assert.NotNull(node.Member);
        }

        [Test(Description = "Ensures all parents of types are not null.")]
        public void ParentNodeNotNull()
        {
            // Boat
            TypeNodeNestable node = GetNodeType("Boat");
            Assert.NotNull(node.Parent);

            // Canoe
            node = GetNodeType("Canoe");
            Assert.NotNull(node.Parent);

            // Runabout
            node = GetNodeType("Runabout");
            Assert.NotNull(node.Parent);

            // Sailboat
            node = GetNodeType("Sailboat");
            Assert.NotNull(node.Parent);

            // Yacht
            node = GetNodeType("Yacht");
            Assert.NotNull(node.Parent);
        }

        [Test]
        public void FindType()
        {
            Assert.AreSame(
                Docs.Models.FindType("Charp.Test.Data.Classes.Boat"),
                Docs.ReflectedMetadata.Classes["Charp.Test.Data.Classes.Boat"]);
        }

        public TypeNodeNestable GetNodeType(string name)
            => Docs.Models.Root
               .Namespaces["Test"]
               .Namespaces["Data"]
               .Namespaces["Classes"]
               .Types[name] as TypeNodeNestable;
    }
}
