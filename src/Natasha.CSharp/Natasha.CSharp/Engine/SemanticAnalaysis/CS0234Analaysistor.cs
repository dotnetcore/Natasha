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
    internal static class CS0234Analaysistor
    {
        private static readonly ConcurrentDictionary<string, Regex> RegCache;
        static CS0234Analaysistor()
        {

            RegCache = new ConcurrentDictionary<string, Regex>();

        }


        private static string GetUnableUsing(Diagnostic diagnostic)
        {

            string formart = diagnostic.Descriptor.MessageFormat.ToString();
            string text = diagnostic.GetMessage();
            if (!RegCache.ContainsKey(formart))
            {

                var deal = RegexHelper.GetRealRegexString(formart).Replace("\\{0\\}", "(?<result0>.*)").Replace("\\{1\\}", "(?<result1>.*)");
                Regex regex = new Regex(deal, RegexOptions.Singleline | RegexOptions.Compiled);
                RegCache[formart] = regex;

            }

            var match = RegCache[formart].Match(text);
            return $"{match.Groups["result1"].Value}.{match.Groups["result0"].Value}";

        }

        internal static IEnumerable<UsingDirectiveSyntax>? Handler(CompilationUnitSyntax root, Diagnostic diagnostic)
        {

            var needToRemove = GetUnableUsing(diagnostic);
            var nodes = from usingDeclaration in root.Usings
                        where usingDeclaration.Name.ToFullString().StartsWith(needToRemove)
                        select usingDeclaration;
            if (nodes!=null)
            {
                DefaultUsing.Remove(nodes.Select(item => item.Name.ToFullString()));
            }
            return nodes;

        }
    }
}
