﻿using Xunit;

namespace NatashaFunctionUT.Domain.Plugin
{
    [Trait("基础功能测试", "插件与域")]
    public class SNDVDVTest: PluginPrepare
    {
        [Fact(DisplayName = "[同名不同版本插件][同名不同版本依赖]测试")]
        public void Test()
        {
            var path1 = PathCombine1("SNDVDV.dll");
            var path2 = PathCombine2("SNDVDV.dll");

            var result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInformation.None, false);
            Assert.Equal("InvalidCastException", result.r1);
            Assert.Equal("FileLoadException", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInformation.None);
            Assert.Equal("Json:12.0.0.0;Dapper:1.60.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("FileLoadException", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInformation.UseHighVersion, false);
            Assert.Equal("Json:12.0.0.0;Dapper:1.60.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("FileLoadException", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInformation.UseLowVersion, false);
            Assert.Equal("Json:9.0.0.0;Dapper:1.60.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("FileLoadException", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, AssemblyCompareInformation.UseDefault, false);
            Assert.Equal("Json:9.0.0.0;Dapper:1.60.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("FileLoadException", result.r2);
        }
    }
}
