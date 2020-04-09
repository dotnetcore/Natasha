using Natasha.CSharpComplier;
using Natasha.Error;
using Natasha.Error.Model;
using Natasha.Framework;
using System.Collections.Generic;

namespace Natasha.CSharpEngine.Compile
{
    public class NatashaCompiler : CSharpCompilerBase
    {

        public List<CompilationException> Exceptions;


        public int RetryLimit;
        private int _retryCount;
        internal bool CanRetry;


        public AssemblyBuildKind AssemblyOutputKind;
        public ExceptionBehavior ErrorBehavior;
        public string DllPath;
        public string PdbPath;


        public NatashaCompiler()
        {

            AssemblyOutputKind = AssemblyBuildKind.Stream;
            ErrorBehavior = ExceptionBehavior.Throw;

        }




        /// <summary>
        /// 对语法树进行编译
        /// </summary>
        public virtual void Compile()
        {

            Exceptions = null;
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
