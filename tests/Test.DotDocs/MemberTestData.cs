using DotDocs.Markdown;
using DotDocs.Markdown.Extensions;
using System.Reflection;
using Test.DotDocs.Source.One;
using Test.DotDocs.Source.One.TypeNames;

namespace Test.DotDocs
{
    public abstract class MemberTestSource<TMember>
        where TMember : MemberInfo
    {
        public Type Source { get; init; }

        protected MemberTestSource(Type srcType)
        {
            Source = TestState.GetECMADefinitionTypeFromRuntimeType(srcType);
        }

        protected abstract IEnumerable<string[]> YieldNames();

        public abstract TMember Get(string name);        
    }

    public class PropertyTestSource : MemberTestSource<PropertyInfo>
    {
        readonly static Type SRC_TYPE = typeof(MyProperties<,>);

        public PropertyTestSource() : base(SRC_TYPE) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Source.GetPropertiesForRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override PropertyInfo Get(string name)
        {
            // Cannot use Source.GetRuntimeProperty or GetProperty.. for some reason it will not return the prop, but the following does
            var props = Source.GetPropertiesForRendering();
            return props.Single(p => p.Name == name);
        }

        public static IEnumerable<string[]> GetNames()
        {
            var src = new PropertyTestSource();
            return src.YieldNames();
        }
    }

    public class FieldTestSource : MemberTestSource<FieldInfo>
    {
        readonly static Type SRC_TYPE = typeof(MyFields);

        public FieldTestSource() : base(SRC_TYPE) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Source.GetFieldsForTypeRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override FieldInfo Get(string name)
        {
            return Source.GetField(name);
        }

        public static IEnumerable<string[]> GetNames()
        {
            var src = new FieldTestSource();
            return src.YieldNames();
        }
    }

    public class MethodTestSource : MemberTestSource<MethodInfo>
    {
        readonly static Type SRC_TYPE = typeof(MyMethods);

        public MethodTestSource() : base(SRC_TYPE) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Source.GetMethodsForRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override MethodInfo Get(string name)
        {
            return Source.GetMethod(name);
        }

        public static IEnumerable<string[]> GetNames()
        {
            var src = new MethodTestSource();
            return src.YieldNames();
        }
    }

    public class EventTestSource : MemberTestSource<EventInfo>
    {
        readonly static Type SRC_TYPE = typeof(MyEvents);

        public EventTestSource() : base(SRC_TYPE) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Source.GetEventsForRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override EventInfo Get(string name)
        {
            return Source.GetEvent(name);
        }

        public static IEnumerable<string[]> GetNames()
        {
            var src = new EventTestSource();
            return src.YieldNames();
        }
    }

    public class TypeNamingTestSource : MemberTestSource<Type>
    {
        readonly static Type SRC_TYPE = typeof(TypeNames<,>);

        public TypeNamingTestSource() : base(SRC_TYPE) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Source.GetFieldsForTypeRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override Type Get(string name)
        {
            return Source.GetField(name).FieldType;
        }

        public static IEnumerable<string[]> GetNames()
        {
            var src = new TypeNamingTestSource();
            return src.YieldNames();
        }
    }

    //public class EnumTestSource : MemberTestSource<FieldInfo>
    //{
    //    public EnumTestSource() : base(typeof(MyProperties<,>)) { }

    //    public override IEnumerable<string[]> GetNames()
    //    {
    //        var members = src.GetFieldsForEnumRendering();
    //        foreach (var mem in members)
    //            yield return new[] { mem.Name };
    //    }

    //    public override FieldInfo Get(string name)
    //    {
    //        return src.GetField(name);
    //    }
    //}       
}
