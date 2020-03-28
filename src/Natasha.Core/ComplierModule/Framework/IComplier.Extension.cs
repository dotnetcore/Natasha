using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using System.Collections.Generic;

namespace Natasha.Core.Compiler
{

    public static class ICompilerExtension
    {

        private readonly static AdhocWorkspace _workSpace;
        private readonly static CSharpParseOptions _options;
        
        static ICompilerExtension()
        {

            _workSpace = new AdhocWorkspace();
            _workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));
            _options = new CSharpParseOptions(LanguageVersion.Latest);

        }


        public static void Deconstruct(
            this string text,
            out SyntaxTree tree, 
            out string formatter, 
            out IEnumerable<Diagnostic> errors)
        {

            tree = CSharpSyntaxTree.ParseText(text.Trim(), _options);
            SyntaxNode root = Formatter.Format(tree.GetCompilationUnitRoot(), _workSpace);
            _workSpace.ClearSolution();
            tree = root.SyntaxTree;
            formatter = tree.ToString();
            errors = tree.GetDiagnostics();

        }



        public static void Deconstruct(
            this SyntaxTree node,
            out SyntaxTree tree,
            out string formatter,
            out IEnumerable<Diagnostic> errors)
        {

            SyntaxNode root = Formatter.Format(node.GetCompilationUnitRoot(), _workSpace);
            _workSpace.ClearSolution();
            tree = root.SyntaxTree;
            formatter = tree.ToString();
            errors = tree.GetDiagnostics();

        }

    }

}
