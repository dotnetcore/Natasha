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
        public Assembly Assembly;
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
                    _domain = (DomainBase)(DomainBase.Default);
#endif
                }
                return _domain;
            }
            set
            {
                if (value == default)
                {
                    value = (DomainBase)(DomainBase.Default);
                }
                _domain = value;
            }
        }

        public void AddOption(Action<TCompilationOptions> action)
        {
            OptionAction += action;
        }

        public virtual bool PreCompiler()
        {
            return true;
        }

        public abstract TCompilation GetCompilation();
        public abstract EmitResult EmitToFile(TCompilation compilation);
        public abstract EmitResult EmitToStream(TCompilation compilation, MemoryStream stream);

        public IEnumerable<SyntaxTree> CompileTrees;

        public event Action<string, string, TCompilation> FileCompileSucceedHandler;

        public event Action<Stream, TCompilation> StreamCompileSucceedHandler;

        public event Action<string, string, ImmutableArray<Diagnostic>, TCompilation> FileCompileFailedHandler;

        public event Action<Stream, ImmutableArray<Diagnostic>, TCompilation> StreamCompileFailedHandler;

        public virtual void CompileToFile()
        {

            if (PreCompiler())
            {

                var compilation = GetCompilation();
                var CompileResult = EmitToFile(compilation);

                if (CompileResult.Success)
                {
                    Assembly = Domain.CompileFileHandler(DllPath, PdbPath, AssemblyName);
                    FileCompileSucceedHandler?.Invoke(DllPath, PdbPath, compilation);
                }
                else
                {
                    FileCompileFailedHandler?.Invoke(DllPath, PdbPath, CompileResult.Diagnostics, compilation);
                }

            }

        }

        public virtual void CompileToStream()
        {

            if (PreCompiler())
            {

                var compilation = GetCompilation();
                MemoryStream stream = new MemoryStream();
                var CompileResult = EmitToStream(compilation, stream);


                if (CompileResult.Success)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    MemoryStream copyStream = new MemoryStream();
                    stream.CopyTo(copyStream);
                    stream.Seek(0, SeekOrigin.Begin);
                    Assembly = Domain.CompileStreamHandler(stream, AssemblyName);
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
        /// 对语法树进行编译
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