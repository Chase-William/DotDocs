using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docshark.Test.Data.Structs
{
    public struct Rectangle
    {
        public int Top { get; set; }
        public int Left { get; set; }

        public int bottom;
        public int right;

        public int GetWidth() => right - Left;
        public int GetHeight() => bottom - Top;

        public struct Builder
        {
            public static Rectangle Default() => new Rectangle()
        }
    }
}
