using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler;
using System;

/// <summary>
/// 程序集编译构建器 - 编译选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{
    private readonly NatashaCSharpCompilerOptions _compilerOptions;
    private CSharpCompilation? _compilation;
    public CSharpCompilation? Compilation { get { return _compilation; } }
    /// <summary>
    /// 配置编译选项. 此方法传入的实例 instance. <br/>
    /// </summary>
    /// <remarks>
    /// <example>
    /// <code>
    /// 
    ///     //默认配置
    ///     
    ///     //关闭空引用支持
    ///     opt=>opt.SetNullableCompile(NullableContextOptions.Disable)
    ///     
    ///     //不处理同名不同版本的引用
    ///     .SetSupersedeLowerVersions(false)
    ///     
    ///     //输出方式为dll
    ///     .SetOutputKind(OutputKind.DynamicallyLinkedLibrary)
    ///     
    ///     //启用 Release 优化
    ///     .CompileAsRelease()
    ///     
    ///     //支持 Unsafe 编译
    ///     .SetUnsafeCompiler(true)
    ///     
    ///     //任何 CPU 平台
    ///     .SetPlatform(Platform.AnyCpu);
    ///
    /// </code>
    /// </example>
    /// </remarks>
    public AssemblyCSharpBuilder ConfigCompilerOption(Action<NatashaCSharpCompilerOptions> action)
    {
        action(_compilerOptions);
        return this;
    }
}



