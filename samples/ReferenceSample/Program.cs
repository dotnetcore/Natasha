using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Internal;
using System.Runtime.CompilerServices;

namespace ReferenceSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Internal.Console()

            Run();
            Console.ReadKey();
            
        }
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Run()
        {
            NatashaManagement.Preheating(true,true);
            Console.WriteLine(1);
        }
        public static void Run1()
        {
            AssemblyCSharpBuilder builder = new();
            builder.ConfigCompilerOption(item => item.AddSupperess("CS0108"));
            builder.WithoutAnalysisAccessibility();

            builder.Domain = DomainManagement.Random();


            builder.AddWithDefaultUsing("public class A { public string Name {get;set;} = \"abc\"; }");
            //此API可以在不用编译的情况下获取到已经格式化好的语法树.
            //注: GetAssembly 方法中已包括此方法, 但不会重复运行.
            builder.GetAvailableCompilation();
            var assemblyA = builder.GetAssembly();
            ShowAssemblyInfo(assemblyA);
            ShowSyntaxTree(builder.Compilation!);

            //清除编译信息,使 builder 可以重新组合 编译信息.
            builder.ClearCompilationCache();
            //builder.Domain = DomainManagement.Random();
            //重置程序集名,以免编译时产生冲突.
            builder.WithRandomAssenblyName();
            builder.ClearScript();
            //向 builder 中追加类型 B.
            builder.AddWithDefaultUsing("namespace TempA { public class A { public string Name {get;set;} = \"abcAaaa\"; }}");
            builder.AddWithDefaultUsing("public static class B{ public static void Show(){ Console.WriteLine(\"HelloWorld!\" + (new TempA.A()).Name); } }");
            builder.AddWithDefaultUsing("public class C: A { public string Name {get;set;} }");
            var assemblyAB = builder.GetAssembly();
            //Console.WriteLine(assemblyAB == assemblyA);
            //var a = assemblyAB.GetName().GetUniqueName();
            ShowAssemblyInfo(assemblyAB);
            ShowSyntaxTree(builder.Compilation!);

            var action = assemblyAB.GetDelegateFromShortName<Action>("B", "Show");
            Console.WriteLine("运行方法:");
            action();

            builder.Clear();
            builder.WithRandomAssenblyName();
            builder.AddWithDefaultUsing("public static class D{ public static object Show(){ return new A(); } }");
            var assemblyD = builder.GetAssembly();
            var func = assemblyD.GetDelegateFromShortName<Func<object>>("D", "Show");
            dynamic obj = func();
            Console.WriteLine(obj.Name);
            //action.DisposeDomain();
            //Console.WriteLine(DomainManagement.IsDeleted(builder.Domain!.Name!));
            static void ShowAssemblyInfo(Assembly assembly)
            {

                Console.WriteLine("此程序集名字:" + assembly.FullName);
                Console.WriteLine("此程序集是否为动态程序集:" + (assembly.IsDynamic ? "是" : "否"));
                Console.WriteLine("编译后的程序集中类的数量:" + assembly.GetTypes().Length);
                Console.WriteLine("程序集中的类分别是:" + string.Join(",", assembly.GetTypes().Select(item => item.Name)));

            }

            static void ShowSyntaxTree(Compilation compilation)
            {
                Console.WriteLine("-----------脚本代码-----------");
                foreach (var item in compilation.SyntaxTrees)
                {

                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine("-----------------------------");
            }
        }
    }
}