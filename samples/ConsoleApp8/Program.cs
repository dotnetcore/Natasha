using Natasha;
using Natasha.CSharp;
using System;
using System.IO;

namespace ConsoleApp8
{
    class Program
    {
        static void Main(string[] args)
        {
            //string p1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "P1", "MyPlugin1.dll");
            //string rp1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RP1", "MyPlugin1.dll");
            //string p2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "P2", "MyPlugin2.dll");
            //DomainManagement.RegisterDefault<AssemblyDomain>();
            //var domain = DomainManagement.Random;
            ////domain.LoadPluginFromStream(p1);
            ////domain.Remove(p1);
            //domain.LoadPluginFromStream(p2);
            //domain.Remove(p2);
            //domain.LoadPluginFromStream(rp1);
            //domain.Remove(rp1);
            //domain.LoadPluginFromStream(p2);
            //domain.Remove(p2);
            //domain.LoadPluginFromStream(p1);
            //var action = NDelegate.UseDomain(domain).Action("Console.WriteLine((new Plugin()).Name);", "MyPlugin1");
            //action();

            DomainManagement.RegisterDefault<AssemblyDomain>();
            AssemblyCSharpBuilder sharpBuilder = new AssemblyCSharpBuilder();
            sharpBuilder.Compiler.Domain = DomainManagement.Random;
            sharpBuilder.UseFileCompile();
            sharpBuilder.ThrowAndLogCompilerError();
            sharpBuilder.ThrowAndLogSyntaxError();
            sharpBuilder.Syntax.Add("using System; public static class Test{ public static void Show(){ Console.WriteLine(\"Hello world!\");}}");
            var assembly = sharpBuilder.GetAssembly();

            //sharpBuilder.
            var action = NDelegate.UseDomain(sharpBuilder.Compiler.Domain).Action("Test.Show();", assembly);
            action();
            Console.ReadKey();

        }
    }
}
