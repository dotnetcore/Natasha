using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha;
using Natasha.CSharp;
using System;

namespace Core20
{
    
    class Program
    {
        
        static void Main(string[] args)
        {

            NatashaInitializer.Initialize();
            //            string text = @"namespace HelloWorld
            //{
            //    public class Test
            //    {
            //        public Test(){
            //            Name=""111"";
            //        }

            //        public string Name;
            //        public int Age{get;set;}
            //    }
            //}";
            //            //根据脚本创建动态类
            //            AssemblyCSharpBuilder oop = new AssemblyCSharpBuilder("test");
            //            oop.Domain = DomainManagement.Random;
            //            oop.Domain.AddReferencesFromDllFile(typeof(object).Assembly.Location);
            //            oop.Add(text);
            //            Type type = oop.GetTypeFromShortName("Test");

            //            Console.WriteLine(type.Name);

            //            var action = NDelegate.UseDomain(oop.Domain).Action("");
            //            var a = action.Method;
            //            Console.WriteLine(action.Method.Module.Assembly);
            //            Console.WriteLine(DomainManagement.IsDeleted(action.GetDomain().Name));

            //var nClass = NClass
            //    .RandomDomain();
            //nClass.DelegateHandler.
            //type = NClass
            //    .RandomDomain().Namespace("")
            //    .Name("Test")
            //    .Ctor(item => item
            //        .Public()
            //        .Body("Name = null;")
            //    )
            //    .PublicField<string>("Name")
            //    .GetType();
            //type.Assembly
            //CSharpScript.EvaluateAsync
            Test test = new Test();
            Test2 test2 = new Test2();
            //test.CreateScript("return A;");
            //Console.WriteLine(test.GetValue());
            test.Show();
            var methods = typeof(Test2).GetMethods();
            var method = typeof(Test2).GetMethod("Get");
            var func = (Func<string>)method.CreateDelegate(typeof(Func<string>), test);
            func();
            //func(test2);
            Console.ReadKey();
        }


    }

}
