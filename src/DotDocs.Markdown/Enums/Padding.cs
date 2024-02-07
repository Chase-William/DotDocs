using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown.Enums
{
    /// <summary>
    /// Padding values often used when rendering Markdown.
    /// </summary>
    public enum Padding
    {
        /// <summary>
        /// No padding.
        /// </summary>
        None,
        /// <summary>
        /// One new line character.
        /// </summary>
        NewLine,
        /// <summary>
        /// Two new line characters; often used when starting a new section.
        /// </summary>
        DoubleNewLine,
        /// <summary>
        /// A single space character.
        /// </summary>
        Space
    }

    public static class PaddingEx
    {
        const string NEW_LINE = "\r\n";
        const string DOUBLE_NEW_LINE = "\r\n\r\n";
        const char SPACE = ' ';

        public static void Put(this Padding padding)
        {
            switch (padding)
            {
                case Padding.DoubleNewLine:
                    State.Builder.Append(DOUBLE_NEW_LINE);
                    break;
                case Padding.NewLine:
                    State.Builder.Append(NEW_LINE);
                    break;
                case Padding.Space:
                    State.Builder.Append(SPACE);
                    break;
                default:
                    break;
            }
        }
    }
}
