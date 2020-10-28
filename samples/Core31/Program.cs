using Natasha.CSharp;
using System;
using System.Runtime.Loader;

namespace Core31
{
    class Program
    {
        static void Main(string[] args)
        {
            NatashaInitializer.InitializeAndPreheating();


            //var hwFunc = FastMethodOperator
            //    .RandomDomain()
            //    .Param(typeof(string), "str1")
            //    .Param<string>("str2")
            //    .Body("return str1+str2;")
            //    .Return<string>()
            //    .Compile<Func<string, string, string>>();
            //Console.WriteLine(hwFunc("Hello", " World!"));

            
            string temp = NDelegate.RandomDomain().Func<string>("return (new A()).Name;")();
            Console.WriteLine(temp);

            //var a123 = NClass.UseDomain(typeof(Program).GetDomain());
            //var domain = DomainManagement.Random;
            //var type = NDelegate.UseDomain(domain,item=>item.AssemblyName="a").GetType($"[assembly: AssemblyKeyFileAttribute(\"c:\\\\vs2019\\\\natasha.snk\")]" +"[assembly: AssemblyVersion(\"1.3.3.3\")]public class A{ public A(){Name=\"1\"; }public string Name;}");
            //var func = NDelegate.UseDomain(domain).Func<string>("return (new A()).Name;");
            //Console.WriteLine(type.FullName);
            //Console.WriteLine(func());

            ////type.RemoveReferences();
            //type = NDelegate.UseDomain(domain,item=>item.AssemblyName="a").GetType($"[assembly: AssemblyKeyFileAttribute(\"c:\\\\vs2019\\\\natasha.snk\")]" + "[assembly: AssemblyVersion(\"2.3.3.4\")]public class A{ public A(){Name=\"2\"; }public string Name;}");
            //func = NDelegate.UseDomain(domain).Func<string>("return (new A()).Name;");
            //Console.WriteLine(type.FullName);
            //Console.WriteLine(func());

            //domain = DomainManagement.Create("a");
            //using (DomainManagement.Lock("a"))
            //{
            //    Console.WriteLine(domain == (NatashaAssemblyDomain)AssemblyLoadContext.CurrentContextualReflectionContext);
            //}

            Console.ReadKey();
        }


    }

}
