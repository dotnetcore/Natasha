#if !NETCOREAPP3_0_OR_GREATER
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Component;
using Natasha.CSharp.Component.Exception;
using Natasha.CSharp.Extension.Inner;
using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Reflection;

/// <summary>
/// 程序集编译构建器 - 编译选项
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{

    /// <summary>
    /// 流编译成功之后触发的事件
    /// </summary>
    public event Action<CSharpCompilation, Assembly>? CompileSucceedEvent;


    /// <summary>
    /// 流编译失败之后触发的事件
    /// </summary>
    public event Action<CSharpCompilation, ImmutableArray<Diagnostic>>? CompileFailedEvent;


    public CSharpCompilation GetAvailableCompilation()
    {
#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif

        var options = _compilerOptions.GetCompilationOptions(_codeOptimizationLevel);
        var references = NatashaReferenceCache.GetReferences();
        _compilation = CSharpCompilation.Create(AssemblyName, SyntaxTrees, references, options);

#if DEBUG
        stopwatch.RestartAndShowCategoreInfo("[Compiler]", "获取编译单元", 2);
#endif


        if (EnableSemanticHandler)
        {
            foreach (var item in _semanticAnalysistor)
            {
                _compilation = item(this, _compilation, _semanticCheckIgnoreAccessibility);
            }
        }

#if DEBUG
        stopwatch.RestartAndShowCategoreInfo("[Semantic]", "语义处理", 2);
#endif
        return _compilation;
    }


    /// <summary>
    /// 将 SyntaxTrees 中的语法树编译到程序集.如果不成功会抛出 NatashaException.
    /// </summary>
    /// <remarks>
    /// <example>
    /// <code>
    /// 
    ///     //程序集的域加载行为, 该行为决定了编译后的程序集随着依赖加载到域中的处理结果.
    ///     //和加载插件原理相同.
    ///     builder.CompileWithAssemblyLoadBehavior(enum);
    ///     
    ///     //编译单元的引用加载行为, 遇到同名不同版本的引用该如何处理.
    ///     builder.CompileWithReferenceLoadBehavior(enum);
    ///     builder.CompileWithReferencesFilter(func);
    /// 
    /// </code>
    /// </example>
    /// </remarks>
    public Assembly GetAssembly()
    {

        if (_compilation == null)
        {
            _compilation = GetAvailableCompilation();
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

        var debugInfoFormat = _debugInfo._informationFormat;
        if (_compilation.Options.OptimizationLevel == OptimizationLevel.Debug)
        {

            if (debugInfoFormat == DebugInformationFormat.PortablePdb)
            {
                if (string.IsNullOrEmpty(PdbFilePath))
                {
                    var tempPdbOutputFolder = Path.Combine(GlobalOutputFolder, "Default");
                    PdbFilePath = Path.Combine(tempPdbOutputFolder, $"{AssemblyName}.pdb");
                    if (!Directory.Exists(tempPdbOutputFolder))
                    {
                        Directory.CreateDirectory(tempPdbOutputFolder);
                    }
                }
                pdbStream = File.Create(PdbFilePath);
            }
            else if (debugInfoFormat == DebugInformationFormat.Embedded)
            {
                PdbFilePath = null;
            }
        }
        else if (!string.IsNullOrEmpty(PdbFilePath))
        {
            PdbFilePath = null;
            debugInfoFormat = 0;
        }

        if (XmlFilePath != string.Empty)
        {
            xmlStream = File.Create(XmlFilePath);
        }

        var compileResult = _compilation.Emit(
            dllStream,
            pdbStream: pdbStream,
            xmlDocumentationStream: xmlStream,
            options: new EmitOptions(
                includePrivateMembers: _includePrivateMembers,
                metadataOnly: _isReferenceAssembly,
                pdbFilePath: PdbFilePath,
                debugInformationFormat: debugInfoFormat
                )
            );


        LogCompilationEvent?.Invoke(_compilation.GetNatashaLog());

        dllStream.Dispose();
        pdbStream?.Dispose();
        xmlStream?.Dispose();

        Assembly? assembly = null;
        if (compileResult.Success)
        {
            assembly = Assembly.LoadFrom(DllFilePath);
            DefaultUsing.AddUsing(assembly);
            NatashaReferenceCache.AddReference(DllFilePath);
            CompileSucceedEvent?.Invoke(_compilation, assembly!);
        }

#if DEBUG 
        stopwatch.StopAndShowCategoreInfo("[  Emit  ]", "编译时长", 2);
#endif

        if (!compileResult.Success)
        {
            CompileFailedEvent?.Invoke(_compilation, compileResult.Diagnostics);
            throw NatashaExceptionAnalyzer.GetCompileException(_compilation, compileResult.Diagnostics);
        }

        return assembly!;
    }

}

#endif

