using Natasha;
using System;
using System.Runtime.Loader;

namespace Core31
{
    class Program
    {
        static void Main(string[] args)
        {

            var a123 = NClass.Use(typeof(Program).GetDomain());

            var domain = DomainManagment.Random;
            var type = NDelegate.Use(domain).GetType("public class A{ public A(){Name=\"1\"; }public string Name;}");
            Console.WriteLine(type.FullName);
            var func = NDelegate.Use(domain).Func<string>("return (new A()).Name;");
            Console.WriteLine(func());

            type.RemoveReferences();
            type = NDelegate.Use(domain).GetType("public class A{ public A(){Name=\"2\"; }public string Name;}");
            func = NDelegate.Use(domain).Func<string>("return (new A()).Name;");
            Console.WriteLine(type.FullName);
            Console.WriteLine(func());

            domain = DomainManagment.Create("a");
            using (DomainManagment.Lock("a"))
            {
                Console.WriteLine(domain == (AssemblyDomain)AssemblyLoadContext.CurrentContextualReflectionContext);
            }
        }
    }
}
