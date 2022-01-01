﻿using Xunit;

namespace NatashaFunctionUT.Domain.Plugin
{
    [Trait("基础功能测试", "插件")]
    public class SNDVSVTest: PluginBaseTest
    {
        [Fact(DisplayName = "[同名不同版本插件][同依赖]测试")]
        public void Test()
        {
            var path1 = PathCombine1("SNDVSV.dll");
            var path2 = PathCombine2("SNDVSV.dll");

            var result = PluginAssertHelper.GetResult(path1, path2, LoadBehaviorEnum.None);
            Assert.Equal("InvalidCastException", result.r1);
            Assert.Equal("FileLoadException", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, LoadBehaviorEnum.None, true);
            Assert.Equal("Json:12.0.0.0;Dapper:1.60.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("FileLoadException", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, LoadBehaviorEnum.UseHighVersion);
            Assert.Equal("Json:12.0.0.0;Dapper:1.60.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("FileLoadException", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, LoadBehaviorEnum.UseLowVersion);
            Assert.Equal("Json:9.0.0.0;Dapper:1.60.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("FileLoadException", result.r2);

            result = PluginAssertHelper.GetResult(path1, path2, LoadBehaviorEnum.UseBeforeIfExist);
            Assert.Equal("Json:9.0.0.0;Dapper:1.60.0.0;IPluginBase:1.0.0.0;Self:1.0.0.0", result.r1);
            Assert.Equal("FileLoadException", result.r2);
        }
    }
}