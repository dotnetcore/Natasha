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

            NatashaInitializer.Initialize(false);
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
            oop.Domain.AddReferencesFromDllFile(typeof(object).Assembly.Location);
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
            Console.ReadKey();
        }


    }

}
