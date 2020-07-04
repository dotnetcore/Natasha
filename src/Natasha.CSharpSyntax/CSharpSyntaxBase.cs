using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Options;
using Natasha.Framework;

namespace Natasha.CSharpSyntax
{
    public abstract class CSharpSyntaxBase : SyntaxBase
    {
        
        //private readonly static AdhocWorkspace _workSpace;
        private readonly static CSharpParseOptions _options;

        static CSharpSyntaxBase()
        {

            //_workSpace = new AdhocWorkspace();
            //_workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));
            _options = new CSharpParseOptions(LanguageVersion.Latest, preprocessorSymbols: new[] { "RELEASE" });

        }




        public override SyntaxTree LoadTreeFromScript(string script)
        {
            
            var tree = SyntaxFactory.ParseSyntaxTree(script.Trim(), _options); ;
            return LoadTree(tree);

        }
        public override SyntaxTree LoadTree(SyntaxTree tree)
        {

            using (var workspace = new AdhocWorkspace())
            {
                SyntaxNode root = Formatter.Format(tree.GetRoot(), workspace);
                tree = root.SyntaxTree;
            }
            return tree;

        }

    }

}
