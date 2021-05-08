using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Natasha.Framework
{
    public abstract class CompilerBase<TCompilation, TCompilationOptions> where TCompilation : Compilation where TCompilationOptions : CompilationOptions
    {

        public string AssemblyName;
        public AssemblyBuildKind AssemblyOutputKind;
        public string DllPath;
        public string PdbPath;
        public bool AllowUnsafe;
        public OutputKind Enum_OutputKind;
        public Platform Enum_Platform;
        public OptimizationLevel Enum_OptimizationLevel;
        public Action<TCompilationOptions> OptionAction;
        public TCompilation Compilation;

        public CompilerBase()
        {
            _domain = null;
        }

        private DomainBase _domain;
        public DomainBase Domain
        {
            get
            {
                if (_domain == null)
                {
#if !NETSTANDARD2_0
                    if (AssemblyLoadContext.CurrentContextualReflectionContext != default)
                    {
                        _domain = (DomainBase)(AssemblyLoadContext.CurrentContextualReflectionContext);
                    }
                    else
                    {
                        _domain = DomainBase.DefaultDomain;
                    }
#else
                    _domain = DomainBase.DefaultDomain;
#endif
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


        /// <summary>
        /// 获取构建编译信息的选项
        /// </summary>
        /// <returns></returns>
        public abstract TCompilation GetCompilation(TCompilationOptions options);


        /// <summary>
        /// 获取语法树
        /// </summary>
        public IEnumerable<SyntaxTree> CompileTrees;


        /// <summary>
        /// 文件编译成功之后触发的事件
        /// </summary>
        public event Action<string, string, TCompilation> FileCompileSucceedHandler;


        /// <summary>
        /// 流编译成功之后触发的事件
        /// </summary>
        public event Action<Stream, TCompilation> StreamCompileSucceedHandler;


        /// <summary>
        /// 流编译失败之后触发的事件
        /// </summary>
        public event Func<Stream, ImmutableArray<Diagnostic>, TCompilation, Assembly> CompileFailedHandler;



        public virtual Assembly ComplieToAssembly()
        {
            Assembly assembly = null;
            EmitResult compileResult = null;
            if (PreCompiler())
            {

                var options = GetCompilationOptions();
                OptionAction?.Invoke(options);
                var compilation = GetCompilation(options);
                Stream outputStream = new MemoryStream();

                if (AssemblyOutputKind == AssemblyBuildKind.File)
                {

                    outputStream = File.Create(DllPath);
                    using (var pdbStream = File.Create(PdbPath))
                    {
                        compileResult = compilation.Emit(
                       outputStream,
                       pdbStream: pdbStream,
                       options: new EmitOptions(pdbFilePath: PdbPath, debugInformationFormat: DebugInformationFormat.PortablePdb));

                    }

                }
                else
                {
                    compileResult = compilation.Emit(
                            outputStream,
                            options: new EmitOptions(false, debugInformationFormat: DebugInformationFormat.PortablePdb));
                }

                if (compileResult.Success)
                {


                    outputStream.Seek(0, SeekOrigin.Begin);
                    MemoryStream copyStream = new MemoryStream();
                    outputStream.CopyTo(copyStream);


                    outputStream.Seek(0, SeekOrigin.Begin);
                    assembly = Domain.CompileStreamCallback(outputStream, AssemblyName);

                    if (AssemblyOutputKind == AssemblyBuildKind.File)
                    {
                        FileCompileSucceedHandler?.Invoke(DllPath, PdbPath, compilation);
                    }
                    else
                    {
                        StreamCompileSucceedHandler?.Invoke(copyStream, compilation);
                    }
                    copyStream.Dispose();

                }
                else
                {

                    assembly = CompileFailedHandler?.Invoke(outputStream, compileResult.Diagnostics, compilation);

                }
                outputStream.Dispose();

            }
            return assembly;
        }

    }
}