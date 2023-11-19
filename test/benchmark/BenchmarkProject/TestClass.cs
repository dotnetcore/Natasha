using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using System.Globalization;
using System.Reflection;

namespace BenchmarkProject
{

    //Show: Github CV-VP 
    //[MarkdownExporter]

    //Show: Webview
    //[HtmlExporter]

    //Show: Text
    //[PlainExporter]

    [RPlotExporter]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [GcConcurrent]
    [MemoryDiagnoser]
    //[MemoryRandomization]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    [CategoriesColumn]

    //faster
    //[ShortRunJob]
    public class TestClass
    {
        private readonly object _arg0 = 4, _arg1 = 5, _arg2 = 6;
        private readonly int _arg00 = 4, _arg11 = 5, _arg22 = 6;
        private readonly object[] _args3 = new object[] { 4, 5, 6 };
        private MethodInfo _method3;
        private MethodInvoker _method3Invoker;
        private Action<int, int, int> _method3Action;
        private DateTime _now;
        [GlobalSetup]
        public void Setup()
        {
            _now = DateTime.Now;
            _method3 = typeof(TestClass).GetMethod("MyMethod3", BindingFlags.NonPublic | BindingFlags.Static);
            _method3Invoker = MethodInvoker.Create(_method3);
            _method3Action = (Action<int, int, int>)Delegate.CreateDelegate(typeof(Action<int, int, int>), _method3);
        }

        //[Benchmark(Baseline = true)]
        //public void MethodBaseInvoke() => _method3.Invoke(null, _args3);

        //[Benchmark]
        //public void MethodInvokerInvoke() => _method3Invoker.Invoke(null, _arg0, _arg1, _arg2);

        //[Benchmark]
        //public void DelegateInvokerInvoke() => _method3Action(_arg00, _arg11, _arg22);
        [Benchmark]
        public void TimeFormatWithoutCulture()
        {
            var result = _now.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'");
        }
        [Benchmark]
        public void TimeFormatWithCulture()
        {
            var result = _now.ToString("ddd, dd MMM yyyy HH':'mm':'ss 'GMT'", CultureInfo.InvariantCulture);
        }
        private static void MyMethod3(int arg1, int arg2, int arg3) { }
    }
}
