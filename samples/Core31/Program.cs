using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Scripting;
using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace Core31
{
    class Program
    {
        static void Main(string[] args)
        {
            //NatashaInitializer.Initialize();
            NatashaInitializer.InitializeAndPreheating();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            NSucceedLog.Enabled = true;
            NDelegate.RandomDomain(opt => opt.UseNatashaFileOut()).Action("Console.WriteLine(\"Hello World!\");")();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            var action = NDelegate.DefaultDomain().Action("Console.WriteLine(1);");
            action.DisposeDomain();

            //NullabilityInfoContext context = new();

            //NullabilityInfo arrayInfo = NullabilityInfo.Create(,);
            //Console.WriteLine(arrayInfo.ReadState);        // NotNull
            //Console.WriteLine(arrayInfo.Element.State);    // Nullable

            //NullabilityInfo tupleInfo = context.Create(tupleField);
            //Console.WriteLine(tupleInfo.ReadState);                      // NotNull
            //Console.WriteLine(tupleInfo.GenericTypeArguments[0].State); // Nullable
            //Console.WriteLine(tupleInfo.GenericTypeArguments[1].State); // NotNull
            //var hwFunc = FastMethodOperator
            //    .RandomDomain()
            //    .Param(typeof(string), "str1")
            //    .Param<string>("str2")
            //    .Body("return str1+str2;")
            //    .Return<string>()
            //    .Compile<Func<string, string, string>>();
            //Console.WriteLine(hwFunc("Hello", " World!"));


            //var assembly = typeof(object).Assembly;
            //var path = assembly.Location;
            //var r1 = MetadataReference.CreateFromFile(path);
            //MetadataReference.CreateFromAssembly
            //MetadataReference.CreateFromStream(assembly.strea);

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

            //var func = NDelegate
            //    .RandomDomain()
            //    .WithFirstArgInvisible()
            //    .Func<Test,int>(@"
            //var b = c;
            //Show(b);
            ////var s = new Test1();
            //Task.Run(()=>{ Show();  });
            //return 0;")(new Test());




            //Console.WriteLine(1.WithScript("return arg+1;").Execute<double>());

            //Test instance = new Test();
            //instance.WithScript("Show();").Execute();

            //NDelegate
            //   .RandomDomain()
            //   .WithFirstArgInvisible("arg")
            //   .Func<Test, int>(@"
            //arg.Show();
            ////var a = Show();
            //var b = c;
            //Show(c);
            //Console.WriteLine(1); 
            //return 0;")(new Test());

            Console.ReadKey();
        }

       

    }

}
