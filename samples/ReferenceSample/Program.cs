using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Internal;
using System.Runtime.CompilerServices;
using static System.Diagnostics.DebuggableAttribute;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;

namespace ReferenceSample
{

    internal class Program
    {
        
        static void Main(string[] args)
        {
            //Run();
            //NatashaManagement.Preheating(true, true);
            //GC.Collect();
            //Thread.Sleep(15000);
            var method = typeof(Program).GetMethod("TestMini");
            for (int i = 0; i < 5; i++)
            {
                method.Invoke(null,null);
                Thread.Sleep(3000);
            }
            Console.ReadKey();
            
        }

        public static void TestMini()
        {
            
            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder();
            builder
                .UseRandomDomain()
                .ConfigCompilerOption(opt=>opt
                        .AppendCompilerFlag(
                        Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressConstraintChecks | 
                        Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressObsoleteChecks | 
                        Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressTypeArgumentBinding | 
                        Natasha.CSharp.Compiler.CompilerBinderFlags.SuppressUnsafeDiagnostics)
                        )
                .DisableSemanticCheck()
                .WithDebugCompile(item=>item.WriteToFile())
                .OutputAsFullAssembly()
                .WithoutPrivateMembers()
                .AddReference(typeof(DebuggableAttribute))
                .AddReference(typeof(object).Assembly)
                .AddReference(typeof(Math).Assembly)
                .AddReference(typeof(MathF).Assembly)
                .WithoutCombineReferences();

            builder.Add(@"
public static class A{  
    public static int N1 = 10;
    public static float N2 = 1.2F; 
    public static double N3 = 3.44;
    private static short N4 = 0;
    public static object Invoke(){

        return N1 + MathF.Log10((float)Math.Sqrt(MathF.Sqrt(N2) + Math.Tan(N3)));
    }
}", UsingLoadBehavior.WithCurrent);
            
            var asm = builder.GetAssembly();
            var type = asm.GetType("A");
            var method = type.GetMethod("Invoke");
            var result = method.Invoke(null, null);
            Console.WriteLine(result);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Run()
        {
            NatashaManagement.Preheating(false, false);
            Console.WriteLine("Completed!");
        }
        public static void Run1()
        {
            AssemblyCSharpBuilder builder = new();
            builder.ConfigCompilerOption(item => item.AddSupperess("CS0108"));
            builder.WithoutAnalysisAccessibility();

            builder.Domain = DomainManagement.Random();


            builder.Add("public class A { public string Name {get;set;} = \"abc\"; }", UsingLoadBehavior.WithDefault);
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
            builder.Add("namespace TempA { public class A { public string Name {get;set;} = \"abcAaaa\"; }}", UsingLoadBehavior.WithDefault);
            builder.Add("public static class B{ public static void Show(){ Console.WriteLine(\"HelloWorld!\" + (new TempA.A()).Name); } }", UsingLoadBehavior.WithDefault);
            builder.Add("public class C: A { public string Name {get;set;} }", UsingLoadBehavior.WithDefault);
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
            builder.Add("public static class D{ public static object Show(){ return new A(); } }", UsingLoadBehavior.WithDefault);
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