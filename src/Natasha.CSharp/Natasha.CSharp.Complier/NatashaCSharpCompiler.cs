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
    public readonly static ConcurrentDictionary<string, ReportDiagnostic> GlobalSuppressDiagnostics;

    public static void AddGlobalSupperess(string errorcode)
    {
        GlobalSuppressDiagnostics[errorcode] = ReportDiagnostic.Suppress;
    }


    public CompilerBinderFlags CompileFlags;
    public bool ReferencesSupersedeLowerVersions;
    public ConcurrentDictionary<string, ReportDiagnostic> SuppressDiagnostics;
    public static readonly Action<CSharpCompilationOptions, uint> SetTopLevelBinderFlagDelegate;
    public static readonly Action<CSharpCompilationOptions, bool> SetReferencesSupersedeLowerVersionsDelegate;


    static NatashaCSharpCompiler()
    {

        GlobalSuppressDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>();
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
            .GetProperty("TopLevelBinderFlags", BindingFlags.Instance | BindingFlags.NonPublic)
            .SetMethod);

        SetReferencesSupersedeLowerVersionsDelegate = (Action<CompilationOptions, bool>)Delegate.CreateDelegate(
            typeof(Action<CompilationOptions, bool>), typeof(CompilationOptions)
            .GetProperty("ReferencesSupersedeLowerVersions", BindingFlags.Instance | BindingFlags.NonPublic)
            .SetMethod);

    }

    public NatashaCSharpCompiler()
    {
        CompileFlags = CompilerBinderFlags.IgnoreAccessibility | CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes;
        ReferencesSupersedeLowerVersions = true;
        AllowUnsafe = true;
        Enum_OutputKind = OutputKind.DynamicallyLinkedLibrary;
        Enum_OptimizationLevel = OptimizationLevel.Release;
        AssemblyOutputKind = AssemblyBuildKind.Stream;
        SuppressDiagnostics = GlobalSuppressDiagnostics;
        Enum_Platform = Platform.AnyCpu;
        SuppressDiagnostics = new ConcurrentDictionary<string, ReportDiagnostic>();

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
                               //strongNameProvider: a,
                               concurrentBuild: true,
                               moduleName: Guid.NewGuid().ToString(),
                               reportSuppressedDiagnostics: false,
                               metadataImportOptions: MetadataImportOptions.All,
                               outputKind: Enum_OutputKind,
                               optimizationLevel: Enum_OptimizationLevel,
                               allowUnsafe: AllowUnsafe,
                               platform: Enum_Platform,
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
       
        Compilation = CSharpCompilation.Create(AssemblyName, CompileTrees, Domain.GetCompileReferences(), options);
        return Compilation;

    }


    /// <summary>
    /// 绑定编译标识
    /// </summary>
    /// <param name="flags"></param>
    public void SetCompilerBindingFlag(CompilerBinderFlags flags)
    {
        CompileFlags = flags;
    }


    /// <summary>
    /// 自动禁用低版本程序集
    /// </summary>
    /// <param name="value"></param>
    public void SetReferencesSupersedeLowerVersions(bool value)
    {
        ReferencesSupersedeLowerVersions = value;
    }


    /// <summary>
    /// 重写方法，将编译信息编译到文件
    /// </summary>
    /// <param name="compilation"></param>
    /// <returns></returns>
    public override EmitResult CompileEmitToFile(CSharpCompilation compilation)
    {

        EmitResult CompileResult;
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            CompileResult = compilation.Emit(DllPath, PdbPath);
        }
        else
        {
            CompileResult = compilation.UnixEmit(DllPath, PdbPath);
        }
        return CompileResult;

    }


    /// <summary>
    /// 重写方法，将编译信息编译到内存流
    /// </summary>
    /// <param name="compilation"></param>
    /// <param name="stream"></param>
    /// <returns></returns>
    public override EmitResult CompileEmitToStream(CSharpCompilation compilation, MemoryStream stream)
    {
        EmitResult CompileResult;
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
            CompileResult = compilation.Emit(stream);
        }
        else
        {
            CompileResult = compilation.Emit(stream, options: new EmitOptions(false, DebugInformationFormat.PortablePdb));
        }
        return CompileResult;
    }
}



