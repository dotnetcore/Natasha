// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using NatashaBenchmark;

BenchmarkRunner.Run<UnsafeDelegate>();
Console.ReadKey();