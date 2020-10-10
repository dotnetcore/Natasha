using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Natasha.Engine.Utils
{

    internal class CS0246Helper
    {

        private static readonly ConcurrentDictionary<string, Regex> RegCache;
        private static readonly ConcurrentDictionary<string, List<Regex>> UsingCache;
        static CS0246Helper()
        {
            RegCache = new ConcurrentDictionary<string, Regex>();
            UsingCache = new ConcurrentDictionary<string, List<Regex>>();
        }




        public static List<Regex> Handler(Diagnostic diagnostic)
        {

            string formart = diagnostic.Descriptor.MessageFormat.ToString();
            string text = diagnostic.GetMessage();

            if (!RegCache.ContainsKey(formart))
            {

                var deal = RegexHelper.GetRealRegexString(formart).Replace("\\{0\\}", "(?<result0>.*)");
                Regex regex = new Regex(deal, RegexOptions.Singleline | RegexOptions.Compiled);
                RegCache[formart] = regex;
                UsingCache[formart] = new List<Regex>();

            }


            var matches = RegCache[formart].Matches(text);
            for (int i = 0; i < matches.Count; i++)
            {

                var tempReg = $"using (?<result0>{matches[i].Groups["result0"].Value}.*?);";
                UsingCache[formart].Add(new Regex(tempReg, RegexOptions.Singleline | RegexOptions.Compiled));

            }
            return UsingCache[formart];

        }




        public static IEnumerable<string> GetUsings(Diagnostic diagnostic, string code)
        {

            string formart = diagnostic.Descriptor.MessageFormat.ToString();
            HashSet<string> sets = new HashSet<string>();
            var usingHandlers = UsingCache[formart].ToArray();
            foreach (var item in usingHandlers)
            {

                var matches = item.Matches(code);
                for (int i = 0; i < matches.Count; i += 1)
                {

                    sets.Add(matches[i].Groups["result0"].Value);

                }

            }
            return sets;

        }

    }

}
