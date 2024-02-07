using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown.Enums
{
    public enum Padding
    {
        None,
        NewLine,
        DoubleNewLine,
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
