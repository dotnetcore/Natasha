using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha;
using Natasha.CSharpEngine;
using Natasha.CSharpEngine.Error;
using Natasha.CSharpEngine.Log;
using Natasha.Error;
using Natasha.Error.Model;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;


/// <summary>
/// 程序集编译构建器
/// </summary>
public class AssemblyCSharpBuilder 
{

    //语义过滤器
    private readonly List<Func<AssemblyCSharpBuilder, AssemblyCSharpBuilder>> _semanticAnalysistor;
    //域
    //添加语法方法
    //CSharp编译器
    //程序集编译方法



    public AssemblyCSharpBuilder()
    {
        _semanticAnalysistor = new();
    }


    public AssemblyCSharpBuilder AddSemanticAnalysistor(Func<AssemblyCSharpBuilder, AssemblyCSharpBuilder> func)
    {
        _semanticAnalysistor.Add(func);
        return this;
    }
}



