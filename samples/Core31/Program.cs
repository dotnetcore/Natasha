using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Scripting;
using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;

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



            //string temp = NDelegate.RandomDomain().Func<string>("return (new A()).Name;")();
            //Console.WriteLine(temp);

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
            //            var temp1 = new Test (){ X = 1, Y = 2 };
            //            Console.WriteLine(CSharpScript.EvaluateAsync<int>(@"
            //var result = X+Y;
            //result+=""X"".Length;
            //result+=""Y"".Length;
            //result+=""XY"".Length;
            //X = X * Y;
            //result+=X;
            //result+=X.Y;
            //return result;
            //", 

            //ScriptOptions
            //.Default
            //.AddReferences(typeof(Test).Assembly)
            //.WithImports("Core31"),
            //globals: temp1).Result);

            var func = NDelegate
                .RandomDomain()
                .WithFirstArgInvisible()
                .Func<Test,int>(@"
            arg.Show();
            //var a = Show();
            var b = c;
            Show(c);
            Console.WriteLine(1); 
            return 0;");




            NDelegate
               .RandomDomain()
               .WithFirstArgInvisible("arg")
               .Func<Test, int>(@"
            arg.Show();
            //var a = Show();
            var b = c;
            Show(c);
            Console.WriteLine(1); 
            return 0;")(new Test());

            Console.ReadKey();
        }

       

    }

}
