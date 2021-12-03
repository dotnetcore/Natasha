using Microsoft.CodeAnalysis.CSharp;
using Natasha.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace ReferenceTest50
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NatashaInitializer.InitializeAndPreheating();
            string code = @"
using ReferenceTest50;
 public class A {   
    /// <summary>
    ///  comment will raise a error.
    /// </summary>
    public string Name{get;set;}
    /// <summary>
    ///  comment will raise a error.
    /// </summary>
    public Test test;
}";

           
            //Show(code);
            
            
            Show2(code);
           
        }


        public static void Show(string code)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            NatashaCSharpCompiler natashaCSharpCompiler = new NatashaCSharpCompiler();
            var options = natashaCSharpCompiler.GetCompilationOptions();
            //NatashaInitializer.InitializeAndPreheating();
            var compilation = CSharpCompilation.Create("test", null, DomainBase.DefaultDomain.GetCompileReferences(), options);
            var tree = CSharpSyntaxTree.ParseText(code);
            compilation = compilation.AddSyntaxTrees(tree);
           // var result = compilation.Emit("1.dll", "1.pdb", "1.xml");

            Stream outputStream = new MemoryStream();
            outputStream = File.Create("1.dll");
            using (var xmlStream = File.Create("1.xml"))
            {
                using (var pdbStream = File.Create("1.pdb"))
                {
                   var result = compilation.Emit(
                   outputStream,
                   pdbStream: pdbStream,
                   xmlDocumentationStream: xmlStream
                   );
                    Console.WriteLine(result.Success);
                }
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
        public static void Show2(string code)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            AssemblyCSharpBuilder assemblyCSharpBuilder = new AssemblyCSharpBuilder();
            assemblyCSharpBuilder.Domain = DomainManagement.Random;
            assemblyCSharpBuilder.Add(code);
            assemblyCSharpBuilder.UseNatashaFileOut();
            var assembly = assemblyCSharpBuilder.GetAssembly();
            stopwatch.Stop();
            Console.WriteLine(assembly!=null);
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

        }
    }
}
