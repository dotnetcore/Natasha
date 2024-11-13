using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using Natasha.CSharp.Compiler.Component.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 程序集编译构建器 - UsingCode
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{
    public readonly HashSet<string> ExceptUsings = [];
    public AssemblyCSharpBuilder AppendExceptUsings(params string[] usings)
    {
        if (usings!=null && usings.Length >0)
        {
            ExceptUsings.UnionWith(usings);
        }
        return this;
    }

}



