using Natasha.CSharpComplier;
using Natasha.Error.Model;
using Natasha.Framework;

namespace Natasha.CSharpEngine.Compile
{
    public class NatashaCSharpCompiler : CSharpCompilerBase
    {

        public AssemblyBuildKind AssemblyOutputKind;
        public ExceptionBehavior ErrorBehavior;
        public string DllPath;
        public string PdbPath;


        public NatashaCSharpCompiler()
        {

            AssemblyOutputKind = AssemblyBuildKind.Stream;
            ErrorBehavior = ExceptionBehavior.Throw;

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
