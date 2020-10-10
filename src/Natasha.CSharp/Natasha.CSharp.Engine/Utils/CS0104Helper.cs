using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Natasha.Engine.Utils
{

    internal static class CS0104Helper
    {

        private static readonly ConcurrentDictionary<string, Regex> RegCache;
        static CS0104Helper()
        {
            RegCache = new ConcurrentDictionary<string, Regex>();
        }


        public static (string str1,string str2) Handler(Diagnostic diagnostic)
        {

            string formart = diagnostic.Descriptor.MessageFormat.ToString();
            string text = diagnostic.GetMessage();
            if (!RegCache.ContainsKey(formart))
            {
                var deal = RegexHelper.GetRealRegexString(formart).Replace("\\{0\\}",".*").Replace("\\{1\\}", "(?<result1>.*)").Replace("\\{2\\}", "(?<result2>.*)");
                Regex regex = new Regex(deal, RegexOptions.Singleline | RegexOptions.Compiled);
                RegCache[formart] = regex;
            }
            var match = RegCache[formart].Match(text);
            var str1 = match.Groups["result1"].Value;
            var str2 = match.Groups["result2"].Value;


            var str0 = str1.Split('.');
            if (str0.Length==1)
            {

                str1 = "System";

            }
            else
            {

                str1 = str1.Substring(0, str1.Length - 1 - str0[str0.Length - 1].Length);

            }

            str0 = str2.Split('.');
            if (str0.Length == 1)
            {

                str2 = "System";

            }
            else
            {

                str2 = str2.Substring(0, str2.Length - 1 - str0[str0.Length - 1].Length);

            }
            return (str1, str2);

        }
    }
}
