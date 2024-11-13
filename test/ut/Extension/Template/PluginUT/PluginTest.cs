using Natasha.CSharp;
using System;
using System.IO;
using Xunit;

namespace NatashaUT
{
    [Trait("类型获取", "")]
    public class PluginTest : PrepareTest
    {


        [Fact(DisplayName = "以文件方式加载插件，并使用")]
        public void LoadFromFileAndUseIt()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Static", "ClassLibrary5.dll");
            var domain = DomainManagement.Random;
            var assemebly = domain.LoadPlugin(path);
            var action = NDelegate
                .UseDomain(domain)
                .AddUsing(assemebly)
                .Func<string>("Test.Instance.Name=\"11\"; return Test.Instance.Name;");
            Assert.Equal("11", action());
        }


        
    }
}
