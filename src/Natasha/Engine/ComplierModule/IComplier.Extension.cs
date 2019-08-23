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

        public static void Deconstruct(
            this string text,
            out SyntaxTree tree, 
            out string[] typeNames, 
            out string formatter, 
            out IEnumerable<Diagnostic> errors)
        {

            tree = CSharpSyntaxTree.ParseText(text, new CSharpParseOptions(LanguageVersion.Latest));
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();


            root = (CompilationUnitSyntax)Formatter.Format(root, IComplier._workSpace);
            formatter = root.ToString();
            errors = root.GetDiagnostics();


            var result = new List<string>(from typeNodes
                         in root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                                          select typeNodes.Identifier.Text);

            result.AddRange(from typeNodes
                    in root.DescendantNodes().OfType<StructDeclarationSyntax>()
                            select typeNodes.Identifier.Text);

            result.AddRange(from typeNodes
                    in root.DescendantNodes().OfType<InterfaceDeclarationSyntax>()
                            select typeNodes.Identifier.Text);

            result.AddRange(from typeNodes
                    in root.DescendantNodes().OfType<EnumDeclarationSyntax>()
                            select typeNodes.Identifier.Text);


            typeNames = result.ToArray();

        }


    }

}
