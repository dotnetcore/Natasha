using BenchmarkDotNet.Running;
using System;

namespace NatashaBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<DynamicCallFieldTest>();
            Console.ReadKey();
        }
    }
}
