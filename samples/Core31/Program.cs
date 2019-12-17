using Natasha;
using System;
using System.Runtime.Loader;

namespace Core31
{
    class Program
    {
        static void Main(string[] args)
        {
            var domain = DomainManagment.Random;
            var type = NDomain.Create(domain).GetType("public class A{ public A(){Name=\"1\"; }public string Name;}");
            Console.WriteLine(type.FullName);
            var func = NDomain.Create(domain).Func<string>("return (new A()).Name;");
            Console.WriteLine(func());

            type.RemoveReferences();
            type = NDomain.Create(domain).GetType("public class A{ public A(){Name=\"2\"; }public string Name;}");
            func = NDomain.Create(domain).Func<string>("return (new A()).Name;");
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
