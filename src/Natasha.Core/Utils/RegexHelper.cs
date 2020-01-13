using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Core
{
    internal static class RegexHelper
    {
        public static string GetRealRegexString(string source)
        {
            return source
                .ReplaceRegChar("\\")
                .ReplaceRegChar("?")
                .ReplaceRegChar("{")
                .ReplaceRegChar("}")
                .ReplaceRegChar("[")
                .ReplaceRegChar("]")
                .ReplaceRegChar("(")
                .ReplaceRegChar(")")
                .ReplaceRegChar("+");
        }

    }
    internal static class StringExtentsion
    {
        internal static string ReplaceRegChar(this string source, string @char)
        {
            return source.Replace(@char, "\\" + @char);
        }
    }
}
