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
 
    public AssemblyCSharpBuilder ConfigCompilerOption(Action<NatashaCSharpCompilerOptions> action)
    {
        action(_compilerOptions);
        return this;
    }
}



