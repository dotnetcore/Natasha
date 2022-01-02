using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler;
using Natasha.CSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 程序集编译构建器 - 语义
/// </summary>
public partial class AssemblyCSharpBuilder 
{
    ///// <summary>
    ///// 被禁断的错误代码
    ///// </summary>
    //private static Func<CSharpCompilation, CSharpCompilation>? _globalSemanticHandler;


    //public static void AddSemanticAnalysistor(Func<CSharpCompilation, CSharpCompilation> globalSemanticHandler)
    //{
    //    _globalSemanticHandler = globalSemanticHandler;
    //}

    //语义过滤器
    private readonly List<Func<AssemblyCSharpBuilder, AssemblyCSharpBuilder>> _semanticAnalysistor;
    //域
    //添加语法方法
    //CSharp编译器
    //程序集编译方法


    public AssemblyCSharpBuilder AddSemanticAnalysistor(Func<AssemblyCSharpBuilder, AssemblyCSharpBuilder> func)
    {
        _semanticAnalysistor.Add(func);
        return this;
    }

}



