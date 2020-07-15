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
            NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>();
            NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
            NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();
        }
    }
}
