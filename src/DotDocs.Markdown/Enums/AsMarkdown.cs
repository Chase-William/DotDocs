using System.ComponentModel;

namespace DotDocs.Markdown.Enums
{
    public enum AsMarkdown
    {
        H1 = 1,
        H2 = 2,
        H3 = 3,
        H4 = 4,
        H5 = 5,
        UnorderedListItem = 6,
        OrderedListItem = 7,
        None,
        Italic,
        Bold,
        BoldItalic,
        Code,
        CodeBlock,
        Link,
        HorizonalLine
    }

    public static class AsMarkdownEx
    {
        const string ENUM_EXCEPTION_MSG = "Value {0} is invalid for the given method {1}.";

        const string ITALIC = "*";
        const string BOLD = "**";
        const string BOLD_ITALIC = "***";
        const string CODE = "`";
        const string CODE_BLOCK = "```";
        const string UNORDERED_LIST_ITEM = "- ";
        const string ORDERED_LIST_ITEM = "1. ";
        const string H1 = "#";
        const string H2 = "##";
        const string H3 = "###";
        const string H4 = "####";
        const string H5 = "#####";
        const string HORIZONTAL_LINE = "---";

        public static void Put(this AsMarkdown style, Padding padding = Padding.None)
        {
            if (style == AsMarkdown.CodeBlock)
                throw new InvalidEnumArgumentException(string.Format(ENUM_EXCEPTION_MSG, style, nameof(Put)));

            GetAsString(style)?.Put(padding);

            // Some styles need a space after to work
            style.PutDependentSpace();
        }

        public static void Prefix(this AsMarkdown style, string str, AsMarkdown wrapStyle = AsMarkdown.None, Padding padding = Padding.None)
        {
            if (style == AsMarkdown.CodeBlock || style == AsMarkdown.HorizonalLine)
                throw new InvalidEnumArgumentException(string.Format(ENUM_EXCEPTION_MSG, style, nameof(Prefix)));
            
            style.Put();            

            wrapStyle.Wrap(str, padding);                     
        }

        public static void Wrap(this AsMarkdown style, string str, Padding padding = Padding.None)
        {
            if (style == AsMarkdown.CodeBlock)
                throw new InvalidEnumArgumentException(string.Format(ENUM_EXCEPTION_MSG, style, nameof(Wrap)));

            var wrapper = GetAsString(style);
            wrapper?.Put();
            str.Put();
            wrapper?.Put();
            padding.Put();
        }

        public static void Link(this AsMarkdown style, string text, string href, AsMarkdown wrapStyle = AsMarkdown.None, Padding padding = Padding.None)
        {
            if (style != AsMarkdown.Link)
                throw new InvalidEnumArgumentException(string.Format(ENUM_EXCEPTION_MSG, style, nameof(Link)));

            "[".Put();
            wrapStyle.Wrap(text);
            "](".Put();
            href.Put();
            ")".Put(padding);           
        }

        public static void OpenCodeBlock(this AsMarkdown style, Language lang = Language.CSharp, Padding padding = Padding.NewLine)
        {
            if (style != AsMarkdown.CodeBlock)
                throw new InvalidEnumArgumentException(string.Format(ENUM_EXCEPTION_MSG, style, nameof(OpenCodeBlock)));

            CODE_BLOCK.Put();
            lang.Put();
            padding.Put();
        }

        public static void CloseCodeBlock(this AsMarkdown style, Padding padding = Padding.DoubleNewLine)
        {
            if (style != AsMarkdown.CodeBlock)
                throw new InvalidEnumArgumentException(string.Format(ENUM_EXCEPTION_MSG, style, nameof(CloseCodeBlock)));

            CODE_BLOCK.Put();
            padding.Put();
        }

        #region Internal Helper Functions
        private static void PutDependentSpace(this AsMarkdown style)
        {
            // These styles require a space between them and their content
            var valueInRange = (int)style; // Use range approach via casting
            if (valueInRange >= (int)AsMarkdown.H1 && valueInRange <= (int)AsMarkdown.OrderedListItem)
                Padding.Space.Put();
        }
        private static string? GetAsString(this AsMarkdown style)
        {
            return style switch
            {
                AsMarkdown.Italic => ITALIC,
                AsMarkdown.Bold => BOLD,
                AsMarkdown.BoldItalic => BOLD_ITALIC,
                AsMarkdown.Code => CODE,
                AsMarkdown.UnorderedListItem => UNORDERED_LIST_ITEM,
                AsMarkdown.CodeBlock => CODE_BLOCK,
                AsMarkdown.H1 => H1,
                AsMarkdown.H2 => H2,
                AsMarkdown.H3 => H3,
                AsMarkdown.H4 => H4,
                AsMarkdown.H5 => H5,
                AsMarkdown.HorizonalLine => HORIZONTAL_LINE,
                AsMarkdown.None => null,
                _ => throw new InvalidEnumArgumentException()
            };
        }
        #endregion
    }
}
