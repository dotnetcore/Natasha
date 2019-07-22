using BenchmarkDotNet.Running;
using System;

namespace NatashaBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            //为兼容多版本测试，请使用命令行功能。
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            Console.ReadKey();
        }
    }
}
