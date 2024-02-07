using DotDocs.Markdown;
using System.Reflection;
using Test.DotDocs.Source.One;
using Test.DotDocs.Source.One.TypeNames;

namespace Test.DotDocs
{
    public abstract class MemberTestSource<TMember>
        where TMember : MemberInfo
    {
        public Type Src { get; private set; }

        protected MemberTestSource(Type type) => Src = type;

        protected abstract IEnumerable<string[]> YieldNames();

        public abstract TMember Get(string name);        
    }

    public class PropertyTestSource : MemberTestSource<PropertyInfo>
    {
        public PropertyTestSource() : base(typeof(MyProperties<,>)) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Src.GetPropertiesForRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override PropertyInfo Get(string name)
        {
            return Src.GetProperty(name);
        }

        public static IEnumerable<string[]> GetNames()
        {
            var src = new PropertyTestSource();
            return src.YieldNames();
        }
    }

    public class FieldTestSource : MemberTestSource<FieldInfo>
    {
        public FieldTestSource() : base(typeof(MyFields)) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Src.GetFieldsForTypeRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override FieldInfo Get(string name)
        {
            return Src.GetField(name);
        }

        public static IEnumerable<string[]> GetNames()
        {
            var src = new FieldTestSource();
            return src.YieldNames();
        }
    }

    public class MethodTestSource : MemberTestSource<MethodInfo>
    {
        public MethodTestSource() : base(typeof(MyMethods)) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Src.GetMethodsForRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override MethodInfo Get(string name)
        {
            return Src.GetMethod(name);
        }

        public static IEnumerable<string[]> GetNames()
        {
            var src = new MethodTestSource();
            return src.YieldNames();
        }
    }

    public class EventTestSource : MemberTestSource<EventInfo>
    {
        public EventTestSource() : base(typeof(MyEvents)) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Src.GetEventsForRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override EventInfo Get(string name)
        {
            return Src.GetEvent(name);
        }

        public static IEnumerable<string[]> GetNames()
        {
            var src = new EventTestSource();
            return src.YieldNames();
        }
    }

    public class TypeNamingTestSource : MemberTestSource<Type>
    {
        public TypeNamingTestSource() : base(typeof(TypeNames<,>)) { }

        protected override IEnumerable<string[]> YieldNames()
        {
            var members = Src.GetFieldsForTypeRendering();
            foreach (var mem in members)
                yield return new[] { mem.Name };
        }

        public override Type Get(string name)
        {
            return Src.GetField(name).FieldType;
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
