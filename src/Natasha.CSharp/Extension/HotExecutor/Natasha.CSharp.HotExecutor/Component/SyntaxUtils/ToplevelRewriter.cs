using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal static class ToplevelRewriter
    {
        public static CompilationUnitSyntax Handle(CompilationUnitSyntax root)
        {
            var file = root.SyntaxTree.FilePath;
            var firstMember = root.Members[0];
            if (firstMember != null && firstMember.IsKind(SyntaxKind.GlobalStatement))
            {
#if DEBUG
                HEProxy.ShowMessage("检测到顶级语句！");
#endif
                var usings = root.Usings;
                root = root.RemoveNodes(usings, SyntaxRemoveOptions.KeepExteriorTrivia)!;
                var content = "public class Program{ async static Task Main(string[] args){" + root!.ToFullString() + "}}";
                var tree = NatashaCSharpSyntax.ParseTree(content, file,(CSharpParseOptions)(root.SyntaxTree).Options);
                root = tree.GetCompilationUnitRoot();
                root = root.AddUsings([.. usings]);
            }
            return root;
        }
    }
}
