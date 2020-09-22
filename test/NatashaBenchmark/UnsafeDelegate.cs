using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace NatashaBenchmark
{

    [MemoryDiagnoser, MarkdownExporter, RPlotExporter]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    [CategoriesColumn]
    public unsafe class UnsafeDelegate
    {
        public FuncHandler<int, int, string> func;
        public Func<int, int, string> func2;
        public delegate* managed<int, int, string> point;
        public delegate*<int, int, string> point1;
        public UnsafeDelegate()
        {

            func = new FuncHandler<int, int, string>();
            func.Invoke = &(TestModel.Get);
            func2 = TestModel.Get;
            point = &(TestModel.Get);
            point1 = &Get;


        }
        public string Get(int a,int b)
        {
            return default;
        }
        [Benchmark(Description = "delegate*(packed)")]
        public void UsePackagePoint()
        {
            func.Invoke(10, 10);
        }

        [Benchmark(Description = "delegate*")]
        public void UseDirectPoint()
        {
            point(10, 10);
        }

        [Benchmark(Description = "system")]
        public void UseSystem()
        {
            func2(10, 10);
        }

        [Benchmark(Description = "origin")]
        public void UseOrigin()
        {
            TestModel.Get(10, 10);
        }
    }

    

    public unsafe class ActionHandler<T1,T2>
    {
        public delegate* managed<T1, T2, void> Invoke;
    }
    public unsafe class FuncHandler<T1, T2, T3>
    {
        public delegate* managed<T1,T2,T3> Invoke;
    }


    public static class TestModel 
    {
    
        public static string Get(int a,int b)
        {
            //return (a + b).ToString();
            return default;
        }
    }

}
