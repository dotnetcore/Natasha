using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Compiler.Component.Exception;
using Natasha.CSharp.Compiler.Utils;
using Natasha.CSharp.Extension.Inner;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
/// <summary>
/// 程序集编译构建器 - 编译器
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{
    /// <summary>
    /// 重复编译
    /// <list type="bullet">
    ///     <item>
    ///     当前方法逻辑为：
    ///         <list type="bullet">
    ///             <item>调用 WithPreCompilationOptions() 阻止创建新的编译选项.</item>
    ///             <item>调用 WithPreCompilationReferences() 阻止覆盖新的引用. </item>
    ///         </list>
    ///     </item>
    ///     <item>
    ///     提示：
    ///         <list type="bullet">
    ///             <item>若之前使用了 ConfigEmitOptions. 需要重新再写一遍. </item>
    ///             <item>若已存在文件 a.dll，则生成 repeate.guid.a.dll.</item>
    ///             <item>WithForceCleanFile(); 可强制清除已存在文件.</item>
    ///             <item>WithoutForceCleanFile(); 则 a.dll 被换成 repeate.guid.a.dll.</item>
    ///             <item>别忘了指定新的程序集名.</item>
    ///             <item>若需要指定新的域.</item>
    ///         </list>
    ///     </item>
    /// </list>
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder Reset()
    {
        WithPreCompilationOptions();
        WithPreCompilationReferences();
        return this;
    }

    /// <summary>
    /// 编译并获取程序集.
    /// <list type="bullet">
    /// <item>用 <see cref="Compilation"/> 获取编译配置载体.</item>
    /// <item>用 <see cref="GetDiagnostics"/> 获取诊断结果.</item>
    /// <item>用 <see cref="GetException"/> 获取抛出的异常结果.</item>
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
        using Stream dllStream = DllFilePath != string.Empty ? File.Create(FileHandle(DllFilePath)) : new MemoryStream();
        Stream ? pdbStream = null;
        Stream? xmlStream = null;
        if (CommentFilePath != string.Empty)
        {
            xmlStream = File.Create(FileHandle(CommentFilePath));
        }

        var debugInfoFormat = _debugConfiguration._informationFormat;
        HashAlgorithmName? hashAlgorithm = null;
        if (_compilation!.Options.OptimizationLevel == OptimizationLevel.Debug)
        {
            debugInfoFormat ??= PdbHelpers.GetPlatformSpecificDebugInformationFormat();
            if (debugInfoFormat != DebugInformationFormat.Embedded)
            {
                hashAlgorithm = default(HashAlgorithmName);
                if (string.IsNullOrEmpty(PdbFilePath))
                {
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"{AssemblyName}.pdb");
                    if (!Directory.Exists(tempPdbOutputFolder))
                    {
                        Directory.CreateDirectory(tempPdbOutputFolder);
                    }
                }
                pdbStream = File.Create(FileHandle(PdbFilePath));
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
               debugInformationFormat: debugInfoFormat.Value,
               pdbChecksumAlgorithm: hashAlgorithm);

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

        dllStream.Dispose();
        pdbStream?.Dispose();
        xmlStream?.Dispose();
        LogCompilationEvent?.Invoke(_compilation.GetNatashaLog());
        if (!compileResult.Success)
        {
            CompileFailedEvent?.Invoke(_compilation, compileResult.Diagnostics);
            _exception = NatashaExceptionAnalyzer.GetCompileException(_compilation, compileResult.Diagnostics);
            throw _exception;
        }


#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[  Emit  ]", "编译时长", 2);
#endif
    }

    /// <summary>
    /// 编译并获取程序集.
    /// <list type="bullet">
    /// <item>用 <see cref="Compilation"/> 获取编译配置载体.</item>
    /// <item>用 <see cref="GetDiagnostics"/> 获取诊断结果.</item>
    /// <item>用 <see cref="GetException"/> 获取抛出的异常结果.</item>
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
            dllStream = File.Create(FileHandle(DllFilePath));
        }
        else
        {
            dllStream = new MemoryStream();
        }
        if (CommentFilePath != string.Empty)
        {
            xmlStream = File.Create(FileHandle(CommentFilePath));
        }

        var debugInfoFormat = _debugConfiguration._informationFormat;
        HashAlgorithmName? hashAlgorithm = null;
        if (_compilation!.Options.OptimizationLevel == OptimizationLevel.Debug)
        {
            debugInfoFormat ??= PdbHelpers.GetPlatformSpecificDebugInformationFormat();
            if (debugInfoFormat == DebugInformationFormat.Embedded)
            {
                PdbFilePath = null;
            }
            else
            {
                hashAlgorithm = default(HashAlgorithmName);
                if (string.IsNullOrEmpty(PdbFilePath))
                {
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"{AssemblyName}.pdb");
                    if (!Directory.Exists(tempPdbOutputFolder))
                    {
                        Directory.CreateDirectory(tempPdbOutputFolder);
                    }
                }
                pdbStream = File.Create(FileHandle(PdbFilePath));
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
               debugInformationFormat: debugInfoFormat.Value,
               pdbChecksumAlgorithm: hashAlgorithm
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
           options: emitOption
           );
        LogCompilationEvent?.Invoke(_compilation.GetNatashaLog());
        Assembly assembly;

        if (compileResult.Success)
        {
            dllStream.Position = 0;
            assembly = Domain.LoadAssemblyFromStream(dllStream, null);
            LoadContext!.LoadMetadataWithAssembly(assembly);
            CompileSucceedEvent?.Invoke(_compilation, assembly!);
            pdbStream?.Dispose();
            xmlStream?.Dispose();
        }
        else
        {
            pdbStream?.Dispose();
            xmlStream?.Dispose();
            CompileFailedEvent?.Invoke(_compilation, compileResult.Diagnostics);
            _exception = NatashaExceptionAnalyzer.GetCompileException(_compilation, compileResult.Diagnostics);
            throw _exception;
        }


#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[  Emit  ]", "编译时长", 2);
#endif
        return assembly;
    }
}
