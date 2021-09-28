using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Natasha.CSharp.SemanticAnalaysis.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Natasha.CSharp.Engine.SemanticAnalaysis
{

    public static class CS0246Analaysistor
    {

        private static readonly ConcurrentDictionary<string, Regex> _formartRegCache;
        static CS0246Analaysistor()
        {
            _formartRegCache = new ConcurrentDictionary<string, Regex>();
        }




        private static List<Regex> GetRegexList(Diagnostic diagnostic)
        {

            string formart = diagnostic.Descriptor.MessageFormat.ToString();
            string text = diagnostic.GetMessage();


            if (!_formartRegCache.TryGetValue(formart, out Regex regex))
            {

                var deal = RegexHelper.GetRealRegexString(formart).Replace("\\{0\\}", "(?<result0>.*)");
                regex = new Regex(deal, RegexOptions.Singleline | RegexOptions.Compiled);
                _formartRegCache[formart] = regex;

            }


            var matches = regex.Matches(text);
            List<Regex> _usingRegCache = new List<Regex>();
            for (int i = 0; i < matches.Count; i++)
            {

                var tempReg = $"using (?<result0>{matches[i].Groups["result0"].Value}.*?);";
                _usingRegCache.Add(new Regex(tempReg, RegexOptions.Singleline | RegexOptions.Compiled));

            }
            return _usingRegCache;

        }




        private static HashSet<string> GetUnableUsing(Diagnostic diagnostic, string code)
        {
            var usingHandlers = GetRegexList(diagnostic);
            HashSet<string> sets = new HashSet<string>();
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


        public static IEnumerable<UsingDirectiveSyntax> Handler(Diagnostic diagnostic)
        {

            var root = diagnostic.Location.SourceTree.GetRoot();
            var sets = GetUnableUsing(diagnostic, root.ToFullString());
            DefaultUsing.Remove(sets);
            return from usingDeclaration in root.DescendantNodes()
                          .OfType<UsingDirectiveSyntax>()
                        where sets.Contains(usingDeclaration.Name.ToFullString())
                        select usingDeclaration;

        }

    }

}
