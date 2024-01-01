using Natasha.CSharp.Compiler.SemanticAnalaysis;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// 程序集编译构建器
/// 默认开启语义过滤
/// 默认域内引用优先
/// 默认GUID作为程序集名
/// </summary>
[assembly: InternalsVisibleTo("NatashaFunctionUT, PublicKey=002400000480000094000000060200000024000052534131000400000100010069acb31dd0d9918441d6ed2b49cd67ae17d15fd6ded4ccd2f99b4a88df8cddacbf72d5897bb54f406b037688d99f482ff1c3088638b95364ef614f01c3f3f2a2a75889aa53286865463fb1803876056c8b98ec57f0b3cf2b1185de63d37041ba08f81ddba0dccf81efcdbdc912032e8d2b0efa21accc96206c386b574b9d9cb8")]
public sealed partial class AssemblyCSharpBuilder
{

    public AssemblyCSharpBuilder() : this(Guid.NewGuid().ToString("N"))
    {

    }
    public AssemblyCSharpBuilder(string assemblyName)
    {
        _parsingBehavior = UsingLoadBehavior.None;
        OutputFolder = GlobalOutputFolder;
        _compilerOptions = new();
        SyntaxTrees = [];
        AssemblyName = assemblyName;
        DllFilePath = string.Empty;
        CommentFilePath = string.Empty;
        _domainConfiguration = new DomainConfiguration();
        _semanticAnalysistor = [UsingAnalysistor._usingSemanticDelegate];
        _specifiedReferences = [];
    }

    internal static bool HasInitialized;
    /// <summary>
    /// 清空编译信息, 下次编译重组 Compilation .
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


    /// <summary>
    /// 自动使用 GUID 作为程序集名称.
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithRandomAssenblyName()
    {
        AssemblyName = Guid.NewGuid().ToString("N");
        return this;
    }

    /// <summary>
    /// 轻便模式：无合并行为，使用当前域的 Using，无语义检查
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder UseSimpleMode()
    {
        this
                .WithCurrentReferences()
                .WithCombineUsingCode(UsingLoadBehavior.WithCurrent)
                .WithReleaseCompile()
                .WithoutSemanticCheck();

        return this;
    }
    /// <summary>
    /// 智能模式：默认合并所有元数据，合并当前域及主域 Using，开启语义检查
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder UseSmartMode()
    {
        this
                .WithCombineReferences(item => item.UseAllReferences())
                .WithCombineUsingCode(UsingLoadBehavior.WithAll)
                .WithReleaseCompile()
                .WithSemanticCheck();

        return this;
    }
}


