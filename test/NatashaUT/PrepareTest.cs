using Natasha;
using System;
using System.Collections.Generic;
using System.Text;

namespace NatashaUT
{
    public class PrepareTest
    {
        static PrepareTest()
        {
            ComponentRegister.RegistDomain<NatashaAssemblyDomain>();
            ComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
            ComponentRegister.RegisteSyntax<NatashaCSharpSyntax>();
        }
    }
}
