using System;

namespace FrameworkFunctionUT
{
    internal static class StringExtension
    {
        internal static string ToOSString(this string text)
        {
            if (text.IndexOf('\r') != -1 && Environment.NewLine[0] != '\r')
            {
                return text.Replace("\r\n", Environment.NewLine);
            }
            else if (text.IndexOf('\r') == -1 && Environment.NewLine[0] == '\r')
            {
                return text.Replace("\n", Environment.NewLine);
            }
            return text;
        }
    }
}
