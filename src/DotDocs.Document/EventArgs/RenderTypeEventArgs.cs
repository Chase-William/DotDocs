namespace DotDocs.Render.Args
{
    public class RenderTypeEventArgs
    {
        public Type Type { get; init; }

        public RenderTypeEventArgs(Type type)
            => Type = type;
    }
}
