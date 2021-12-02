using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Compiler;
using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;



public class NatashaCSharpCompiler : CompilerBase<CSharpCompilation, CSharpCompilationOptions>
{

    /// <summary>
    /// 被禁断的错误代码
    /// </summary>
    private static readonly ConcurrentDictionary<string, ReportDiagnostic> _globalSuppressDiagnostics;
    private static Func<CSharpCompilation, CSharpCompilation>? _globalSemanticHandler;

    public static void AddGlobalSupperess(string errorcode)
    {
        _globalSuppressDiagnostics[errorcode] = ReportDiagnostic.Suppress;
    }
    public static void AddSemanticAnalysistor(Func<CSharpCompilation, CSharpCompilation> globalSemanticHandler)
    {
        _globalSemanticHandler = globalSemanticHandler;
    }

    
    public CompilerBinderFlags CompileFlags;
    public bool ReferencesSupersedeLowerVersions;
    public ConcurrentDictionary<string, ReportDiagnostic> SuppressDiagnostics;
    public static readonly Action<CSharpCompilationOptions, uint> SetTopLevelBinderFlagDelegate;
    public static readonly Action<CSharpCompilationOptions, bool> SetReferencesSupersedeLowerVersionsDelegate;
    private CSharpCompilation? _compilation;

    static NatashaCSharpCompiler()
    {

        _globalSuppressDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>();
        AddGlobalSupperess("CS1701");
        AddGlobalSupperess("CS1702");
        AddGlobalSupperess("CS1705");
        AddGlobalSupperess("CS162");
        AddGlobalSupperess("CS219");
        AddGlobalSupperess("CS0414");
        AddGlobalSupperess("CS0616");
        AddGlobalSupperess("CS0649");
        AddGlobalSupperess("CS0693");
        AddGlobalSupperess("CS1591");
        AddGlobalSupperess("CS1998");

        // CS8019
        // CS0162 - Unreachable code detected.
        // CS0219 - The variable 'V' is assigned but its value is never used.
        // CS0414 - The private field 'F' is assigned but its value is never used.
        // CS0616 - Member is obsolete.
        // CS0649 - Field 'F' is never assigned to, and will always have its default value.
        // CS0693 - Type parameter 'type parameter' has the same name as the type parameter from outer type 'T'
        // CS1591 - Missing XML comment for publicly visible type or member 'Type_or_Member'
        // CS1998 - This async method lacks 'await' operators and will run synchronously

        SetTopLevelBinderFlagDelegate = (Action<CSharpCompilationOptions, uint>)Delegate.CreateDelegate(
            typeof(Action<CSharpCompilationOptions, uint>), typeof(CSharpCompilationOptions)
            .GetProperty("TopLevelBinderFlags", BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetMethod!);

        SetReferencesSupersedeLowerVersionsDelegate = (Action<CompilationOptions, bool>)Delegate.CreateDelegate(
            typeof(Action<CompilationOptions, bool>), typeof(CompilationOptions)
            .GetProperty("ReferencesSupersedeLowerVersions", BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetMethod!);

    }

    public NatashaCSharpCompiler()
    {

        CompileFlags = CompilerBinderFlags.IgnoreAccessibility | CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes;
        ReferencesSupersedeLowerVersions = true;
        AllowUnsafe = true;
        AssemblyKind = OutputKind.DynamicallyLinkedLibrary;
        CodeOptimizationLevel = OptimizationLevel.Release;
        AssemblyOutputKind = AssemblyBuildKind.Stream;
        SuppressDiagnostics = _globalSuppressDiagnostics;
        ProcessorPlatform = Platform.AnyCpu;
        SetSemanticAnalysistor(_globalSemanticHandler!);
        //SuppressDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>();

    }


    public NatashaCSharpCompiler SupportSkipLocalInit()
    {
        CompileFlags = (uint)CompileFlags - CompilerBinderFlags.IgnoreAccessibility;
        return this;
    }



    /// <summary>
    /// 获取构建编译信息的选项
    /// </summary>
    /// <returns></returns>
    public override CSharpCompilationOptions GetCompilationOptions()
    {
        //var a = new DesktopStrongNameProvider(ImmutableArray.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "natasha.snk")));
        //CS0012  DesktopAssembly
        var compilationOptions = new CSharpCompilationOptions(
                                nullableContextOptions: NullableCompileOption,
                               //strongNameProvider: a,
                               deterministic:false,
                               concurrentBuild: true,
                               moduleName: Guid.NewGuid().ToString(),
                               reportSuppressedDiagnostics: false,
                               metadataImportOptions: MetadataImportOptions.All,
                               outputKind: AssemblyKind,
                               optimizationLevel: CodeOptimizationLevel,
                               allowUnsafe: AllowUnsafe,
                               platform: ProcessorPlatform,
                               checkOverflow: false,
                               assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default,
                               specificDiagnosticOptions: SuppressDiagnostics);
        if (CompileFlags != 0)
        {
            SetTopLevelBinderFlagDelegate(compilationOptions, (uint)CompileFlags);
        }
        //CS1704
        SetReferencesSupersedeLowerVersionsDelegate(compilationOptions, ReferencesSupersedeLowerVersions);
        return compilationOptions;

    }


    /// <summary>
    /// 获取编译选项
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public override CSharpCompilation GetCompilation(CSharpCompilationOptions options)
    {
        if (_compilation==default)
        {
            _compilation = CSharpCompilation.Create(AssemblyName, null, Domain.GetCompileReferences(), options);
        }
        return _compilation.RemoveAllSyntaxTrees();

    }


    /// <summary>
    /// 绑定编译标识
    /// </summary>
    /// <param name="flags"></param>
    public NatashaCSharpCompiler SetCompilerBindingFlag(CompilerBinderFlags flags)
    {
        CompileFlags = flags;
        return this;
    }


    /// <summary>
    /// 自动禁用低版本程序集
    /// </summary>
    /// <param name="value"></param>
    public NatashaCSharpCompiler SetReferencesSupersedeLowerVersions(bool value)
    {
        ReferencesSupersedeLowerVersions = value;
        return this;
    }


    
}



