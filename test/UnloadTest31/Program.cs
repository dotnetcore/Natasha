using Natasha.CSharp;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnloadTest31
{
    class Program
    {
        const int count = 2000;
        static Type[] func = new Type[count];
        static Type tempType;
        static void Main(string[] args)
        {
            NatashaComponentRegister.RegistDomain<NatashaAssemblyDomain>();
            NatashaComponentRegister.RegistCompiler<NatashaCSharpCompiler>();
            NatashaComponentRegister.RegistSyntax<NatashaCSharpSyntax>();

            NErrorLog.Enabled = false;
            NSucceedLog.Enabled = false;
            NWarningLog.Enabled = false;
            Stopwatch watch = new Stopwatch();
            double tempTotleTime;
            #region Natasha Preheating
            var preTime = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Start();
            tempType = NClass.CreateDomain("tes1t")
                    .Namespace("Test")
                    .UseRandomName()
                    .PublicField<string>("Name")
                    .PublicField<string>("Age")
                    .PublicField<int[]>("Temp")
                    .Ctor(item => item.Body("Temp = new int[40960];"))
                    .GetType();
            watch.Stop();
            tempTotleTime = GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime);
            if (watch.Elapsed.Seconds > 0)
            {
                tempTotleTime = tempTotleTime / watch.Elapsed.Seconds;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Natasha预热:");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{tempTotleTime.ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            #endregion

            Thread.Sleep(1000);

            #region Run Compiler
            preTime = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Restart();
            Test();
            watch.Stop();
            tempTotleTime = GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime);
            if (watch.Elapsed.Seconds > 0)
            {
                tempTotleTime = tempTotleTime / watch.Elapsed.Seconds;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{count}个独立域编译后:");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{tempTotleTime.ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            #endregion

            Thread.Sleep(1000);

            #region Release Handler
            preTime = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Restart();
            Release();
            watch.Stop();
            tempTotleTime = GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime);
            if (watch.Elapsed.Seconds > 0)
            {
                tempTotleTime = tempTotleTime / watch.Elapsed.Seconds;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("释放中:");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{tempTotleTime.ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            #endregion

            Thread.Sleep(1000);
            #region Run GC
            preTime = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Restart();
            RunGc();
            watch.Stop();
            tempTotleTime = GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime);
            if (watch.Elapsed.Seconds>0)
            {
                tempTotleTime = tempTotleTime / watch.Elapsed.Seconds;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("回收后:");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{tempTotleTime.ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            #endregion

            Thread.Sleep(1000);

            #region Check Alive
            preTime = Process.GetCurrentProcess().TotalProcessorTime;
            watch.Restart();
            var alive = CheckAlive();
            DomainManagement.Clear();
            watch.Stop();
            tempTotleTime = GetCpu(Process.GetCurrentProcess().TotalProcessorTime, preTime);
            if (watch.Elapsed.Seconds > 0)
            {
                tempTotleTime = tempTotleTime / watch.Elapsed.Seconds;
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine($"存活检测: {alive}");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            Console.WriteLine($"|\tCPU:{tempTotleTime.ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            Console.WriteLine("-----------------------------------------------------------------------------------------");
            #endregion

            //for (int i = 0; i < 10; i++)
            //{
            //    Thread.Sleep(3000);
            //    preTime = Process.GetCurrentProcess().TotalProcessorTime;
            //    Console.WriteLine($"第{i}次静默检测:");
            //    Console.WriteLine("-----------------------------------------------------------------------------------------");
            //    Console.WriteLine($"|\tCPU:{tempTotleTime.ToString("f2")}%\t|\t内存:{Process.GetCurrentProcess().PrivateMemorySize64 / 1024 / 1024}M\t|\t执行耗时：{watch.Elapsed}\t|");
            //    Console.WriteLine("-----------------------------------------------------------------------------------------");
            //}
            
            Console.ReadKey();

        }


        public static double GetCpu(TimeSpan totleTime, TimeSpan preTime)
        {

            //间隔时间内的CPU运行时间除以逻辑CPU数量

            return (totleTime - preTime).TotalMilliseconds / 1000 / Environment.ProcessorCount * 100;
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


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Test()
        {

            for (int i = 0; i < count; i++)
            {
                var type = NClass.CreateDomain("test" + i.ToString())
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
                     .Ctor(item => item.Body("Temp = new int[40960];"))
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
