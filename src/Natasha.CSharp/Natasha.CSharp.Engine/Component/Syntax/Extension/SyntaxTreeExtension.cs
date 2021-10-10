using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.Collections.Generic;
using System.Text;


public static class SyntaxTreeExtension
{
    public static SyntaxTree ConvertToSyntaxTree(this string code)
    {

        var tree = SyntaxFactory.ParseSyntaxTree(code.Trim(), NatashaCSharpSyntax._options);
        return tree.GetRoot().NormalizeWhitespace().SyntaxTree;

    }
}

