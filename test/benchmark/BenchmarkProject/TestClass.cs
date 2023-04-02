using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;

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

    }
}
