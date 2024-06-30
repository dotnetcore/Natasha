using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using Natasha.CSharp.Extension.HotExecutor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal static class UsingsRewriter
    {
        private static readonly NatashaUsingCache _usingCache;
        private static UsingDirectiveSyntax[] _defaultUsingNodes = [];
        static UsingsRewriter()
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
            return _usingCache!.GetUsingNodes().ToArray();
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
                List<UsingDirectiveSyntax> usingList = [];
                if (cs0104Cache.TryGetValue(filePath, out var sets))
                {
                    if (sets.Count > 0)
                    {
                        foreach (var node in _defaultUsingNodes)
                        {
                            var name = node.Name!.ToString();
                            if (!sets.Contains(name))
                            {
                                usingList.Add(node);
                            }
#if DEBUG
                            else
                            {
                                HEProxy.ShowMessage($"排除 {name}");
                            }
#endif
                        }
                        return root.AddUsings([.. usingList]);
                    }
                }
                return root.AddUsings(_defaultUsingNodes);
            }
            return root;
        }

        public static void OnceInitDefaultUsing(ConcurrentDictionary<string, SyntaxTree> cache, CSharpParseOptions options)
        {
            if (_defaultUsingNodes.Length == 0)
	        {
                foreach (var tree in cache.Values)
                {
                    var namespaces = tree
                                        .GetCompilationUnitRoot()
                                        .DescendantNodes()
                                        .OfType<NamespaceDeclarationSyntax>()
                                        .Select(ns => ns.Name.ToString())
                                        .ToList();
                    RemoveUsings(namespaces);
                }
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
