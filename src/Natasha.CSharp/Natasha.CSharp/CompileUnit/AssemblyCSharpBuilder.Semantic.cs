using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler;
using Natasha.CSharp.Compiler.SemanticAnalaysis;
using Natasha.CSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 程序集编译构建器 - 语义
/// </summary>
public partial class AssemblyCSharpBuilder 
{

    private readonly List<Func<AssemblyCSharpBuilder, CSharpCompilation, CSharpCompilation>> _semanticAnalysistor;

    public AssemblyCSharpBuilder AddSemanticAnalysistor(Func<AssemblyCSharpBuilder, CSharpCompilation, CSharpCompilation> func)
    {
        _semanticAnalysistor.Add(func);
        return this;
    }

    public AssemblyCSharpBuilder RemoveSemanticAnalysistor(Func<AssemblyCSharpBuilder, CSharpCompilation, CSharpCompilation> func)
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



