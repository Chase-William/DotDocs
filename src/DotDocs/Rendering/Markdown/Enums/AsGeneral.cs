using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotDocs.Markdown.Extensions;
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
        ClosingCurly,
        SemiColon
    }

    public static class AsGeneralEx
    {
        const char COMMA = ',';
        const char OPENING_PARENTHESE = '(';
        const char CLOSING_PARENTHESE = ')';
        const char SMALLER_THAN_ARROW = '<';
        const char LARGER_THAN_ARROW = '>';
        const char OPENING_CURLY = '{';
        const char CLOSING_CURLY = '}';
        const char SEMI_COLON = ';';
        
        public static void Put(this AsGeneral style, Padding padding = Padding.None)
            => GetAsChar(style).Put(padding);

        public static void Prefix(this AsGeneral style, string str, bool includeSpaceBetween = false, Padding padding = Padding.None)
        {
            style.Put();
            if (includeSpaceBetween)
                Padding.Space.Put();
            str.Put(padding);
        }

        #region Internal Helper Functions
        private static char GetAsChar(this AsGeneral style)
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
                AsGeneral.SemiColon => SEMI_COLON,
                _ => throw new InvalidEnumArgumentException()
            };
        }
        #endregion
    }
}
