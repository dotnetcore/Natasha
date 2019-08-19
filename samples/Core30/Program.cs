using Natasha;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Core30
{
    class Program
    {
        public static Action action;
        static void Main(string[] args)
        {
            Console.WriteLine("隔离编译动态方法:");
            Console.WriteLine();
            Show();
            if (action!=null)
            {
                Console.WriteLine("\t静态引用动态方法，增加方法代数！");
            }
            //var a = AssmblyManagment.Remove("TempDomain");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("第一次检测：");
            Console.WriteLine(AssemblyManagment.IsDelete("TempDomain") ? "回收成功！" : "回收失败！");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("启用GC回收方法！");

            for (int i = 0;(!AssemblyManagment.IsDelete("TempDomain")) && (i < 15); i++)
            {
                Console.WriteLine($"\t第{i}次！");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(500);
                if (i==10)
                {
                    Console.WriteLine("\t计数为10，删除静态引用！");
                    action = null;
                }
                
            }
            Console.WriteLine();
            Console.WriteLine();
            //Console.WriteLine(!a.IsAlive? "回收成功！":"回收失败！");
            Console.Write("第二次检测：");
            Console.WriteLine(AssemblyManagment.IsDelete("TempDomain") ? "回收成功！" : "回收失败！");
            
            action?.Invoke();
            Console.ReadKey();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Show()
        {
            var domain = AssemblyManagment.Create("TempDomain");
            domain.Unloading += Domain_Unloading;
            // using (domain)
            //{
            var temp = NewMethod.Create<Action>(builder =>
            {
                    //builder.Complier.UseFileComplie(); 
                    builder
                 .InDomain(domain)
                 .MethodBody(@"Console.WriteLine(""\t动态功能输出：Hello World!"");");
            });

            action = temp.Method;
            //////action();
            temp.Method();
            // }
        }

        private static void B_Unloading(System.Runtime.Loader.AssemblyLoadContext obj)
        {
            Console.WriteLine("B Unloding!");
        }

        private static void Domain_Unloading(System.Runtime.Loader.AssemblyLoadContext obj)
        {
            Console.WriteLine("\t\t触发回收事件!");
        }
    }
}
