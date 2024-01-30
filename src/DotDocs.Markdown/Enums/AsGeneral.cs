using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using State = DotDocs.Markdown.RenderState;

namespace DotDocs.Markdown.Enums
{
    public enum AsGeneral
    {
        Comma,
        OpeningParenthese,
        ClosingParenthese,
        SmallerThanArrow,
        LargerThanArrow,
        OpeningCurly,
        ClosingCurly
    }

    public static class AsGeneralEx
    {
        const string COMMA = ", ";
        const string OPENING_PARENTHESE = "(";
        const string CLOSING_PARENTHESE = ")";
        const string SMALLER_THAN_ARROW = "<";
        const string LARGER_THAN_ARROW = ">";
        const string OPENING_CURLY = "{";
        const string CLOSING_CURLY = "}";

        public static void Put(this AsGeneral style, Padding padding = Padding.None)
            => GetAsString(style).Put(padding);

        public static void Prefix(this AsGeneral style, string str, Padding padding = Padding.None)
        {
            style.Put();
            str.Put(padding);
        }

        #region Internal Helper Functions
        private static string GetAsString(this AsGeneral style)
        {
            return style switch
            {
                AsGeneral.Comma => COMMA,
                AsGeneral.OpeningParenthese => OPENING_PARENTHESE,
                AsGeneral.ClosingParenthese => CLOSING_PARENTHESE,
                AsGeneral.SmallerThanArrow => SMALLER_THAN_ARROW,
                AsGeneral.LargerThanArrow => LARGER_THAN_ARROW,
                AsGeneral.OpeningCurly => OPENING_CURLY,
                AsGeneral.ClosingCurly => CLOSING_CURLY,
                _ => throw new InvalidEnumArgumentException()
            };
        }
        #endregion
    }
}
