using Microsoft.CodeAnalysis;
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
                        _domain = DomainManagment.Default;
                    }
#else 
                     _domain = DomainManagment.Default;
#endif

                }
                return _domain;

            }
            set
            {

                if (value == default)
                {
                    value = DomainManagment.Default;
                }
                _domain = value;

            }

        }




        public virtual bool PreCompiler()
        {
            return true;
        }
        public abstract T GetCompilation();
        public IEnumerable<SyntaxTree> CompileTrees;




        public event Action<string, string, T> FileCompileSucceedHandler;
        public event Action<Stream, T> StreamCompileSucceedHandler;
        public event Action<string, string, ImmutableArray<Diagnostic>, T> FileCompileFailedHandler;
        public event Action<Stream, ImmutableArray<Diagnostic>, T> StreamCompileFailedHandler;




        public virtual void CompileToFile(string dllPath, string pdbPath)
        {

            if (PreCompiler())
            {

                var compilation = GetCompilation();
                var CompileResult = compilation.Emit(dllPath, pdbPath);
                if (CompileResult.Success)
                {

                    Assembly = Domain.CompileFileHandler(dllPath, pdbPath, AssemblyName);
                    FileCompileSucceedHandler?.Invoke(dllPath, pdbPath, compilation);

                }
                else
                {

                    FileCompileFailedHandler?.Invoke(dllPath, pdbPath, CompileResult.Diagnostics, compilation);

                }

            }
            
        }
        public virtual void CompileToStream()
        {

            if (PreCompiler())
            {

                var compilation = GetCompilation();
                MemoryStream stream = new MemoryStream();
                var CompileResult = compilation.Emit(stream);
                if (CompileResult.Success)
                {

                    stream.Position = 0;
                    MemoryStream copyStream = new MemoryStream();
                    stream.CopyTo(copyStream);
                    stream.Position = 0;
                    Assembly = Domain.CompileStreamHandler(stream, AssemblyName);
                    copyStream.Position = 0;
                    StreamCompileSucceedHandler?.Invoke(stream, compilation);
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
                    CompileToFile(DllPath, PdbPath);
                    break;
                case AssemblyBuildKind.Stream:
                    CompileToStream();
                    break;

            }

        }
    }
}
