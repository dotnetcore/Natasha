using BenchmarkDotNet.Running;
using System;

namespace NatashaBenchmark
{
    class Program
    {
        static Action<string[]> Temp;
        static void Main(string[] args)
        {
            //Temp = new Action<string[]>(Program.Main);
            //Temp = Program.Main;
            //var a = typeof(Program).GetMethod("Main", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
            //Console.WriteLine(Temp.Method.MethodHandle == a.MethodHandle);
            //为兼容多版本测试，请使用命令行功能。
            //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            //BenchmarkRunner.Run<UnsafeDelegate>();

            //DynamicCallFieldTest a = new DynamicCallFieldTest();
            Console.ReadKey();
        }
    }
}
