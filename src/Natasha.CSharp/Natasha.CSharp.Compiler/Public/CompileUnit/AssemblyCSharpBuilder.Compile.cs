using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Component.Exception;
using Natasha.CSharp.Extension.Inner;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;




#if NETCOREAPP3_0_OR_GREATER
using Natasha.CSharp.Component.Domain;
#else
using Natasha.CSharp.Component;
#endif
/// <summary>
/// 程序集编译构建器 - 编译选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{
    private ConcurrentQueue<Func<EmitOptions, EmitOptions>>? _emitOptionHandle;
    /// <summary>
    /// 处理默认生成的 emitOption 并返回一个新的
    /// </summary>
    /// <param name="handleAndReturnNewEmitOption"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigEmitOptions(Func<EmitOptions, EmitOptions> handleAndReturnNewEmitOption)
    {
        _emitOptionHandle ??= new ConcurrentQueue<Func<EmitOptions, EmitOptions>>();
        _emitOptionHandle.Enqueue(handleAndReturnNewEmitOption);
        return this;
    }

    private bool _notLoadIntoDomain;
    /// <summary>
    /// 仅仅生成程序集，而不加载到域。
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithoutInjectToDomain()
    {
        _notLoadIntoDomain = true;
        return this;
    }
    /// <summary>
    /// 既生成程序集又加载到域。
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithInjectToDomain()
    {
        _notLoadIntoDomain = false;
        return this;
    }

    /// <summary>
    /// 将 SyntaxTrees 中的语法树编译到程序集.如果不成功会抛出 NatashaException.
    /// </summary>
    /// <remarks>
    /// <example>
    /// <code>
    /// 
    ///     //Core3.0以上版本
    ///     //程序集的域加载行为决定了编译后的程序集随着依赖加载到域中的处理结果.
    ///     //编译单元支持的依赖加载方法:
    ///     WithHighVersionDependency
    ///     WithLowVersionDependency
    ///     WithDefaultVersionDependency
    ///     WithCustomVersionDependency
    ///     
    ///     //编译单元的引用加载行为, 遇到同名不同版本的引用该如何处理.
    ///     //首先启用合并引用，此方法将允许主域引用和当前域引用进行整合
    ///     builder.WithCombineReferences(configAction);
    ///     //其参数支持同名引用的加载逻辑，包括
    ///         config.UseHighVersionReferences
    ///         config.UseLowVersionReferences
    ///         config.UseDefaultReferences
    ///         config.UseCustomReferences
    ///         //手动设置同名过滤器
    ///         config.ConfigSameNameReferencesFilter(func);
    ///     //手动设置全部引用过滤器
    ///     builder.SetReferencesFilter
    /// 
    /// </code>
    /// </example>
    /// </remarks>
    public Assembly GetAssembly()
    {
        _compilation ??= GetAvailableCompilation();

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
#if NETCOREAPP3_0_OR_GREATER
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, Domain.Name!);
#else
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, "Default");
#endif
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"{AssemblyName}.pdb");
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
           options: emitOption
           );
        LogCompilationEvent?.Invoke(_compilation.GetNatashaLog());
        Assembly? assembly = null;


#if NETCOREAPP3_0_OR_GREATER
        if (compileResult.Success)
        {

            if (_compilation.Options.OptimizationLevel == OptimizationLevel.Debug)
            {
                pdbStream?.Dispose();
            }
            dllStream.Seek(0, SeekOrigin.Begin);
            if (!_notLoadIntoDomain)
            {
                assembly = Domain.LoadAssemblyFromStream(dllStream, null);
                CompileSucceedEvent?.Invoke(_compilation, assembly!);
            }
        }
        dllStream.Dispose();
        pdbStream?.Dispose();
        xmlStream?.Dispose();
#else
        dllStream.Dispose();
        pdbStream?.Dispose();
        xmlStream?.Dispose();
        if (compileResult.Success)
        {
            assembly = Assembly.LoadFrom(DllFilePath);
            DefaultUsing.AddUsing(assembly);
            NatashaReferenceCache.AddReference(DllFilePath);
            CompileSucceedEvent?.Invoke(_compilation, assembly!);
        }
#endif
        if (!compileResult.Success)
        {
            CompileFailedEvent?.Invoke(_compilation, compileResult.Diagnostics);
            throw NatashaExceptionAnalyzer.GetCompileException(_compilation, compileResult.Diagnostics);
        }

#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[  Emit  ]", "编译时长", 2);
#endif
        return assembly!;
    }
}
