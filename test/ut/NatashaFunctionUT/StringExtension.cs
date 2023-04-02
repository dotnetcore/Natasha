using System;

namespace NatashaFunctionUT
{
    internal static class StringExtension
    {
        internal static string ToOSString(this string text)
        {
            if (text.Contains('\r') && Environment.NewLine[0] != '\r')
            {
                return text.Replace("\r\n", Environment.NewLine);
            }
            else if (!text.Contains('\r') && Environment.NewLine[0] == '\r')
            {
                return text.Replace("\n", Environment.NewLine);
            }
            return text;
        }
    }
}
