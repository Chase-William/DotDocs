using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown.Enums
{
    public enum Language
    {
        CSharp
    }

    public static class LanguageEx
    {
        public const string C_SHARP = "cs";

        public static void Put(this Language language)
        {
            switch (language)
            {
                case Language.CSharp:
                    State.Builder.Append(C_SHARP);
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            };
        }        
    }
}
