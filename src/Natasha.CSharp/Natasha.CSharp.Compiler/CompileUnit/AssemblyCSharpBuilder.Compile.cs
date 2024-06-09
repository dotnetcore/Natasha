using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Compiler.Component;
using Natasha.CSharp.Compiler.Component.Exception;
using Natasha.CSharp.Extension.Inner;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Reflection;
/// <summary>
/// 程序集编译构建器 - 编译选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{
    private Func<IEnumerable<MetadataReference>, IEnumerable<MetadataReference>>? _referencesFilter;
    private CombineReferenceBehavior _combineReferenceBehavior = CombineReferenceBehavior.UseCurrent;
    private readonly ReferenceConfiguration _referenceConfiguration = new();


    /// <summary>
    /// 该方法允许共享域参与编译.
    /// <list type="bullet">
    /// <item>
    /// <description>[共享域] 元数据参与编译.</description>
    /// </item>
    /// <item>
    /// <description>[当前域] 元数据参与编译.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// 注：若两个域不同，且存在相同名称元数据，默认优先使用主域的元数据.
    /// </remarks>
    /// <param name="action">配置同名元数据的解决策略</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithCombineReferences(Action<ReferenceConfiguration>? action = null)
    {
        action?.Invoke(_referenceConfiguration);
        _combineReferenceBehavior = CombineReferenceBehavior.CombineDefault;
        return this;
    }


    /// <summary>
    /// 配置编译元数据的合并行为.
    /// <list type="bullet">
    /// <item>
    /// <description>[共享域] 元数据 [不] 参与编译.</description>
    /// </item>
    /// <item>
    /// <description>[当前域] 元数据参与编译.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithCurrentReferences()
    {
        _combineReferenceBehavior = CombineReferenceBehavior.UseCurrent;
        return this;
    }

    private readonly List<MetadataReference> _specifiedReferences;
    /// <summary>
    /// 使用外部指定的元数据引用进行编译.
    /// <list type="bullet">
    /// <item>
    /// <description>[共享域] 元数据 [不] 参与编译.</description>
    /// </item>
    /// <item>
    /// <description>[当前域] 元数据 [不] 参与编译.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// 使用 ClearOutsideReferences 可以清除本次传递的元数据引用.
    /// </remarks>
    /// <param name="metadataReferences"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithSpecifiedReferences(IEnumerable<MetadataReference> metadataReferences)
    {
        lock (_specifiedReferences)
        {
            _specifiedReferences.AddRange(metadataReferences);
        }
        _combineReferenceBehavior = CombineReferenceBehavior.UseSpecified;
        return this;
    }

    /// <summary>
    /// 清除由 WithSpecifiedReferences 方法传入的元数据引用.
    /// </summary>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder ClearOutsideReferences()
    {
        lock (_specifiedReferences)
        {
            _specifiedReferences.Clear();
        }
        return this;
    }


    /// <summary>
    /// 配置元数据引用过滤策略.
    /// </summary>
    /// <param name="referencesFilter"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder SetReferencesFilter(Func<IEnumerable<MetadataReference>, IEnumerable<MetadataReference>>? referencesFilter)
    {
        _referencesFilter = referencesFilter;
        return this;
    }

    /// <summary>
    /// 流编译成功之后触发的事件.
    /// </summary>
    /// <remarks>
    /// 此时已编译结束，程序集已经生成并加载.
    /// </remarks>
    public event Action<CSharpCompilation, Assembly>? CompileSucceedEvent;


    /// <summary>
    /// 流编译失败之后触发的事件.
    /// </summary>
    /// <remarks>
    /// 此时已经编译结束, 但是编译失败.
    /// </remarks>
    public event Action<CSharpCompilation, ImmutableArray<Diagnostic>>? CompileFailedEvent;


    private ConcurrentQueue<Func<EmitOptions, EmitOptions>>? _emitOptionHandle;
    /// <summary>
    /// 追加对 emitOption 的处理逻辑.
    /// <list type="bullet">
    /// <item>一次性配置，不可重用.</item>
    /// <item>多次调用会进入配置队列.</item>
    /// <item>调用 <see cref="GetAssembly"/> 后清空队列.</item>
    /// <item>调用 <see cref="Clear"/> 后清空队列.</item>
    /// <item>调用 <see cref="ClearEmitOptionCache"/> 后清空队列.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// 注：该配置属于一次性配置，若重复使用该配置逻辑，请在这次编译后重新调用该方法.
    /// </remarks>
    /// <param name="handleAndReturnNewEmitOption"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder ConfigEmitOptions(Func<EmitOptions, EmitOptions> handleAndReturnNewEmitOption)
    {
        _emitOptionHandle ??= new ConcurrentQueue<Func<EmitOptions, EmitOptions>>();
        _emitOptionHandle.Enqueue(handleAndReturnNewEmitOption);
        return this;
    }

    /// <summary>
    /// 编译并获取程序集.
    /// <list type="number">
    /// <item> 编译后的信息获取
    /// <list type="bullet">
    /// <item>用 <see cref="Compilation"/> 获取编译配置载体.</item>
    /// <item>用 <see cref="GetDiagnostics"/> 获取诊断结果.</item>
    /// <item>用 <see cref="GetException"/> 获取运行抛出的异常结果.</item>
    /// <item>用 <see cref="CompileSucceedEvent"/> 监听成功编译结果.</item>
    /// <item>用 <see cref="CompileFailedEvent"/> 监听失败编译结果.</item> 
    /// <code>
    ///     builder.CompileFailedEvent += (compilation, errors) =>{
    ///         log = compilation.GetNatashaLog();
    ///         log.ToString();
    ///     };
    /// </code>
    /// <item>用 <see cref="LogCompilationEvent"/> 监听编译日志.
    /// <code>
    ///     builder.LogCompilationEvent += (log) =>{
    ///         //log.ToString();
    ///     };
    /// </code>
    /// </item>
    /// </list>
    /// </item>
    /// <item>重复编译
    /// <list type="bullet">
    /// <item>查看您所使用过的 Config 开头方法的注释.</item>
    /// <item>用 <see cref="Clear"/> 方法使 Builder 可以继续使用.</item>
    /// <item>用 <see cref="WithoutFileOutput"/> 方法清空上一次记录的输出路径. 如果第二编译需要输出，请重新指定路径.</item>
    /// </list>
    /// </item>
    /// </list>
    /// </summary>
    public void CompileWithoutAssembly()
    {
        GetAvailableCompilation();
        if (Domain!.Name != "Default")
        {
            Domain.SetAssemblyLoadBehavior(_domainConfiguration._dependencyLoadBehavior);
        }

#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif
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

        if (CommentFilePath != string.Empty)
        {
            xmlStream = File.Create(CommentFilePath);
        }

        var debugInfoFormat = _debugConfiguration._informationFormat;
        if (_compilation!.Options.OptimizationLevel == OptimizationLevel.Debug)
        {

            if (debugInfoFormat != DebugInformationFormat.Embedded)
            {
                if (string.IsNullOrEmpty(PdbFilePath))
                {
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"{AssemblyName}.pdb");
                    if (!Directory.Exists(tempPdbOutputFolder))
                    {
                        Directory.CreateDirectory(tempPdbOutputFolder);
                    }
                }
                if (File.Exists(PdbFilePath))
                {
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"repeate.{AssemblyName}.{Guid.NewGuid():N}.pdb");
                    if (!Directory.Exists(tempPdbOutputFolder))
                    {
                        Directory.CreateDirectory(tempPdbOutputFolder);
                    }
                }
                pdbStream = File.Create(PdbFilePath);
            }
            else
            {
                PdbFilePath = null;
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(PdbFilePath))
            {
                PdbFilePath = null;
            }
            debugInfoFormat = 0;
        }
        var emitOption = new EmitOptions(
               //runtimeMetadataVersion: Assembly.GetExecutingAssembly().ImageRuntimeVersion,
               //instrumentationKinds: [InstrumentationKind.TestCoverage],
               includePrivateMembers: _includePrivateMembers,
               metadataOnly: _isReferenceAssembly,
               pdbFilePath: PdbFilePath,
               debugInformationFormat: debugInfoFormat);

        if (_emitOptionHandle != null)
        {
            while (!_emitOptionHandle.IsEmpty)
            {
                while (_emitOptionHandle.TryDequeue(out var func))
                {
                    emitOption = func(emitOption);
                }
            }
        }

        var compileResult = _compilation.Emit(
           dllStream,
           pdbStream: pdbStream,
           xmlDocumentationStream: xmlStream,
           options: emitOption
           );
        LogCompilationEvent?.Invoke(_compilation.GetNatashaLog());

        if (compileResult.Success)
        {
            if (_compilation.Options.OptimizationLevel == OptimizationLevel.Debug)
            {
                pdbStream?.Dispose();
            }
        }
        else
        {
            CompileFailedEvent?.Invoke(_compilation, compileResult.Diagnostics);
            _exception = NatashaExceptionAnalyzer.GetCompileException(_compilation, compileResult.Diagnostics);
            throw _exception;
        }
        dllStream.Dispose();
        pdbStream?.Dispose();
        xmlStream?.Dispose();

#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[  Emit  ]", "编译时长", 2);
#endif
    }


    /// <summary>
    /// 编译并获取程序集.
    /// <list type="number">
    /// <item> 编译后的信息获取
    /// <list type="bullet">
    /// <item>用 <see cref="Compilation"/> 获取编译配置载体.</item>
    /// <item>用 <see cref="GetDiagnostics"/> 获取诊断结果.</item>
    /// <item>用 <see cref="GetException"/> 获取运行抛出的异常结果.</item>
    /// <item>用 <see cref="CompileSucceedEvent"/> 监听成功编译结果.</item>
    /// <item>用 <see cref="CompileFailedEvent"/> 监听失败编译结果.</item> 
    /// <code>
    ///     builder.CompileFailedEvent += (compilation, errors) =>{
    ///         log = compilation.GetNatashaLog();
    ///         log.ToString();
    ///     };
    /// </code>
    /// <item>用 <see cref="LogCompilationEvent"/> 监听编译日志.
    /// <code>
    ///     builder.LogCompilationEvent += (log) =>{
    ///         //log.ToString();
    ///     };
    /// </code>
    /// </item>
    /// </list>
    /// </item>
    /// <item>重复编译
    /// <list type="bullet">
    /// <item>查看您所使用过的 Config 开头方法的注释.</item>
    /// <item>用 <see cref="Clear"/> 方法使 Builder 可以继续使用.</item>
    /// <item>用 <see cref="WithoutFileOutput"/> 方法清空上一次记录的输出路径. 如果第二编译需要输出，请重新指定路径.</item>
    /// </list>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// 注：若不需要加载到域，请使用 CompileWithoutAssembly 方法.
    /// </remarks>
    /// <returns>编译成功生成的程序集.</returns>
    public Assembly GetAssembly()
    {

        GetAvailableCompilation();
        if (Domain!.Name != "Default")
        {
            Domain.SetAssemblyLoadBehavior(_domainConfiguration._dependencyLoadBehavior);
        }

#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif
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

        if (CommentFilePath != string.Empty)
        {
            xmlStream = File.Create(CommentFilePath);
        }

        var debugInfoFormat = _debugConfiguration._informationFormat;
        if (_compilation!.Options.OptimizationLevel == OptimizationLevel.Debug)
        {

            if (debugInfoFormat != DebugInformationFormat.Embedded)
            {
                if (string.IsNullOrEmpty(PdbFilePath))
                {
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"{AssemblyName}.pdb");
                    if (!Directory.Exists(tempPdbOutputFolder))
                    {
                        Directory.CreateDirectory(tempPdbOutputFolder);
                    }
                }
                if (File.Exists(PdbFilePath))
                {
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"repeate.{AssemblyName}.{Guid.NewGuid():N}.pdb");
                    if (!Directory.Exists(tempPdbOutputFolder))
                    {
                        Directory.CreateDirectory(tempPdbOutputFolder);
                    }
                }
                pdbStream = File.Create(PdbFilePath);
            }
            else
            {
                PdbFilePath = null;
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(PdbFilePath))
            {
                PdbFilePath = null;
            }
            debugInfoFormat = 0;
        }
        var emitOption = new EmitOptions(
               //runtimeMetadataVersion: Assembly.GetExecutingAssembly().ImageRuntimeVersion,
               //instrumentationKinds: [InstrumentationKind.TestCoverage],
               includePrivateMembers: _includePrivateMembers,
               metadataOnly: _isReferenceAssembly,
               pdbFilePath: PdbFilePath,
               debugInformationFormat: debugInfoFormat);

        if (_emitOptionHandle != null)
        {
            while (!_emitOptionHandle.IsEmpty)
            {
                while (_emitOptionHandle.TryDequeue(out var func))
                {
                    emitOption = func(emitOption);
                }
            }
        }

        var compileResult = _compilation.Emit(
           dllStream,
           pdbStream: pdbStream,
           xmlDocumentationStream: xmlStream,
           options: emitOption
           );
        LogCompilationEvent?.Invoke(_compilation.GetNatashaLog());
        Assembly assembly;

        if (compileResult.Success)
        {
            if (_compilation.Options.OptimizationLevel == OptimizationLevel.Debug)
            {
                pdbStream?.Dispose();
            }

            dllStream.Seek(0, SeekOrigin.Begin);
            assembly = Domain.LoadAssemblyFromStream(dllStream, null);
            LoadContext!.LoadMetadataWithAssembly(assembly);
            CompileSucceedEvent?.Invoke(_compilation, assembly!);
        }
        else
        {
            CompileFailedEvent?.Invoke(_compilation, compileResult.Diagnostics);
            _exception = NatashaExceptionAnalyzer.GetCompileException(_compilation, compileResult.Diagnostics);
            throw _exception;
        }
        dllStream.Dispose();
        pdbStream?.Dispose();
        xmlStream?.Dispose();

#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[  Emit  ]", "编译时长", 2);
#endif
        return assembly;
    }

    /// <summary>
    /// 热重载相关(未完成，无法使用)
    /// </summary>
    /// <param name="oldAssembly"></param>
    /// <returns></returns>
    private unsafe Assembly UpdateAssembly(Assembly oldAssembly)
    {
        GetAvailableCompilation();
        if (Domain!.Name != "Default")
        {
            Domain.SetAssemblyLoadBehavior(_domainConfiguration._dependencyLoadBehavior);
        }

#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif
        Stream dllStream = new MemoryStream();
        Stream pdbStream = new MemoryStream();
        Stream metaStream = new MemoryStream();
        Stream? xmlStream = null;
        if (DllFilePath != string.Empty)
        {
            dllStream = File.Create(DllFilePath);
        }

        if (CommentFilePath != string.Empty)
        {
            xmlStream = File.Create(CommentFilePath);
        }

        var debugInfoFormat = _debugConfiguration._informationFormat;
        if (_compilation!.Options.OptimizationLevel == OptimizationLevel.Debug)
        {

            if (debugInfoFormat != DebugInformationFormat.Embedded)
            {
                if (string.IsNullOrEmpty(PdbFilePath))
                {
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"{AssemblyName}.pdb");
                    if (!Directory.Exists(tempPdbOutputFolder))
                    {
                        Directory.CreateDirectory(tempPdbOutputFolder);
                    }
                }
                if (File.Exists(PdbFilePath))
                {
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"repeate.{AssemblyName}.{Guid.NewGuid():N}.pdb");
                    if (!Directory.Exists(tempPdbOutputFolder))
                    {
                        Directory.CreateDirectory(tempPdbOutputFolder);
                    }
                }
                pdbStream = File.Create(PdbFilePath);
            }
            else
            {
                PdbFilePath = null;
            }
        }
       
        var emitOption = new EmitOptions(
               //runtimeMetadataVersion: Assembly.GetExecutingAssembly().ImageRuntimeVersion,
               //instrumentationKinds: [InstrumentationKind.TestCoverage],
               includePrivateMembers: _includePrivateMembers,
               metadataOnly: _isReferenceAssembly,
               pdbFilePath: PdbFilePath,
               debugInformationFormat: debugInfoFormat
               );

        if (_emitOptionHandle != null)
        {
            while (!_emitOptionHandle.IsEmpty)
            {
                while (_emitOptionHandle.TryDequeue(out var func))
                {
                    emitOption = func(emitOption);
                }
            }
        }

        var compileResult = _compilation.Emit(
           dllStream,
           pdbStream: pdbStream,
           xmlDocumentationStream: xmlStream,
           //options: emitOption,
           metadataPEStream: metaStream
           );
        LogCompilationEvent?.Invoke(_compilation.GetNatashaLog());


        if (compileResult.Success)
        {
            var ilDelta = AsReadOnlySpan(dllStream);
            var pdbDelta = AsReadOnlySpan(pdbStream);
            var metadataDelta = AsReadOnlySpan(metaStream);
            RuntimeInnerHelper.ApplyUpdate(oldAssembly, metadataDelta, ilDelta, null);
            dllStream.Dispose();
            pdbStream.Dispose();
            metaStream.Dispose();
            xmlStream?.Dispose();
            return oldAssembly!;

        }
        else
        {
            dllStream.Dispose();
            pdbStream.Dispose();
            metaStream.Dispose();
            xmlStream?.Dispose();
            CompileFailedEvent?.Invoke(_compilation, compileResult.Diagnostics);
            _exception = NatashaExceptionAnalyzer.GetCompileException(_compilation, compileResult.Diagnostics);
            throw _exception;
        }

        static ReadOnlySpan<byte> AsReadOnlySpan(Stream input)
        {
            input.Seek(0, SeekOrigin.Begin);
            // 创建一个 MemoryStream 对象来保存 Stream 的数据
            using MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);

            // 将 MemoryStream 的数据转换为 Span 对象
            return ms.GetBuffer().AsSpan();
        }
    }
}
