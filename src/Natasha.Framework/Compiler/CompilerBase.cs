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
    public abstract class CompilerBase<T> where T : Compilation
    {
        public string AssemblyName;
        public Assembly Assembly;
        public AssemblyBuildKind AssemblyOutputKind;
        public string DllPath;
        public string PdbPath;

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
                        _domain = DomainManagement.Default;
                    }
#else
                    _domain = DomainManagement.Default;
#endif
                }
                return _domain;
            }
            set
            {
                if (value == default)
                {
                    value = DomainManagement.Default;
                }
                _domain = value;
            }
        }

        public virtual bool PreCompiler()
        {
            return true;
        }

        public abstract T GetCompilation();
        public virtual EmitResult EmitToFile(T compilation)
        {

            EmitResult CompileResult;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                CompileResult = compilation.Emit(DllPath, PdbPath);
            }
            else
            {
                CompileResult = compilation.UnixEmit(DllPath, PdbPath);
            }
            return CompileResult;

        }
        public virtual EmitResult EmitToStream(T compilation, MemoryStream stream)
        {
            EmitResult CompileResult;
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                CompileResult = compilation.Emit(stream);
            }
            else
            {
                CompileResult = compilation.Emit(stream, options: new EmitOptions(false, DebugInformationFormat.PortablePdb));
            }
            return CompileResult;
        }

        public IEnumerable<SyntaxTree> CompileTrees;

        public event Action<string, string, T> FileCompileSucceedHandler;

        public event Action<Stream, T> StreamCompileSucceedHandler;

        public event Action<string, string, ImmutableArray<Diagnostic>, T> FileCompileFailedHandler;

        public event Action<Stream, ImmutableArray<Diagnostic>, T> StreamCompileFailedHandler;

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
                    stream.Position = 0;
                    MemoryStream copyStream = new MemoryStream();
                    stream.CopyTo(copyStream);
                    stream.Position = 0;
                    Assembly = Domain.CompileStreamHandler(stream, AssemblyName);
                    StreamCompileSucceedHandler?.Invoke(copyStream, compilation);
                    copyStream.Dispose();
                }
                else
                {
                    stream.Position = 0;
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