namespace Test.DotDocs.Source.One
{
    /// <summary>
    /// Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
    /// </summary>
    public class MyProperties<T, K>
    {
        /// <include file='../Assets/Docs.xml' path='doc/member[@name="Summary"]/*'/>
        public int MyProperty01 { get; set; }
        public int MyProperty02 { get; }
        public int MyProperty03 { set { } }
        public int MyProperty04 { get; private set; }
        public int MyProperty05 { get; protected set; }
        public int MyProperty06 { get; internal set; }        
        public int MyProperty07 { private get; set; }
        public int MyProperty08 { protected get; set; }
        public int MyProperty09 { internal get; set; }
    }
}