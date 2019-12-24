using Natasha;
using Natasha.Log;
using Natasha.Operator;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnloadTest31
{
    class Program
    {
        const int count = 100;
        static Func<int[]>[] func = new Func<int[]>[100];
        static void Main(string[] args)
        {

            NError.Enabled = false;
            NSucceed.Enabled = false;
            NWarning.Enabled = false;

            var preTime = Process.GetCurrentProcess().TotalProcessorTime;
            Stopwatch watch = new Stopwatch();
            watch.Start();
            var a = CtorOperator.Create().NewDelegate<Process>();
            watch.Stop();
            Thread.Sleep(1000);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Natasha预热:");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime).ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 /1024/1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Thread.Sleep(3000);
            preTime = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Restart();
            Test();
            watch.Stop();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("100个独立域编译后:");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime).ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Thread.Sleep(3000);
            preTime = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Restart();
            Release();
            watch.Stop();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("释放中:");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime).ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Thread.Sleep(3000);
            preTime = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Restart();
            RunGc();
            watch.Stop();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("回收后:");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime).ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            preTime = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Restart();
            var alive = CheckAlive();
            watch.Stop();
            Thread.Sleep(4000);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine($"存活检测: {alive}");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime).ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64/1024/1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.ReadKey();
        }


        public static double GetCpu(TimeSpan totleTime, TimeSpan preTime)
        {

            //间隔时间内的CPU运行时间除以逻辑CPU数量

            return  (totleTime - preTime).TotalMilliseconds / 1000 / Environment.ProcessorCount * 100;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int CheckAlive()
        {

            int count = 0;
            for (int i = 0; i < count; i++)
            {

                if (!DomainManagment.IsDeleted("test" + i.ToString()))
                {
                    count++;
                    Console.WriteLine($"{i} is aliave!");
                }

            }
            return count;

        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RunGc()
        {
            for (int i = 0; i < 20; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Release()
        {
            func = null;
            for (int i = 0; i < count; i++)
            {
                var handler = DomainManagment.Get("test" + i.ToString());
                handler.Dispose();
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Test()
        {

            for (int i = 0; i < count; i++)
            {
                var builder = FastMethodOperator.Create("test" + i.ToString());
                func[i] = builder.MethodBody($@"
int[] a = new int[80960];
for(int i =0;i<80960;i++){{a[i]=i;}}
string temp = @""111111111111111111111111
11111111111111
111111111111111111111
111111111111111111111
111111111111111111111
11111111111111111111111111111111111
111111111111111111111111111111111111111111
111111111111111111111111111111111111111111
111111111111111111111
111111111111111111111
11111111111111111111111111111111111
111111111111111111111111111111111111111111
1111111111111111111111111111111111111
111111111111111111111
111111111111111111111
11111111111111111111111111111111111
111111111111111111111111111111111111111111
1111111111111111111111111111111111111
111111111111111111111
111111111111111111111
11111111111111111111111111111111111
111111111111111111111111111111111111111111
1111111111111111111111111111111111111
11111111111111111111111111111111111
11111111111111111111111111111111111
1111111111111111111111111111
1111111111111111111111111111
11111111111111111111111111111111111111"";
a[0]+=temp.Length;
return a;
").Complie<Func<int[]>>();
                func[i]();
            }
        }
    }
}
