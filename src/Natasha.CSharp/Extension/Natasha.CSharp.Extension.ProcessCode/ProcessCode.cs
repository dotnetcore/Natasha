using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using System;

namespace Natasha.CSharp.Extension.ProcessCode
{
    public static class ProcessCode
    {
        internal static Func<CSharpCompilation, CSharpCompilation> Creator(string argument)
        {
            return (compilation) =>
            {

                

                return compilation.ReplaceSyntaxTree();

            };
        }
    }
}
