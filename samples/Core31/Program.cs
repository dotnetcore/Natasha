using Natasha;
using Natasha.CSharp;
using System;
using System.Runtime.Loader;

namespace Core31
{
    class Program
    {
        static void Main(string[] args)
        {

            AssemblyDomain.Init();

            var hwFunc = FastMethodOperator
                .RandomDomain()
                .Param(typeof(string), "str1")
                .Param<string>("str2")
                .Body("return str1+str2;")
                .Return<string>()
                .Compile<Func<string, string, string>>();
            Console.WriteLine(hwFunc("Hello"," World!"));
                

            var a123 = NClass.UseDomain(typeof(Program).GetDomain());
            var domain = DomainManagement.Random;
            var type = NDelegate.UseDomain(domain).GetType("public class A{ public A(){Name=\"1\"; }public string Name;}");
            var func = NDelegate.UseDomain(domain).Func<string>("return (new A()).Name;");
            Console.WriteLine(type.FullName);
            Console.WriteLine(func());

            //type.RemoveReferences();
            type = NDelegate.UseDomain(domain).GetType("public class A{ public A(){Name=\"2\"; }public string Name;}");
            func = NDelegate.UseDomain(domain).Func<string>("return (new A()).Name;");
            Console.WriteLine(type.FullName);
            Console.WriteLine(func());

            domain = DomainManagement.Create("a");
            using (DomainManagement.Lock("a"))
            {
                Console.WriteLine(domain == (AssemblyDomain)AssemblyLoadContext.CurrentContextualReflectionContext);
            }
        }
    }
}
