using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha.Framework
{
    public abstract class CompilerBase<TCompilation, TCompilationOptions> where TCompilation : Compilation where TCompilationOptions : CompilationOptions
    {
        public bool AllowUnsafe;
        public string AssemblyName;
        public string DllFilePath;
        public string PdbFilePath;
        public string XmlFilePath;
        public TCompilation? Compilation;
        public OutputKind AssemblyKind;
        public Platform ProcessorPlatform;
        //public AssemblyBuildKind AssemblyOutputKind;
        public OptimizationLevel CodeOptimizationLevel;
        public Action<TCompilationOptions>? OptionAction;
        public NullableContextOptions NullableCompileOption;
        public bool EnableSemanticHandle;
        public CompilerBase()
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
                if (DllFilePath!=string.Empty)
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

    }
}