using Natasha.CSharp;
using System;
using System.Runtime.CompilerServices;

namespace NET50
{
    class Program
    {
        static void Main(string[] args)
        {
            NatashaInitializer.InitializeAndPreheating();
            Console.WriteLine(Test.Name);
            //NDelegate.RandomDomain().Action("Console.WriteLine(\"Hello World!\");")();
            var type = NClass
                .DefaultDomain()
                .SetFlag("assembly:TypeForwardedTo(typeof(NET50.Test))")
                .Property(item=>item.Static().Name("Name").Type<string>().Getter().Setter())
                .Ctor(item => item.Static().NoUseAccess().Body("Name=\"new\";"))
                .Method(item=>item.Public().Name("Show").Body("Console.WriteLine(\"new\");"))
                .GetType();
            Console.WriteLine(type);
            Console.WriteLine(Test.Name);

            var test = new Test();
            test.Show();
            Console.ReadKey();
        }
    }
}
