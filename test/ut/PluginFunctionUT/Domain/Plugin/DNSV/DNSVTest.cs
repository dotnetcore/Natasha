using Xunit;

namespace NatashaFunctionUT.Domain.Plugin
{
    [Trait("基础功能测试", "插件与域")]
    public class DNSVTest: PluginPrepare
    {
        [Fact(DisplayName = "[不同插件][同名同版本依赖]测试")]
        public void Test()
        {
            var path1 = PathCombine1("DNSV1.dll");
            var path2 = PathCombine2("DNSV2.dll");

            var result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInfomation.None, false);
            Assert.Equal("InvalidCastException", result.r1);
            Assert.Equal("InvalidCastException", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInfomation.None);
            Assert.Equal("Json:13.0.0.0;Dapper:2.0.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("Json:13.0.0.0;Dapper:2.0.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInfomation.UseHighVersion);
            Assert.Equal("Json:13.0.0.0;Dapper:2.0.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("Json:13.0.0.0;Dapper:2.0.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInfomation.UseLowVersion);
            Assert.Equal("Json:9.0.0.0;Dapper:2.0.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("Json:9.0.0.0;Dapper:2.0.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInfomation.UseDefault);
            Assert.Equal("Json:9.0.0.0;Dapper:2.0.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("Json:9.0.0.0;Dapper:2.0.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r2);
        }
    }
}
