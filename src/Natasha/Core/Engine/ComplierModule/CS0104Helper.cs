using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Natasha.Complier
{
    public static class CS0104Helper
    {
        private static readonly ConcurrentDictionary<string, Regex> RegCache;
        static CS0104Helper()
        {
            RegCache = new ConcurrentDictionary<string, Regex>();
        }


        public static (string str1,string str2) Handler(string formart, string text)
        {
            if (!RegCache.ContainsKey(formart))
            {
                var deal = formart.Replace("{0}", "(?<result0>.*)").Replace("{1}", "(?<result1>.*)").Replace("{2}", "(?<result2>.*)");
                Regex regex = new Regex(deal, RegexOptions.Singleline | RegexOptions.Compiled);
                RegCache[formart] = regex;
            }
            var match = RegCache[formart].Match(text);
            var str0 = match.Groups["result0"].Value;
            var str1 = match.Groups["result1"].Value;
            var str2 = match.Groups["result2"].Value;
            return (str1.Substring(0, str1.Length - 1 - str0.Length), str2.Substring(0, str2.Length - 1 - str0.Length));

        }
    }
}
