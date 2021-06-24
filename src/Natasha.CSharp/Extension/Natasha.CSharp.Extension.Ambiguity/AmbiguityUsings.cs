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
        internal static Func<CSharpCompilation, CSharpCompilation> OopBuilderCreator<T>(OopBuilder<T> oopBuilder) where T : OopBuilder<T>, new()
        {
            return compilation =>
            {

                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {
                    var semantiModel = compilation.GetSemanticModel(tree);
                    var errors = semantiModel.GetDiagnostics();
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
                    var editor = new SyntaxEditor(root, new AdhocWorkspace());
                    var removeCache = new HashSet<UsingDirectiveSyntax>();
                    var usings = oopBuilder.Usings;

                    foreach (var diagnostic in errors)
                    {

                        if (diagnostic.Id == "CS0104")
                        {
                            (string str1, string str2) = CS0104Analaysistor.GetUnableUsing(diagnostic);
                            var needToRemove = str1;

                            if (usings.Contains(str1))
                            {
                                if (usings.Contains(str2))
                                {
                                    if (str2 == "System")
                                    {
                                        needToRemove = str2;
                                    }
                                    else
                                    {
                                        needToRemove = str1;
                                    }
                                }
                                else
                                {
                                    needToRemove = str2;
                                }
                            }
                            else
                            {
                                needToRemove = str1;
                            }


                            removeCache.UnionWith(from usingDeclaration in diagnostic.Location.SourceTree.GetRoot()
                                        .DescendantNodes()
                                        .OfType<UsingDirectiveSyntax>()
                                                  where usingDeclaration.Name.ToFullString() == needToRemove
                                                  select usingDeclaration);
                        }

                    }
                    foreach (var item in removeCache)
                    {
                        editor.RemoveNode(item);
                    }
                    compilation = compilation.ReplaceSyntaxTree(tree, editor.GetChangedRoot().SyntaxTree);
                }
                _usingsCache.Remove(compilation);
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
                    var semantiModel = compilation.GetSemanticModel(tree);
                    var errors = semantiModel.GetDiagnostics();
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
                    var editor = new SyntaxEditor(root, new AdhocWorkspace());
                    var removeCache = new HashSet<UsingDirectiveSyntax>();
                    var usings = methodBuilder.OopHandler.Usings;

                    foreach (var diagnostic in errors)
                    {

                        if (diagnostic.Id == "CS0104")
                        {
                            (string str1, string str2) = CS0104Analaysistor.GetUnableUsing(diagnostic);
                            var needToRemove = str1;

                            if (usings.Contains(str1))
                            {
                                if (usings.Contains(str2))
                                {
                                    if (str2 == "System")
                                    {
                                        needToRemove = str2;
                                    }
                                    else
                                    {
                                        needToRemove = str1;
                                    }
                                }
                                else
                                {
                                    needToRemove = str2;
                                }
                            }
                            else
                            {
                                needToRemove = str1;
                            }


                            removeCache.UnionWith(from usingDeclaration in diagnostic.Location.SourceTree.GetRoot()
                                        .DescendantNodes()
                                        .OfType<UsingDirectiveSyntax>()
                                                  where usingDeclaration.Name.ToFullString() == needToRemove
                                                  select usingDeclaration);
                        }

                    }
                    foreach (var item in removeCache)
                    {
                        editor.RemoveNode(item);
                    }
                    compilation = compilation.ReplaceSyntaxTree(tree, editor.GetChangedRoot().SyntaxTree);
                }
                _usingsCache.Remove(compilation);
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
                    var semantiModel = compilation.GetSemanticModel(tree);
                    var errors = semantiModel.GetDiagnostics();
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
                    var editor = new SyntaxEditor(root, new AdhocWorkspace());
                    var removeCache = new HashSet<UsingDirectiveSyntax>();
                    var usings = nDelegate.Usings;
                    OopBuilder oopBuilder = new OopBuilder();
                    oopBuilder.Using(usings);
                    var usingSets = oopBuilder.Usings;

                    foreach (var diagnostic in errors)
                    {

                        if (diagnostic.Id == "CS0104")
                        {
                            (string str1, string str2) = CS0104Analaysistor.GetUnableUsing(diagnostic);
                            var needToRemove = str1;

                            if (usingSets.Contains(str1))
                            {
                                if (usingSets.Contains(str2))
                                {
                                    if (str2 == "System")
                                    {
                                        needToRemove = str2;
                                    }
                                    else
                                    {
                                        needToRemove = str1;
                                    }
                                }
                                else
                                {
                                    needToRemove = str2;
                                }
                            }
                            else
                            {
                                needToRemove = str1;
                            }


                            removeCache.UnionWith(from usingDeclaration in diagnostic.Location.SourceTree.GetRoot()
                                        .DescendantNodes()
                                        .OfType<UsingDirectiveSyntax>()
                                                  where usingDeclaration.Name.ToFullString() == needToRemove
                                                  select usingDeclaration);
                        }

                    }
                    foreach (var item in removeCache)
                    {
                        editor.RemoveNode(item);
                    }
                    compilation = compilation.ReplaceSyntaxTree(tree, editor.GetChangedRoot().SyntaxTree);
                }
                _usingsCache.Remove(compilation);
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
                    var semantiModel = compilation.GetSemanticModel(tree);
                    var errors = semantiModel.GetDiagnostics();
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
                    var editor = new SyntaxEditor(root, new AdhocWorkspace());
                    var removeCache = new HashSet<UsingDirectiveSyntax>();
                    var usingSets = _usingsCache[compilation];
                    foreach (var diagnostic in errors)
                    {

                        if (diagnostic.Id=="CS0104")
                        {
                            (string str1, string str2) = CS0104Analaysistor.GetUnableUsing(diagnostic);
                            var needToRemove = str1;

                            if (usingSets.Contains(str1))
                            {
                                if (usingSets.Contains(str2))
                                {
                                    if (str2 == "System")
                                    {
                                        needToRemove = str2;
                                    }
                                    else
                                    {
                                        needToRemove = str1;
                                    }
                                }
                                else
                                {
                                    needToRemove = str2;
                                }
                            }
                            else
                            {
                                needToRemove = str1;
                            }


                            removeCache.UnionWith(from usingDeclaration in diagnostic.Location.SourceTree.GetRoot()
                                        .DescendantNodes()
                                        .OfType<UsingDirectiveSyntax>()
                                   where usingDeclaration.Name.ToFullString() == needToRemove
                                   select usingDeclaration);
                        }

                    }
                    foreach (var item in removeCache)
                    {
                        editor.RemoveNode(item);
                    }
                    compilation = compilation.ReplaceSyntaxTree(tree, editor.GetChangedRoot().SyntaxTree);
                }
                _usingsCache.Remove(compilation);
                return compilation;

            };
        }
    }
}
