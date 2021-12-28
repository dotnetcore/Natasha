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
            
            Nacos.Request.RequestClient request = default;
            Nacos.Microsoft.Extensions.Configuration.ConfigListener b = default;
            Nacos.AspNetCore.V2.NacosAspNetOptions options = default;
            //Check();
            //Console.WriteLine(typeof(string).Assembly == typeof(object).Assembly && typeof(string).Namespace == typeof(object).Namespace);
            //Console.WriteLine(typeof(Console).Assembly == typeof(object).Assembly && typeof(Console).Namespace == typeof(object).Namespace);

            //var assemblyNames = DependencyContext.Default.GetDefaultAssemblyNames();
            //foreach (var item in assemblyNames)
            //{
            //    Console.WriteLine(item.Name);
            //}
            //Console.ReadKey();
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
            //Console.WriteLine("重新加载引用后,默认域程序集数:");
            //Console.WriteLine(System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies.Count());
            //Show();
            //            //Show(code);


            //            Show2(code);
           // Check();
            //Microsoft.CodeAnalysis.Workspaces.ErrorLogger
            Stopwatch stopwatch = new();
            stopwatch.Start();
            NatashaInitializer.InitializeAndPreheating();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
           // Check();

            //Check();
            //Console.WriteLine("=====================================");
            //var domain = DomainManagement.Random;
            ////domain.AddReferencesFromAssembly(typeof(object).Assembly);
            ////domain.AddReferencesFromAssembly(typeof(Console).Assembly);
            //NDelegate.UseDomain(domain).Action("Console.WriteLine(1);");
            ////NDelegate.RandomDomain(item=>item.DisableSemanticCheck()).Action("Console.WriteLine(1);");

            stopwatch.Restart();
            NDelegate.RandomDomain().Action("Serilog.Configuration.LoggerAuditSinkConfiguration loggerAuditSinkConfiguration = default;")();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
            //Check();
            stopwatch.Restart();
            NDelegate
                .RandomDomain(item=>item.DisableSemanticCheck())
                .SetMethod(item=>item.SkipInit().SkipInit())
                .Action("Serilog.Configuration.LoggerAuditSinkConfiguration loggerAuditSinkConfiguration = default;")();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
            //Check();
            Console.WriteLine("Completed!");
            Console.ReadKey();
        }

        public static void Check()
        {
            var assemblies = System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies;
            Console.WriteLine("默认域:");
            foreach (var item in assemblies)
            {
                if (!item.IsDynamic)
                {
                    Console.WriteLine(item.Location);
                }
                
            }
            Console.WriteLine(assemblies.Count());
            //var names = new HashSet<string>(assemblies.Select(item => item.GetName().Name));
            //Console.WriteLine("当前程序集:");
            //foreach (var item in names)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies.Count());
            //IEnumerable<string> paths = DependencyContext
            //        .Default
            //        .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());


            ////重复
            //Console.WriteLine("重复:");
            //foreach (var item in paths)
            //{

            //    //Console.WriteLine(Path.GetFileNameWithoutExtension(item));
            //    if (names.Contains(Path.GetFileNameWithoutExtension(item)))
            //    {
            //        Console.ForegroundColor = ConsoleColor.Green;
            //        Console.WriteLine(item);
            //        Console.ResetColor();
            //    }
            //    else
            //    {
            //        //Console.ForegroundColor = ConsoleColor.Yellow;
            //        //Console.WriteLine(item);
            //        //Console.ResetColor();
            //    }
            //}
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
