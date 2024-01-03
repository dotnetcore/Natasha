using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler.Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;
/// <summary>
/// 程序集编译构建器 - 编译选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{
    private readonly NatashaCSharpCompilerOptions _compilerOptions;
    private CSharpCompilation? _compilation;
    public CSharpCompilation? Compilation { get { return _compilation; } }
 
    public AssemblyCSharpBuilder ConfigCompilerOption(Action<NatashaCSharpCompilerOptions> action)
    {
        action(_compilerOptions);
        return this;
    }

    private bool _isReferenceAssembly;

    private bool _includePrivateMembers;

    /// <summary>
    /// 输出文件包含私有字段信息
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithPrivateMembers()
    {
        _includePrivateMembers = true;
        return this;
    }
    /// <summary>
    /// 输出文件不包含私有字段信息(默认)
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithoutPrivateMembers()
    {
        _includePrivateMembers = false;
        return this;
    }

    /// <summary>
    /// 是否以引用程序集方式输出
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder OutputAsRefAssembly()
    {
        _isReferenceAssembly = true;
        _includePrivateMembers = false;
        return this;
    }
    /// <summary>
    /// 是否以完全程序集方式输出(默认)
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder OutputAsFullAssembly()
    {
        _isReferenceAssembly = false;
        return this;
    }

    private readonly DebugConfiguration _debugConfiguration = new();
    private bool _withDebugInfo;
    private OptimizationLevel _codeOptimizationLevel;

    /// <summary>
    /// 编译时使用 Debug 模式
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithDebugCompile(Action<DebugConfiguration>? action = null)
    {
        action?.Invoke(_debugConfiguration);
        _codeOptimizationLevel = OptimizationLevel.Debug;
        return this;
    }
    /// <summary>
    /// 编译时使用 Release 模式优化（默认）
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithReleaseCompile()
    {
        _withDebugInfo = false;
        _codeOptimizationLevel = OptimizationLevel.Release;
        return this;
    }
    /// <summary>
    /// 编译时使用携带有 DebugInfo 的 Release 模式优化（默认）
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithFullReleaseCompile()
    {
        _withDebugInfo = true;
        _codeOptimizationLevel = OptimizationLevel.Release;
        return this;
    }


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



