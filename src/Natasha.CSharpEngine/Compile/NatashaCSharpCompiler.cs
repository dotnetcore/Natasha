using Natasha.CSharpCompiler;
using Natasha.Error.Model;
using Natasha.Framework;

namespace Natasha.CSharpEngine.Compile
{
    public class NatashaCSharpCompiler : CSharpCompilerBase
    {

        public ExceptionBehavior ErrorBehavior;


        public NatashaCSharpCompiler()
        {

            AssemblyOutputKind = AssemblyBuildKind.Stream;
            ErrorBehavior = ExceptionBehavior.Throw;

        }

    }

}
