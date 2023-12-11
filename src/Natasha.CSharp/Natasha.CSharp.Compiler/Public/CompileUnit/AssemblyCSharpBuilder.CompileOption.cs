using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler;
using System;
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
    /// 编译时使用 debug 模式
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithDebugCompile(Action<DebugConfiguration>? action = null)
    {
        action?.Invoke(_debugConfiguration);
        _codeOptimizationLevel = OptimizationLevel.Debug;
        return this;
    }
    /// <summary>
    /// 编译时使用 release 模式优化（默认）
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithReleaseCompile(bool withDebugInfo = false)
    {
        _withDebugInfo = withDebugInfo;
        _codeOptimizationLevel = OptimizationLevel.Release;
        return this;
    }

}



