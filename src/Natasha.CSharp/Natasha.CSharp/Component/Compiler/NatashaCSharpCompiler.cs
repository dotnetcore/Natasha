using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Compiler;
using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Reflection;



public class NatashaCSharpCompiler
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
        AddGlobalSupperess("CS0219");
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


    public bool AllowUnsafe;
    public string AssemblyName;
    public string DllFilePath;
    public string PdbFilePath;
    public string XmlFilePath;
    //public TCompilation? Compilation;
    public OutputKind AssemblyKind;
    public Platform ProcessorPlatform;
    //public AssemblyBuildKind AssemblyOutputKind;
    public OptimizationLevel CodeOptimizationLevel;
    //public Action<TCompilationOptions>? OptionAction;
    public NullableContextOptions NullableCompileOption;
    public bool EnableSemanticHandle;
    public NatashaCSharpCompiler()
    {

        EnableSemanticHandle = true;
        DllFilePath = string.Empty;
        PdbFilePath = string.Empty;
        XmlFilePath = string.Empty;
        NullableCompileOption = NullableContextOptions.Disable;
        this.AssemblyName = Guid.NewGuid().ToString("N");
        _semanticAnalysistor = new List<Func<TCompilation, TCompilation>>();

    }

    private DomainBase? _domain;
    public DomainBase Domain
    {
        get
        {
            if (_domain == null)
            {

                if (AssemblyLoadContext.CurrentContextualReflectionContext != default)
                {
                    _domain = (DomainBase)(AssemblyLoadContext.CurrentContextualReflectionContext);
                }
                else
                {
                    _domain = DomainBase.DefaultDomain;
                }

            }
            return _domain;
        }
        set
        {
            if (value == default)
            {
                value = DomainBase.DefaultDomain;
            }
            _domain = value;
        }
    }


    /// <summary>
    /// 在构建选项创建之后，对选项进行的操作
    /// </summary>
    /// <param name="action"></param>
    public void AddOption(Action<TCompilationOptions> action)
    {
        OptionAction += action;
    }


    /// <summary>
    /// 获取构建编译信息的编译选项
    /// </summary>
    /// <returns></returns>
    public abstract TCompilationOptions GetCompilationOptions();


    /// <summary>
    /// 构建编译信息之前需要做什么
    /// </summary>
    /// <returns></returns>
    public virtual bool PreCompiler()
    {
        return true;
    }


    public IEnumerable<SyntaxTree> SyntaxTrees { get { return Compilation!.SyntaxTrees; } }


    /// <summary>
    /// 获取构建编译信息的选项
    /// </summary>
    /// <returns></returns>
    public abstract TCompilation GetCompilation(TCompilationOptions options);


    /// <summary>
    /// 流编译成功之后触发的事件
    /// </summary>
    public event Action<string, string, Stream, TCompilation>? CompileSucceedEvent;


    /// <summary>
    /// 流编译失败之后触发的事件
    /// </summary>
    public event Func<Stream, ImmutableArray<Diagnostic>, TCompilation, Assembly>? CompileFailedEvent;


    /// <summary>
    /// 用户定义的语义分析器
    /// </summary>
    public List<Func<TCompilation, TCompilation>> _semanticAnalysistor;


    /// <summary>
    /// 设置语义分析
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public void AppendSemanticAnalysistor(Func<TCompilation, TCompilation> action)
    {
        _semanticAnalysistor.Add(action);
    }
    public void SetSemanticAnalysistor(Func<TCompilation, TCompilation> action)
    {
        _semanticAnalysistor.Clear();
        _semanticAnalysistor.Add(action);
    }


    /// <summary>
    /// 将语法树生成到程序集
    /// </summary>
    /// <param name="trees"></param>
    /// <returns></returns>
    public virtual Assembly? ComplieToAssembly(IEnumerable<SyntaxTree> trees)
    {
#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif
        Assembly? assembly = null;
        EmitResult compileResult;
        if (PreCompiler())
        {

            //Mark : 26ms
            var options = GetCompilationOptions();
            OptionAction?.Invoke(options);
            Compilation = GetCompilation(options);
#if DEBUG
            Console.WriteLine();
            stopwatch.StopAndShowCategoreInfo("[Compilation]", "获取编译单元", 2);
            stopwatch.Restart();
#endif
            //Mark : 951ms
            //Mark : 19M Memory
            Compilation = (TCompilation)Compilation.AddSyntaxTrees(trees);
            if (EnableSemanticHandle)
            {
                foreach (var item in _semanticAnalysistor)
                {
                    Compilation = item(Compilation);
                }
#if DEBUG
                stopwatch.StopAndShowCategoreInfo("[Semantic]", "语义解析排查", 2);
                stopwatch.Restart();
#endif
            }

            //Mark : 264ms
            //Mark : 3M Memory
            Stream dllStream;
            Stream? pdbStream = null;
            Stream? xmlStream = null;
            if (DllFilePath != string.Empty)
            {
                dllStream = File.Create(DllFilePath);
            }
            else
            {
                dllStream = new MemoryStream();
            }

            if (PdbFilePath != string.Empty)
            {
                pdbStream = File.Create(PdbFilePath);
            }

            if (XmlFilePath != string.Empty)
            {
                xmlStream = File.Create(XmlFilePath);
            }

            compileResult = Compilation.Emit(
               dllStream,
               pdbStream: pdbStream,
               xmlDocumentationStream: xmlStream,
               options: new EmitOptions(pdbFilePath: pdbStream == null ? null : PdbFilePath, debugInformationFormat: DebugInformationFormat.PortablePdb));


            if (compileResult.Success)
            {

                dllStream.Seek(0, SeekOrigin.Begin);
                if (CompileSucceedEvent != default)
                {
                    MemoryStream copyStream = new();
                    dllStream.CopyTo(copyStream);
                    dllStream.Seek(0, SeekOrigin.Begin);
                    assembly = Domain.CompileStreamCallback(DllFilePath, PdbFilePath, dllStream, AssemblyName);
                    CompileSucceedEvent(DllFilePath, PdbFilePath, copyStream, Compilation);
                    copyStream.Dispose();
                }
                else
                {
                    assembly = Domain.CompileStreamCallback(DllFilePath, PdbFilePath, dllStream, AssemblyName);
                }

            }
            else
            {

                assembly = CompileFailedEvent?.Invoke(dllStream, compileResult.Diagnostics, Compilation);

            }
            dllStream.Dispose();
            pdbStream?.Dispose();
            xmlStream?.Dispose();
#if DEBUG
            stopwatch.StopAndShowCategoreInfo("[  Emit  ]", "编译时长", 2);
#endif

        }
        return assembly;
    }

    public NatashaCSharpCompiler()
    {
#if DEBUG
        CompileFlags = CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes | CompilerBinderFlags.IgnoreAccessibility;
#else
        CompileFlags = CompilerBinderFlags.IgnoreCorLibraryDuplicatedTypes;
#endif

        ReferencesSupersedeLowerVersions = true;
        AllowUnsafe = true;
        AssemblyKind = OutputKind.DynamicallyLinkedLibrary;
        CodeOptimizationLevel = OptimizationLevel.Release;;
        SuppressDiagnostics = _globalSuppressDiagnostics;
        ProcessorPlatform = Platform.AnyCpu;
        SetSemanticAnalysistor(_globalSemanticHandler!);

    }


    public NatashaCSharpCompiler SupportSkipLocalInit()
    {
        if (CompileFlags.HasFlag(CompilerBinderFlags.IgnoreAccessibility))
        {
            CompileFlags = (uint)CompileFlags - CompilerBinderFlags.IgnoreAccessibility;
        }
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



