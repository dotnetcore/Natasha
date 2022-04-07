using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnloadTest31
{
    class Program
    {
        const int count = 1000;
        const int arrayCount = 20480;
        static Type[] func = new Type[count];
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            Thread.Sleep(1000);
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"初始内存占用:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M");
            Console.WriteLine("-----------------------------------------------------------------------------------------");

            ShowTaskResoucesInfomation("Natasha预热", ()=> { NatashaInitializer.Preheating(); }, ConsoleColor.Magenta);
            Thread.Sleep(1000);
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"预热后内存占用:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M");
            Console.WriteLine("-----------------------------------------------------------------------------------------");

            ShowTaskResoucesInfomation($"{count}个独立域编译后", () => { Test(); }, ConsoleColor.Red);
            ShowTaskResoucesInfomation($"释放中", () => { Release(); }, ConsoleColor.Yellow);
            ShowTaskResoucesInfomation($"回收后", () => { RunGc(); }, ConsoleColor.Green);
            ShowTaskResoucesInfomation($"存活检测", () => {
                var alive = CheckAlive();
                DomainManagement.Clear();
            }, ConsoleColor.Cyan);

            
            Console.ReadKey();

        }


        public static double GetCpu(TimeSpan totleTime, TimeSpan preTime)
        {

            //间隔时间内的CPU运行时间除以逻辑CPU数量
            return (totleTime - preTime).TotalMilliseconds / 10 / (Environment.ProcessorCount-1);

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int CheckAlive()
        {

            int tempCount = 0;
            for (int i = 0; i < count; i++)
            {

                if (!DomainManagement.IsDeleted("test" + i.ToString()))
                {
                    tempCount++;
                    Console.WriteLine($"{i} is aliave!");
                }

            }
            return tempCount;

        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RunGc()
        {
            for (int i = 0; i < 20; i++)
            {
                GC.Collect();
                //GC.Collect(2);
                GC.WaitForPendingFinalizers();
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Release()
        {

            for (int i = 0; i < count; i++)
            {
                func[i] = null;
                var handler = DomainManagement.Get("test" + i.ToString());
                handler.Dispose();
            }
            func = null;

        }



        public static void ShowTaskResoucesInfomation(string title, Action action, ConsoleColor color)
        {
            Thread.Sleep(1000);
            var processor = Process.GetCurrentProcess();
            Stopwatch watch = new Stopwatch();

            //var userTime = processor.UserProcessorTime.TotalMilliseconds;
            watch.Start();
            var totalTime = processor.TotalProcessorTime.TotalMilliseconds;
            action();
            totalTime = processor.TotalProcessorTime.TotalMilliseconds - totalTime;
            watch.Stop();
            double cpuResult = 0;
            if (watch.Elapsed.Milliseconds > 10)
            {
                cpuResult = totalTime / Environment.ProcessorCount / watch.Elapsed.Milliseconds * 100;
            }
            Console.ForegroundColor = color;
            Console.WriteLine();
            Console.WriteLine($"{title}:");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{cpuResult:f2}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
        }




        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Test()
        {

            for (int i = 0; i < count; i++)
            {
                var type = NClass.CreateDomain("test" + i.ToString(),item=>item.DisableSemanticCheck())
                     .Namespace("Test")
                     .UseRandomName()
                     .PublicField<string>("Name")
                     .PublicField<string>("Name1")
                     .PublicField<string>("Name2")
                     .PublicField<string>("Name3")
                     .PublicField<string>("Name4")
                     .PublicField<string>("Name5")
                     .PublicField<string>("Age")
                     .PublicField<int[]>("Temp")
                     .Ctor(item => item.Body($"Temp = new int[{arrayCount}];"))
                    .GetType();
                //builder.Compiler.SyntaxInfos.SyntaxExceptions.Clear();
                //builder.Compiler.SyntaxInfos.TreeCodeMapping.Clear();
                //builder.Compiler.SyntaxInfos.TreeUsingMapping.Clear();
                //builder = null;
                if (type == default)
                {
                    throw new Exception("Bad Builder!");
                }
                func[i] = type;
            }
        }

    }
}
