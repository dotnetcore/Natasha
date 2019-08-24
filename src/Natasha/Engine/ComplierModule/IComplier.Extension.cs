using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System.Collections.Generic;
using System.Linq;

namespace Natasha.Complier
{

    public static class IComplierExtension
    {

        private readonly static AdhocWorkspace _workSpace;
        static IComplierExtension()
        {

            _workSpace = new AdhocWorkspace();
            _workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));

        }


        public static void Deconstruct(
            this string text,
            out SyntaxTree tree, 
            out string formatter, 
            out IEnumerable<Diagnostic> errors)
        {

            text = text.Trim();
            tree = CSharpSyntaxTree.ParseText(text, new CSharpParseOptions(LanguageVersion.Latest));
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();


            root = (CompilationUnitSyntax)Formatter.Format(root, _workSpace);
            tree = root.SyntaxTree;
            formatter = root.ToString();
            errors = root.GetDiagnostics();

        }


    }

}
