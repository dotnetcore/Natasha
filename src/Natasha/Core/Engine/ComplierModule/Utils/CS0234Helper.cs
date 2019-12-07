using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Natasha.Complier
{

    internal class CS0234Helper
    {

        private static readonly ConcurrentDictionary<string, Regex> RegCache;
        static CS0234Helper()
        {
            RegCache = new ConcurrentDictionary<string, Regex>();
        }


        public static string Handler(string formart, string text)
        {

            if (!RegCache.ContainsKey(formart))
            {

                var deal = RegexHelper.GetRealRegexString(formart).Replace("\\{0\\}", "(?<result0>.*)").Replace("\\{1\\}", "(?<result1>.*)");
                Regex regex = new Regex(deal, RegexOptions.Singleline | RegexOptions.Compiled);
                RegCache[formart] = regex;
            }

            var match = RegCache[formart].Match(text);
            return match.Groups["result1"].Value + "."+ match.Groups["result0"].Value;

        }

    }

}
