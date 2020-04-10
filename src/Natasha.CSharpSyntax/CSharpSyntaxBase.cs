using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using Natasha.Error.Model;
using Natasha.Framework;

namespace Natasha.CSharpSyntax
{
    public abstract class CSharpSyntaxBase : SyntaxBase
    {
        
        private readonly static AdhocWorkspace _workSpace;
        private readonly static CSharpParseOptions _options;

        static CSharpSyntaxBase()
        {

            _workSpace = new AdhocWorkspace();
            _workSpace.AddSolution(SolutionInfo.Create(SolutionId.CreateNewId("formatter"), VersionStamp.Default));
            _options = new CSharpParseOptions(LanguageVersion.Latest);

        }




        public override SyntaxTree LoadTreeFromScript(string script)
        {
            var tree = CSharpSyntaxTree.ParseText(script.Trim(), _options);
            return LoadTree(tree);
        }
        public override SyntaxTree LoadTree(SyntaxTree tree)
        {
            SyntaxNode root = Formatter.Format(tree.GetCompilationUnitRoot(), _workSpace);
            tree = root.SyntaxTree;
            _workSpace.ClearSolution();
            return tree;
        }

    }

}
