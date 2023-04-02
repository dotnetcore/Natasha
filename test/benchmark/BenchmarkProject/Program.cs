using BenchmarkDotNet.Running;

namespace BenchmarkProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<TestClass>();
            Console.ReadKey();
        }
    }
}