using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Internal;
using System.Runtime.CompilerServices;
using static System.Diagnostics.DebuggableAttribute;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using HarmonyLib;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.PortableExecutable;
using static System.Reflection.Metadata.BlobBuilder;

namespace ReferenceSample
{

    internal class Program
    {
        
        static void Main(string[] args)
        {
            //NatashaManagement.Preheating(true, true);
            //var domain = NatashaManagement.CreateRandomDomain();
            //var asm = domain.LoadPluginUseDefaultDependency("I:\\OpenSource\\Natasha\\samples\\ReferenceSample\\bin\\Debug\\net8.0\\DynamicLibraryFolders\\Nc0e9a864079d427680ea239b5a9e525e\\a69937be3d244336a20c46843d51d19b.dll");

            TestMini();
            //var a = Math.Min(1, args.Length);
            //NatashaManagement.Preheating(false, false);
            //Console.WriteLine("=============================");
            //AssemblyCSharpBuilder builder = new();
            //var asm = builder
            //    .UseRandomDomain()
            //    .ConfigCompilerOption(item=>item.AddSupperess("CS8019"))
            //    .WithCombineReferences(item => item.UseDefaultReferences())
            //    .WithCombineUsingCode(UsingLoadBehavior.WithAll)
            //    .Add("public class A{}",UsingLoadBehavior.WithAll)
            //    .GetAssembly();
            //Console.WriteLine(asm.FullName);
            //Console.ReadKey();
            //NatashaInitializer.Preheating((asmName, name) => {
            //    if (name != null)
            //    {
            //        if (name.Contains("System"))
            //        {
            //            if (name.Contains("IO"))
            //            {
            //                //排除
            //                return true;
            //            }
            //        }
            //    }
            //    return false;
            //});
            //var type = typeof(HarmonyPatch);
            //var type2 = typeof(HarmonyPrefix);
            //Run();
            //TestMini();
            //TestMini();
            //NatashaManagement.Preheating(true, true);
            ////GC.Collect();
            ////Thread.Sleep(15000);
            //var method = typeof(Program).GetMethod("TestMini");
            //for (int i = 0; i < 5; i++) 
            //{
            //    method.Invoke(null,null);
            //    Thread.Sleep(3000);
            //}
            //Console.ReadKey();
            
        }

        public static void TestMini()
        {
            AssemblyCSharpBuilder builder = new();
            builder.WithAnalysisAccessibility()
                
                .UseRandomDomain()
                //.WithOutput()
                .WithoutCombineReferences()
                .WithoutSemanticCheck()
                .WithDebugCompile(item=>item.WriteToAssembly())
                //.WithReleaseCompile(false)
                //.OutputAsRefAssembly()
                .WithoutPrivateMembers()
                .WithCombineUsingCode(UsingLoadBehavior.WithCurrent)
                .AddReference(typeof(HarmonyPatch))
                .AddReference(typeof(DebuggableAttribute))
                .AddReference(typeof(object).Assembly)
                .AddReference(typeof(Math).Assembly)
                .AddReference(typeof(MathF).Assembly)
                .AddReference(typeof(SuppressMessageAttribute));

            builder.Add(@"
[assembly: TargetFramework("".NETCoreApp,Version=v8.0"", FrameworkDisplayName = "".NET 8.0"")]
public class A{  
    public static int N1 = 10;
    public static float N2 = 1.2F; 
    public static double N3 = 3.44;
    private static short N4 = 0;
    public static object Invoke2(){
        return new object();
    }
    public static object Invoke3<T>() where T : new()
    {
        return new T();
    }
    public static object Invoke(){
        var type = typeof(HarmonyPatch);
        var type2 = typeof(HarmonyPrefix);
        int[] a = [1,2,3];
        return N1 + MathF.Log10((float)Math.Sqrt(MathF.Sqrt(N2) + Math.Tan(N3)));
    }
    [SuppressMessage(""Microsoft.Performance"", ""CA1801:ReviewUnusedParameters"", MessageId = ""isChecked"")]
    public object Test(){
        return N1;
    }

}
");
            builder.Add(@"public class B{

    public object Invoke2(){
        return new object();
    }
    public object Invoke3<T>() where T : new()
    {
        return new T();
    }
}");
            builder.Add(@"
namespace Microsoft.CodeAnalysis.Runtime
{
        public static class Instrumentation
        {
            public static bool[] CreatePayload(System.Guid mvid, int methodToken, int fileIndex, ref bool[] payload, int payloadLength)
            {
                if (payload == null)
                {
                    payload = new bool[payloadLength];
                }
                return payload;
            }

            public static bool[] CreatePayload(System.Guid mvid, int methodToken, int[] fileIndices, ref bool[] payload, int payloadLength)
            {
                if (payload == null)
                {
                    payload = new bool[payloadLength];
                }
                return payload;
            }

            public static void FlushPayload()
            {

            }
        }
}");
            DebugDirectoryBuilder debug = new DebugDirectoryBuilder();
            debug.AddReproducibleEntry();
            debug.AddReproducibleEntry();
            var asm = builder.GetAssembly();
            var type = asm.GetType("A");
            var type1 = asm.GetType("<PrivateImplementationDetails>");
            var method = type.GetMethod("Invoke");
             var result = method.Invoke(null, null);
            var method2 = type.GetMethod("Invoke2");
            var result2 = method2.Invoke(Activator.CreateInstance(type), null);

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