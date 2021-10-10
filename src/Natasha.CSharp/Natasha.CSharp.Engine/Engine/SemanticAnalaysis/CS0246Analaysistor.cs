using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Natasha.CSharp.SemanticAnalaysis.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Natasha.CSharp.Engine.SemanticAnalaysis
{

    public static class CS0246Analaysistor
    {

        private static readonly ConcurrentDictionary<string, Regex> _formartRegCache;
        static CS0246Analaysistor()
        {
            _formartRegCache = new ConcurrentDictionary<string, Regex>();
        }




        private static Regex GetRegex(Diagnostic diagnostic)
        {

            string formart = diagnostic.Descriptor.MessageFormat.ToString();
            string text = diagnostic.GetMessage();


            if (!_formartRegCache.TryGetValue(formart, out Regex regex))
            {

                var deal = RegexHelper.GetRealRegexString(formart).Replace("\\{0\\}", "(?<result0>.*)");
                regex = new Regex(deal, RegexOptions.Singleline | RegexOptions.Compiled);
                _formartRegCache[formart] = regex;

            }


            var match = regex.Match(text);
            var tempReg = $"using (?<result0>{match.Groups["result0"].Value}.*?);";
            //_usingRegCache.Add();
            return new Regex(tempReg, RegexOptions.Singleline | RegexOptions.Compiled);

        }




        internal static HashSet<string> GetUnableUsing(IEnumerable<Diagnostic> diagnostics, string code)
        {

            HashSet<string> sets = new HashSet<string>();
            object obj = new object();
            Parallel.ForEach(diagnostics, diagnostic => {
                var usingHandler = GetRegex(diagnostic);
                var matches = usingHandler.Matches(code);
                lock (obj)
                {
                    for (int i = 0; i < matches.Count; i += 1)
                    {
                        sets.Add(matches[i].Groups["result0"].Value);
                    }
                }
            });
            return sets;

        }


        internal static IEnumerable<UsingDirectiveSyntax> Handler(CompilationUnitSyntax root,HashSet<string> noUseUsings)
        {

            DefaultUsing.Remove(noUseUsings);
            return from usingDeclaration in root.Usings
                   where noUseUsings.Contains(usingDeclaration.Name.ToFullString())
                   select usingDeclaration;

        }

    }

}
