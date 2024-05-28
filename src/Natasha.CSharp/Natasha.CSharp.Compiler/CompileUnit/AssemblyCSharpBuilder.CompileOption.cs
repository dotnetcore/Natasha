using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler.Component;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
/// <summary>
/// 程序集编译构建器 - 编译选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{
    private readonly NatashaCSharpCompilerOptions _compilerOptions;
    private CSharpCompilation? _compilation;
    public CSharpCompilation? Compilation { get { return _compilation; } }

    /// <summary>
    /// 配置编译选项.
    /// <list type="table">
    /// <item>复用 Builder 场景:
    /// <list type="bullet">
    /// <item>
    ///     该方法为一次性方法，配置不会被缓存.
    /// </item>
    /// <item>
    ///     用 <see cref="Clear"/> 方法后，重调此方法并传入您的配置逻辑.
    /// </item>
    /// </list>
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="action">配置 [编译载体] 的逻辑.</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder ConfigCompilerOption(Action<NatashaCSharpCompilerOptions> action)
    {
        action(_compilerOptions);
        return this;
    }

    /// <summary>
    /// 获取当前 [编译载体] 的诊断信息.
    /// </summary>
    /// <returns>诊断信息集合</returns>
    public ImmutableArray<Diagnostic>? GetDiagnostics()
    {
        return _compilation?.GetDiagnostics();
    }

    private bool _isReferenceAssembly;

    private bool _includePrivateMembers;

    /// <summary>
    /// 输出文件包含私有字段信息.
    /// </summary>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithPrivateMembers()
    {
        _includePrivateMembers = true;
        return this;
    }
    /// <summary>
    /// 输出文件不包含私有字段信息(默认).
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithoutPrivateMembers()
    {
        _includePrivateMembers = false;
        return this;
    }

    /// <summary>
    /// 是否以引用程序集方式输出.
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder OutputAsRefAssembly()
    {
        _isReferenceAssembly = true;
        _includePrivateMembers = false;
        return this;
    }
    /// <summary>
    /// 是否以完全程序集方式输出(默认).
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder OutputAsFullAssembly()
    {
        _isReferenceAssembly = false;
        return this;
    }

    private readonly DebugConfiguration _debugConfiguration = new();
    private bool _withDebugInfo;
    private OptimizationLevel _codeOptimizationLevel;

    /// <summary>
    /// 编译时使用 Debug 模式.
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithDebugCompile(Action<DebugConfiguration>? action = null)
    {
        action?.Invoke(_debugConfiguration);
        _codeOptimizationLevel = OptimizationLevel.Debug;
        return this;
    }
    /// <summary>
    /// 编译时使用 Release 模式优化（默认）.
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithReleaseCompile()
    {
        _withDebugInfo = false;
        _codeOptimizationLevel = OptimizationLevel.Release;
        return this;
    }
    /// <summary>
    /// 编译时使用携带有 DebugInfo 的 Release 模式优化（默认）.
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithFullReleaseCompile()
    {
        _withDebugInfo = true;
        _codeOptimizationLevel = OptimizationLevel.Release;
        return this;
    }

    /// <summary>
    /// 获取一个配置好的 [编译载体].
    /// <list type="bullet">
    /// <item>
    ///     语法树已经完成格式化.
    /// </item>
    /// <item>
    ///     语法树已经完成语义过滤(若开启).
    /// </item>
    /// <item>
    ///     元数据已经填充完毕.
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="initOptionsFunc"></param>
    /// <returns>编译载体.</returns>
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
        var options = _compilerOptions.GetCompilationOptions(_codeOptimizationLevel, _withDebugInfo);
        if (initOptionsFunc != null)
        {
            options = initOptionsFunc(options);
        }
        IEnumerable<MetadataReference> references;
        if (_combineReferenceBehavior == CombineReferenceBehavior.CombineDefault)
        {
            references = LoadContext!.GetReferences(_referenceConfiguration);
        }
        else if (_combineReferenceBehavior == CombineReferenceBehavior.UseCurrent)
        {
            references = LoadContext!.ReferenceRecorder.GetReferences();
        }
        else
        {
            references = _specifiedReferences;
        }

        if (_referencesFilter != null)
        {
            references = _referencesFilter(references);
        }

        _compilation = CSharpCompilation.Create(AssemblyName, SyntaxTrees, references, options);
#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[Compiler]", "获取编译单元", 2);
#endif

        if (EnableSemanticHandler)
        {
            foreach (var item in _semanticAnalysistor)
            {
                _compilation = item(this, _compilation, _semanticCheckIgnoreAccessibility);
            }
        }
        return _compilation;
    }
}



