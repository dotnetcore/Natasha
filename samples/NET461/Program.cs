using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET461
{
    class Program
    {
        static void Main(string[] args)
        {
            NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>();
            NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
            NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();

            var action = NDelegate.RandomDomain().Action("Console.WriteLine(\"Hello world!\")");
            action();

            Console.ReadKey();
        }
    }
}
