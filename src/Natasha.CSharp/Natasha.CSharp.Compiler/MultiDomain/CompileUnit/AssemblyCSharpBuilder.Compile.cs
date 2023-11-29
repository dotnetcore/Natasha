using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

#if NETCOREAPP3_0_OR_GREATER
using System.IO;
using System.Reflection;
using Natasha.CSharp.Component.Domain;
/// <summary>
/// 程序集编译构建器 - 编译选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{
    
    private Func<IEnumerable<MetadataReference>, IEnumerable<MetadataReference>>? _referencesFilter;
    private CombineReferenceBehavior _combineReferenceBehavior;
    private ReferenceConfiguration _referenceConfiguration = new();


    /// <summary>
    /// 编译时，使用主域引用覆盖引用集,并配置同名引用版本行为
    /// </summary>
    /// <param name="action">配置委托</param>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithCombineReferences(Action<ReferenceConfiguration>? action = null)
    {
        action?.Invoke(_referenceConfiguration);
        _combineReferenceBehavior = CombineReferenceBehavior.CombineDefault;
        return this;
    }

    /// <summary>
    /// 编译时，不使用主域引用覆盖引用集
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithoutCombineReferences()
    {
        _combineReferenceBehavior = CombineReferenceBehavior.UseCurrent;
        return this;
    }

    /// <summary>
    /// 配置引用过滤策略
    /// </summary>
    /// <param name="referencesFilter"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigReferencesFilter(Func<IEnumerable<MetadataReference>, IEnumerable<MetadataReference>>? referencesFilter)
    {
        _referencesFilter = referencesFilter;
        return this;
    }

    /// <summary>
    /// 流编译成功之后触发的事件
    /// </summary>
    public event Action<CSharpCompilation, Assembly>? CompileSucceedEvent;



    /// <summary>
    /// 流编译失败之后触发的事件
    /// </summary>
    public event Action<CSharpCompilation, ImmutableArray<Diagnostic>>? CompileFailedEvent;


    public CSharpCompilation GetAvailableCompilation(Func<CSharpCompilationOptions, CSharpCompilationOptions>? initOptionsFunc = null)
    {
#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif

        //Mark : 26ms
        //if (_compileReferenceBehavior == PluginLoadBehavior.None)
        //{
        //    _compilerOptions.WithLowerVersionsAssembly();
        //}

        var options = _compilerOptions.GetCompilationOptions(_codeOptimizationLevel);
        if (initOptionsFunc != null)
        {
            options = initOptionsFunc(options);
        }
        IEnumerable<MetadataReference> references;
        if (_combineReferenceBehavior == CombineReferenceBehavior.CombineDefault)
        {
            references = Domain.GetReferences(_referenceConfiguration);
        }
        else
        {
            references = Domain.References.GetReferences();
        }

        if (_referencesFilter != null)
        {
            references = _referencesFilter(references);
        }

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
        stopwatch.StopAndShowCategoreInfo("[Semantic]", "语义处理", 2);
#endif
        return _compilation;
    }


}
#endif


