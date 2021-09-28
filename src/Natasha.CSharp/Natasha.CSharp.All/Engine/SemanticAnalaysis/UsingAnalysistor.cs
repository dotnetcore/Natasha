using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Natasha.CSharp.Engine.SemanticAnalaysis
{
    public static class UsingAnalysistor
    {

        public static Func<CSharpCompilation, CSharpCompilation> Creator()
        {

            return (compilation) =>
            {
                var trees = compilation.SyntaxTrees;
                foreach (var tree in trees)
                {
                    var semantiModel = compilation.GetSemanticModel(tree);
                    var errors = semantiModel.GetDiagnostics();
                    CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
                    var editor = new SyntaxEditor(root, new AdhocWorkspace());
                    var removeCache = new HashSet<UsingDirectiveSyntax>();
                    foreach (var item in errors)
                    {

                        var result = UsingAnalysistorManagement.Handle(item);
                        if (result!=null)
                        {
                            removeCache.UnionWith(result);
                        }

                    }
                    foreach (var item in removeCache)
                    {
                        editor.RemoveNode(item);
                    }
                    compilation = compilation.ReplaceSyntaxTree(tree, editor.GetChangedRoot().SyntaxTree);
                }
                return compilation;
            };
        }
    }
}
