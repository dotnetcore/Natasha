using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyModel;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ReferenceTest50
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(typeof(string).Assembly == typeof(object).Assembly && typeof(string).Namespace == typeof(object).Namespace);
            Console.WriteLine(typeof(Console).Assembly == typeof(object).Assembly && typeof(Console).Namespace == typeof(object).Namespace);
            var assemblies = System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies;
            foreach (var item in assemblies)
            {
                Console.WriteLine(item.Location);
            }
            var names = new HashSet<string>(assemblies.Select(item => item.GetName().Name));
            Console.WriteLine(System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies.Count());
            IEnumerable<string> paths = DependencyContext
                    .Default
                    .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());
            foreach (var item in names)
            {
                Console.WriteLine(item);
            }
            foreach (var item in paths)
            {
                //Console.WriteLine(Path.GetFileNameWithoutExtension(item));
                if (names.Contains(Path.GetFileNameWithoutExtension(item)))
                {
                    Console.WriteLine(item);
                }
                System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(item);
            }

            //            NatashaInitializer.InitializeAndPreheating();
            //            string code = @"
            //using ReferenceTest50;
            // public class A {   
            //    /// <summary>
            //    ///  comment will raise a error.
            //    /// </summary>
            //    public string Name{get;set;}
            //    /// <summary>
            //    ///  comment will raise a error.
            //    /// </summary>
            //    public Test test;
            //}";
            Console.WriteLine(System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies.Count());
            //Show();
            //            //Show(code);


            //            Show2(code);
           
            Console.WriteLine("Completed!");
        }

        public static void Show()
        {
            NatashaInitializer.InitializeAndPreheating();
            DomainBase.DefaultDomain.AssemblyReferencesCache.Clear();
            var assemblies = System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies;
            foreach (var item in assemblies)
            {
                if (item.IsDynamic)
                {
                    Console.WriteLine($"{item.GetName().Name} is a Dynamic Assembly!");
                }
                else
                {
                    DomainBase.DefaultDomain.AddReferencesFromFileStream(item.Location);
                }
                
            }
            AssemblyCSharpBuilder assemblyCSharpBuilder = new AssemblyCSharpBuilder();
            assemblyCSharpBuilder.Add(@"public class A{  public async Task Show(){  }  }");
            var assembly = assemblyCSharpBuilder.GetAssembly();
            Console.WriteLine(assembly!=null);
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
            assemblyCSharpBuilder.SetPdbFilePath("2.pdb");
            assemblyCSharpBuilder.SetXmlFilePath("1.xml");
            var assembly = assemblyCSharpBuilder.GetAssembly();
            stopwatch.Stop();
            Console.WriteLine(assembly!=null);
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

        }
    }
}
