using Natasha.CSharp;
using System;

namespace NET50
{
    class Program
    {
        static void Main(string[] args)
        {
            NatashaInitializer.InitializeAndPreheating();
            NDelegate.RandomDomain().Action("Console.WriteLine(\"Hello World!\");")();
            Console.ReadKey();
        }
    }
}
