using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Builder;
using Natasha.CSharp.Using;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Natasha.CSharp.Extension.Ambiguity
{
    internal static class AmbiguityUsings
    {
        //internal static readonly ConcurrentDictionary<CSharpCompilation, HashSet<string>> _usingsCache;
        //static AmbiguityUsings()
        //{
        //    _usingsCache = new ConcurrentDictionary<CSharpCompilation, HashSet<string>>();
        //}

        private static CSharpCompilation HandlerCS0104(SyntaxTree syntaxTree, CSharpCompilation compilation,  NatashaUsingCache usings, bool ignoreAccessibility)
        {
            var semantiModel = compilation.GetSemanticModel(syntaxTree, ignoreAccessibility);
            var errors = semantiModel.GetDiagnostics();
            if (errors.Length > 0)
            {
                CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();
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
                            if (!usings.HasUsing(spaceName))
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

                if (removeCache.Count > 0)
                {
                    compilation = compilation.ReplaceSyntaxTree(syntaxTree, root.RemoveNodes(removeCache, SyntaxRemoveOptions.KeepNoTrivia)!.SyntaxTree);
                }
            }
            return compilation;
        }
           

        internal static Func<AssemblyCSharpBuilder, CSharpCompilation, bool, CSharpCompilation> OopBuilderCreator<T>(OopBuilder<T> oopBuilder) where T : OopBuilder<T>, new()
        {
            return (builder, compilation, ignoreAccessibility) =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {
                    compilation = HandlerCS0104(tree, compilation, oopBuilder.UsingRecorder, ignoreAccessibility);
                }
                return compilation;

            };
        }





        internal static Func<AssemblyCSharpBuilder, CSharpCompilation, bool, CSharpCompilation> MethodBuilderCreator<T>(MethodBuilder<T> methodBuilder) where T : MethodBuilder<T>, new()
        {
            return (builder,compilation, ignoreAccessibility) =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {

                    compilation = HandlerCS0104(tree, compilation, methodBuilder.OopHandler.UsingRecorder, ignoreAccessibility);

                }
                return compilation;

            };
        }
        internal static Func<AssemblyCSharpBuilder, CSharpCompilation, bool,  CSharpCompilation> NDelegateCreator(NDelegate nDelegate)
        {
            return (builder, compilation, ignoreAccessibility) =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {
                    compilation = HandlerCS0104(tree, compilation, nDelegate.MethodHandler.OopHandler.UsingRecorder, ignoreAccessibility);
                }
                return compilation;

            };
        }
        //internal static Func<CSharpCompilation, CSharpCompilation> UsingsCreator()
        //{
        //    return compilation =>
        //    {

        //        var trees = compilation.SyntaxTrees;
        //        foreach (var tree in trees)
        //        {
        //            compilation = HandlerCS0104(tree, compilation, );

        //        }
        //        _usingsCache!.Remove(compilation);
        //        return compilation;

        //    };
        //}
    }
}
