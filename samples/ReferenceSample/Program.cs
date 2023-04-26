using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ReferenceSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NatashaManagement.Preheating();
            AssemblyCSharpBuilder builder = new();
            builder.Domain = DomainManagement.Random();

            

            builder.AddWithDefaultUsing("public class A { }");
            //此API可以在不用编译的情况下获取到已经格式化好的语法树.
            //注: GetAssembly 方法中已包括此方法, 但不会重复运行.
            builder.GetAvailableCompilation();
            var assemblyA = builder.GetAssembly();
            ShowAssemblyInfo(assemblyA);
            ShowSyntaxTree(builder.Compilation!);

            //清除编译信息,使 builder 可以重新组合 编译信息.
            builder.ClearCompilationCache();
            //重置程序集名,以免编译时产生冲突.
            builder.WithRandomAssenblyName();
            //向 builder 中追加类型 B.
            builder.AddWithDefaultUsing("public static class B{ public static void Show(){ Console.WriteLine(\"HelloWorld!\"); } }");
            
            var assemblyAB = builder.GetAssembly();
            //var a = assemblyAB.GetName().GetUniqueName();
            ShowAssemblyInfo(assemblyAB);
            ShowSyntaxTree(builder.Compilation!);

            var action = assemblyAB.GetDelegateFromShortName<Action>("B", "Show");
            Console.WriteLine("运行方法:");
            action();

            //action.DisposeDomain();
            //Console.WriteLine(DomainManagement.IsDeleted(builder.Domain!.Name!));
            Console.ReadKey();

            static void ShowAssemblyInfo(Assembly assembly)
            {

                Console.WriteLine("此程序集名字:"+assembly.FullName);
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