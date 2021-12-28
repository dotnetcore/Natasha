using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Natasha.CSharp.Builder;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Natasha.CSharp.Extension.Ambiguity
{
    internal static class AmbiguityUsings
    {
        internal static readonly ConcurrentDictionary<CSharpCompilation, HashSet<string>> _usingsCache;
        static AmbiguityUsings()
        {
            _usingsCache = new ConcurrentDictionary<CSharpCompilation, HashSet<string>>();
        }



        public static CSharpCompilation HandlerCS0104(SyntaxTree syntaxTree, CSharpCompilation compilation,  HashSet<string> usings)
        {
            var semantiModel = compilation.GetSemanticModel(syntaxTree);
            var errors = semantiModel.GetDiagnostics();
            if (errors.Length > 0)
            {
                CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();
                var editor = new SyntaxEditor(root, new AdhocWorkspace());
                var removeCache = new HashSet<UsingDirectiveSyntax>();

                foreach (var diagnostic in errors)
                {

                    if (diagnostic.Id == "CS0104")
                    {
                        var info = semantiModel.GetSymbolInfo(diagnostic.GetSyntaxNode(root));
                        bool stepLastest = false;
                        for (int i = 0; i < info.CandidateSymbols.Length - 1; i++)
                        {
                            var spaceName = info.CandidateSymbols[i]!.OriginalDefinition.ContainingNamespace.Name;
                            if (!usings.Contains(spaceName))
                            {

                                var node = (from usingDeclaration in root.Usings
                                            where usingDeclaration.Name.ToString() == spaceName
                                            select usingDeclaration).FirstOrDefault();
                                if (node != null)
                                {
                                    removeCache.Add(node);
                                }
                            }
                            else
                            {
                                stepLastest = true;
                            }
                        }
                        if (stepLastest)
                        {
                            var spaceName = info.CandidateSymbols[info.CandidateSymbols.Length - 1]!.OriginalDefinition.ContainingNamespace.ToString();

                            var node = (from usingDeclaration in root.Usings
                                        where usingDeclaration.Name.ToString() == spaceName
                                        select usingDeclaration).FirstOrDefault();
                            if (node != null)
                            {
                                removeCache.Add(node);
                            }
                        }
                    }

                }

                foreach (var item in removeCache)
                {
                    editor.RemoveNode(item);
                }

                return compilation.ReplaceSyntaxTree(syntaxTree, editor.GetChangedRoot().SyntaxTree);
            }
            return compilation;
        }
           

        internal static Func<CSharpCompilation, CSharpCompilation> OopBuilderCreator<T>(OopBuilder<T> oopBuilder) where T : OopBuilder<T>, new()
        {
            return compilation =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {
                    compilation = HandlerCS0104(tree, compilation, oopBuilder.Usings);
                }
                return compilation;

            };
        }





        internal static Func<CSharpCompilation, CSharpCompilation> MethodBuilderCreator<T>(MethodBuilder<T> methodBuilder) where T : MethodBuilder<T>, new()
        {
            return compilation =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {

                    compilation = HandlerCS0104(tree, compilation, methodBuilder.OopHandler.Usings);

                }
                return compilation;

            };
        }
        internal static Func<CSharpCompilation, CSharpCompilation> NDelegateCreator(NDelegate nDelegate)
        {
            return compilation =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {
                    var usings = nDelegate.Usings;
                    OopBuilder oopBuilder = new OopBuilder();
                    oopBuilder.Using(usings);
                    compilation = HandlerCS0104(tree, compilation, oopBuilder.Usings);
                }
                return compilation;

            };
        }
        internal static Func<CSharpCompilation, CSharpCompilation> UsingsCreator()
        {
            return compilation =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {
                    compilation = HandlerCS0104(tree, compilation, _usingsCache[compilation]);

                }
                _usingsCache!.Remove(compilation);
                return compilation;

            };
        }
    }
}
