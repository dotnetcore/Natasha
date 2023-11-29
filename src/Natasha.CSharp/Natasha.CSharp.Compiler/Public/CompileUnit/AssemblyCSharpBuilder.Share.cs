using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using System;

public sealed partial class AssemblyCSharpBuilder
{
    private DebugConfiguration _debugConfiguration = new();

    private bool _isReferenceAssembly;

    private bool _includePrivateMembers;

    /// <summary>
    /// pdb文件包含私有字段信息
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithPrivateMembers()
    {
        _includePrivateMembers = true;
        return this;
    }
    /// <summary>
    /// pdb文件不包含私有字段信息
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
    /// 是否以完全程序集方式输出
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder OutputAsFullAssembly()
    {
        _isReferenceAssembly = false;
        return this;
    }

    /// <summary>
    /// 清空编译信息, 下次编译重组 Compilation 和语法树.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder ClearCompilationCache()
    {
        _compilation = null;
        return this;
    }

    /// <summary>
    /// 清空所有记录,包括编译信息和脚本记录,以及程序集名称.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder Clear()
    {
        _compilation = null;
        SyntaxTrees.Clear();
        AssemblyName = string.Empty;
        return this;
    }


    private OptimizationLevel _codeOptimizationLevel = OptimizationLevel.Release;
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
    public AssemblyCSharpBuilder WithReleaseCompile()
    {
        _codeOptimizationLevel = OptimizationLevel.Release;
        return this;
    }


    /// <summary>
    /// 自动使用 GUID 作为程序集名称.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithRandomAssenblyName()
    {
        AssemblyName = Guid.NewGuid().ToString("N");
        return this;
    }


}

