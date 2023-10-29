#if MULTI
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

    public AssemblyCSharpBuilder():this(Guid.NewGuid().ToString("N"))
    {

    }
    public AssemblyCSharpBuilder(string assemblyName)
    {
        EnableSemanticHandler = true;
        _semanticCheckIgnoreAccessibility = true;
        _combineReferences = true;
        _compileReferenceBehavior = PluginLoadBehavior.UseDefault;
        OutputFolder = GlobalOutputFolder;
        _compilerOptions = new();
        _semanticAnalysistor = new()
        {
            UsingAnalysistor._usingSemanticDelegate
        };
        SyntaxTrees = new();
        AssemblyName = assemblyName;
        DllFilePath = string.Empty;
        PdbFilePath = string.Empty;
        XmlFilePath = string.Empty;
    }

}
#endif


