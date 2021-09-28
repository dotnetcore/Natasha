using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Natasha;
using Natasha.CSharp;
using System;
using System.Diagnostics;

namespace Core20
{
    
    class Program
    {
        
        static void Main(string[] args)
        {
//            string code = @"public class A{
//    public string A{get;set;}
//public string B{get;set;}

//public string C{get;set;}

//public void  Test( ){NormalTestModel result = new NormalTestModel();int i=0 ; var t=new{Name=""""};return result;Console.WriteLine( ""aa"");}
//}";
//            var _options = new CSharpParseOptions(LanguageVersion.Preview);
//            var tree = SyntaxFactory.ParseSyntaxTree(code.Trim(), _options).GetRoot().NormalizeWhitespace().SyntaxTree;
//            using (var workspace = new AdhocWorkspace())
//            {
//                tree = Formatter.Format(tree.GetRoot(), workspace).SyntaxTree;
//            }
//            Console.WriteLine(tree.ToString());
//            Console.WriteLine();
            NatashaInitializer.InitializeAndPreheating();
            //var func = NDelegate.RandomDomain().Func<int>("return 1;");
            //Console.WriteLine(func());
            /*
            NatashaInitializer.Initialize();
            string text = @"namespace HelloWorld
            {
                public class Test
                {
                    public Test(){
                        Name=""111"";
                    }

                    public string Name;
                    public int Age{get;set;}
                }
            }";
            //根据脚本创建动态类
            AssemblyCSharpBuilder oop = new AssemblyCSharpBuilder("test");
            oop.Domain = DomainManagement.Random;
            //oop.Domain.AddReferencesFromDllFile(typeof(object).Assembly.Location);
            oop.Add(text);
            Type type = oop.GetTypeFromShortName("Test");

            Console.WriteLine(type.Name);

            var action = NDelegate.UseDomain(oop.Domain).Action("");
            var a = action.Method;
            Console.WriteLine(action.Method.Module.Assembly);
            Console.WriteLine(DomainManagement.IsDeleted(action.GetDomain().Name));

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
            */
            Console.ReadKey();
        }


    }

}
