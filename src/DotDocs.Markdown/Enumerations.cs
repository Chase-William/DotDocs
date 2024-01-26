using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotDocs.Markdown
{
    public enum HeaderVariant : byte
    {
        H1 = 1,
        H2,
        H3,
        H4,
        H5
    }

    public enum Padding
    {
        NewLine,
        DoubleNewLine,
        NoPadding
    }

    public enum ListStyle
    {
        Ordered,
        Unordered
    }
}
