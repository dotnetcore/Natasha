using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler;
using Natasha.CSharp.Compiler.SemanticAnalaysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

/// <summary>
/// 程序集编译构建器 - 语义
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{

    private readonly List<Func<AssemblyCSharpBuilder, CSharpCompilation, bool,  CSharpCompilation>> _semanticAnalysistor;
    private bool _semanticCheckIgnoreAccessibility;



    /// <summary>
    /// 在语义分析时检测 可访问性问题, 默认分析. 降低性能.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder AnalysisIgnoreAccessibility()
    {
        _semanticCheckIgnoreAccessibility = false;
        return this;
    }

    /// <summary>
    /// 不在语义分析时检测 可访问性问题, 可提升性能.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder NotAnalysisIgnoreAccessibility()
    {
        _semanticCheckIgnoreAccessibility = true;
        return this;
    }

    public AssemblyCSharpBuilder AddSemanticAnalysistor(Func<AssemblyCSharpBuilder, CSharpCompilation, bool, CSharpCompilation> func)
    {
        _semanticAnalysistor.Add(func);
        return this;
    }

    public AssemblyCSharpBuilder RemoveSemanticAnalysistor(Func<AssemblyCSharpBuilder, CSharpCompilation, bool, CSharpCompilation> func)
    {
        _semanticAnalysistor.Remove(func);
        return this;
    }

    public bool EnableSemanticHandler;

    public AssemblyCSharpBuilder ClearInnerSemanticAnalysistor()
    {
        _semanticAnalysistor.Remove(UsingAnalysistor._usingSemanticDelegate);
        return this;
    }


}



