#if !NETCOREAPP3_0_OR_GREATER
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Component;
using Natasha.CSharp.Component.Exception;
using Natasha.CSharp.Extension.Inner;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Reflection;

/// <summary>
/// 程序集编译构建器 - 编译选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{

    /// <summary>
    /// 流编译成功之后触发的事件
    /// </summary>
    public event Action<CSharpCompilation, Assembly>? CompileSucceedEvent;


    /// <summary>
    /// 流编译失败之后触发的事件
    /// </summary>
    public event Action<CSharpCompilation, ImmutableArray<Diagnostic>>? CompileFailedEvent;


    public CSharpCompilation GetAvailableCompilation()
    {
#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif

        var options = _compilerOptions.GetCompilationOptions(_codeOptimizationLevel);
        var references = NatashaReferenceCache.GetReferences();
        _compilation = CSharpCompilation.Create(AssemblyName, SyntaxTrees, references, options);

#if DEBUG
        stopwatch.RestartAndShowCategoreInfo("[Compiler]", "获取编译单元", 2);
#endif


        if (EnableSemanticHandler)
        {
            foreach (var item in _semanticAnalysistor)
            {
                _compilation = item(this, _compilation, _semanticCheckIgnoreAccessibility);
            }
        }

#if DEBUG
        stopwatch.RestartAndShowCategoreInfo("[Semantic]", "语义处理", 2);
#endif
        return _compilation;
    }
    
}

#endif

