using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Compiler;
using Natasha.CSharp.Component.Domain;
using Natasha.CSharp.Core;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

/// <summary>
/// 程序集编译构建器 - 编译选项
/// </summary>
public partial class AssemblyCSharpBuilder 
{

    private LoadBehaviorEnum _compileReferenceBehavior;
    private Func<AssemblyName, AssemblyName, LoadVersionResultEnum>? _referencePickFunc;

    public AssemblyCSharpBuilder SetReferencesLoadBehavior(LoadBehaviorEnum loadBehavior)
    {
        _compileReferenceBehavior = loadBehavior;
        return this;
    }

    public AssemblyCSharpBuilder SetReferencesFilter(Func<AssemblyName, AssemblyName, LoadVersionResultEnum>? useAssemblyNameFunc = null)
    {
        _referencePickFunc = useAssemblyNameFunc;
        return this;
    }

    /// <summary>
    /// 流编译成功之后触发的事件
    /// </summary>
    public event Action<CSharpCompilation, Assembly>? CompileSucceedEvent;


    /// <summary>
    /// 流编译失败之后触发的事件
    /// </summary>
    public event Action<CSharpCompilation, ImmutableArray<Diagnostic>>? CompileFailedEvent;
    /// <summary>
    /// 将语法树生成到程序集
    /// </summary>
    /// <param name="trees"></param>
    /// <returns></returns>
    public Assembly GetAssembly()
    {
#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif

        //Mark : 26ms
        if (_compileReferenceBehavior == LoadBehaviorEnum.None)
        {
            _compilerOptions.SetSupersedeLowerVersions(true);
        }


        var options = _compilerOptions.GetCompilationOptions();
        IEnumerable<PortableExecutableReference> references;
        if (Domain.Name == "Default")
        {
            references = NatashaDomain.DefaultDomain._referenceCache.GetReferences();
        }
        else
        {
            references = Domain._referenceCache.CombineReferences(NatashaDomain.DefaultDomain._referenceCache, _compileReferenceBehavior, _referencePickFunc);
        }
        var compilation = CSharpCompilation.Create(AssemblyName, SyntaxTrees, references, options);

#if DEBUG
        Console.WriteLine();
        stopwatch.StopAndShowCategoreInfo("[Compilation]", "获取编译单元", 2);
        stopwatch.Restart();
#endif

        //Mark : 951ms
        //Mark : 19M Memory
        //Todo
        //        Compilation = (TCompilation)Compilation.AddSyntaxTrees(trees);
        //        if (EnableSemanticHandle)
        //        {
        //            foreach (var item in _semanticAnalysistor)
        //            {
        //                Compilation = item(Compilation);
        //            }
        //#if DEBUG
        //            stopwatch.StopAndShowCategoreInfo("[Semantic]", "语义解析排查", 2);
        //            stopwatch.Restart();
        //#endif
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

        var compileResult = compilation.Emit(
           dllStream,
           pdbStream: pdbStream,
           xmlDocumentationStream: xmlStream,
           options: new EmitOptions(pdbFilePath: PdbFilePath == string.Empty ? null : PdbFilePath, debugInformationFormat: DebugInformationFormat.PortablePdb));


        Assembly? assembly = null;
        if (compileResult.Success)
        {
            dllStream.Seek(0, SeekOrigin.Begin);
            pdbStream?.Seek(0, SeekOrigin.Begin);
            assembly = Domain.LoadFromStream(dllStream, pdbStream);
            CompileSucceedEvent?.Invoke(compilation, assembly!);
        }
        dllStream.Dispose();
        pdbStream?.Dispose();
        xmlStream?.Dispose();

        if (!compileResult.Success)
        {
            CompileFailedEvent?.Invoke(compilation, compileResult.Diagnostics);
            throw NatashaExceptionAnalyzer.GetCompileException(compilation, compileResult.Diagnostics);
        }

#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[  Emit  ]", "编译时长", 2);
#endif

        return assembly!;
    }

}



