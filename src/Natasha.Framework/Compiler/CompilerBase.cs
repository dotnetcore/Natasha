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
        public Assembly AssemblyResult;
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
        /// 重写方法，将编译信息编译到文件
        /// </summary>
        /// <param name="compilation"></param>
        /// <returns></returns>
        public abstract EmitResult CompileEmitToFile(TCompilation compilation);



        /// <summary>
        /// 重写方法，将编译信息编译到内存流
        /// </summary>
        /// <param name="compilation"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public abstract EmitResult CompileEmitToStream(TCompilation compilation, MemoryStream stream);


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
        /// 文件编译失败之后触发的事件
        /// </summary>
        public event Action<string, string, ImmutableArray<Diagnostic>, TCompilation> FileCompileFailedHandler;

        
        /// <summary>
        /// 流编译失败之后触发的事件
        /// </summary>
        public event Action<Stream, ImmutableArray<Diagnostic>, TCompilation> StreamCompileFailedHandler;

        
        /// <summary>
        /// 文件编译流程
        /// </summary>
        public virtual void CompileToFile()
        {

            if (PreCompiler())
            {

                var options = GetCompilationOptions();
                OptionAction?.Invoke(options);
                var compilation = GetCompilation(options);
                var CompileResult = CompileEmitToFile(compilation);

                if (CompileResult.Success)
                {
                    AssemblyResult = Domain.CompileFileHandler(DllPath, PdbPath, AssemblyName);
                    FileCompileSucceedHandler?.Invoke(DllPath, PdbPath, compilation);
                }
                else
                {
                    FileCompileFailedHandler?.Invoke(DllPath, PdbPath, CompileResult.Diagnostics, compilation);
                }

            }

        }


        /// <summary>
        /// 流编译流程
        /// </summary>
        public virtual void CompileToStream()
        {

            if (PreCompiler())
            {

                var options = GetCompilationOptions();
                OptionAction?.Invoke(options);
                var compilation = GetCompilation(options);
                MemoryStream stream = new MemoryStream();
                var CompileResult = CompileEmitToStream(compilation, stream);


                if (CompileResult.Success)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    MemoryStream copyStream = new MemoryStream();
                    stream.CopyTo(copyStream);
                    stream.Seek(0, SeekOrigin.Begin);
                    AssemblyResult = Domain.CompileStreamHandler(stream, AssemblyName);
                    StreamCompileSucceedHandler?.Invoke(copyStream, compilation);
                    copyStream.Dispose();
                }
                else
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    StreamCompileFailedHandler?.Invoke(stream, CompileResult.Diagnostics, compilation);
                }
                stream.Dispose();

            }

        }


        /// <summary>
        /// 编译入口，对语法树进行编译
        /// </summary>
        public virtual void Compile()
        {
            switch (AssemblyOutputKind)
            {
                case AssemblyBuildKind.File:
                    CompileToFile();
                    break;

                case AssemblyBuildKind.Stream:
                    CompileToStream();
                    break;
            }
        }
    }
}