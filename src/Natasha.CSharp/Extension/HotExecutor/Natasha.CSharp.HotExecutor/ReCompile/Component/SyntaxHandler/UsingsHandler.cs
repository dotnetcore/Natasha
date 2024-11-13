using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using System.Collections.Concurrent;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component
{
    internal static class UsingsHandler
    {
        private static readonly NatashaUsingCache _usingCache;
        private static UsingDirectiveSyntax[] _defaultUsingNodes = [];
        static UsingsHandler()
        {
            _usingCache = new();
            
        }
        public static void RemoveUsings(IEnumerable<string> usings)
        {
            _usingCache!.Remove(usings);
        }
        public static UsingDirectiveSyntax[] FirstRunAndFillUsingCache()
        {
            _usingCache.Using(NatashaLoadContext.DefaultContext.UsingRecorder._usings);
            return _usingCache!.ToUsingNodes().ToArray();
        }

        public static void InitDefaultUsings(UsingDirectiveSyntax[] nodes)
        {
            _defaultUsingNodes = nodes;
        }
        public static CompilationUnitSyntax Handle(CompilationUnitSyntax root, ConcurrentDictionary<string, HashSet<string>> cs0104Cache)
        {
            if (_defaultUsingNodes.Length == 0)
            {
                var filePath = root.SyntaxTree.FilePath;
                if (cs0104Cache.TryGetValue(filePath, out var sets) && sets.Count > 0)
                {
                    var usingList = _defaultUsingNodes
                        .Where(node => !sets.Contains(node.Name!.ToString()))
                        .ToList();

#if DEBUG
                    foreach (var node in _defaultUsingNodes)
                    {
                        var name = node.Name!.ToString();
                        if (sets.Contains(name))
                        {
                            HEProxy.ShowMessage($"Excluded {name}");
                        }
                    }
#endif
                    return root.AddUsings(usingList.ToArray());
                }
                return root.AddUsings(_defaultUsingNodes);
            }
            return root;
        }

        public static void OnceInitDefaultUsing(ConcurrentDictionary<string, SyntaxTree> cache, CSharpParseOptions options)
        {
            if (_defaultUsingNodes.Length == 0)
	        {
                var namespaces = cache.Values
                    .SelectMany(tree => tree.GetCompilationUnitRoot()
                    .DescendantNodes()
                    .OfType<NamespaceDeclarationSyntax>()
                    .Select(ns => ns.Name.ToString()))
                    .ToList();

                RemoveUsings(namespaces);
                FirstRunAndFillUsingCache();

                foreach (var item in cache)
                {
                    var root = item.Value.GetCompilationUnitRoot();
                    cache[item.Key] = CSharpSyntaxTree.Create(root, options, item.Key, Encoding.UTF8);
                }
            }
        }
    }
}
