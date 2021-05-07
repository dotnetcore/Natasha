using Natasha.CSharp;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace NatashaUT
{
    [Trait("插件测试", "文件方式")]
    public class PluginFileTest : PrepareTest
    {


        [Fact(DisplayName = "单次加载插件")]
        public void LoadFromDll1()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Static", "ClassLibrary6.dll");
            var domain = DomainManagement.Random;
            var assemebly = domain.LoadPluginFromFile(path);
            var action = NDelegate
                .UseDomain(domain)
                .Func<string>("Test.Instance.Name=\"11\"; return Test.Instance.Name;",assemebly);
            Assert.Equal("11", action());
        }


        [Fact(DisplayName = "多次加载插件")]
        public void LoadFromDll2()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Static", "ClassLibrary6.dll");
            var domain = DomainManagement.Random;
            var assemebly = domain.LoadPluginFromFile(path);
            assemebly = domain.LoadPluginFromFile(path);
            assemebly = domain.LoadPluginFromFile(path);
            assemebly = domain.LoadPluginFromFile(path);
            var action = NDelegate
                .UseDomain(domain)
                .Func<string>("Test.Instance.Name=\"11\"; return Test.Instance.Name;", assemebly);
            Assert.Equal("11", action());
        }


        [Fact(DisplayName = "加载不同版本插件1")]
        public void DiffDllTest()
        {
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV1", "TestRefererenceLibrary.dll");
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV2", "TestReferenceLibrary2.dll");

            var domain = DomainManagement.Random;

            //Load A => C v2.0
            var assembly2 = domain.LoadPluginFromFile(path2);
            //Load B => C v1.0
            var assembly = domain.LoadPluginFromFile(path1);

            var result = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary2.TestReference().Get();")();
            var result2 = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary.TestReference().Get();")();
            Assert.Equal(result, result2);
            Assert.Equal("1.0.0.0", result);
        }

        [Fact(DisplayName = "加载不同版本插件2")]
        public void DiffDllTest2()
        {
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV1", "TestRefererenceLibrary.dll");
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV2", "TestReferenceLibrary2.dll");
    
            var domain = DomainManagement.Random;
            
            //Load B => C v1.0
            var assembly = domain.LoadPluginFromFile(path1);
            //Load A => C v2.0
            var assembly2 = domain.LoadPluginFromFile(path2);

            var result = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary2.TestReference().Get();")();
            var result2 = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary.TestReference().Get();")();
            Assert.Equal(result, result2);
            Assert.Equal("2.0.0.0", result);
        }

        [Fact(DisplayName = "加载不同版本插件3")]
        public void DiffDllTest3()
        {
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV1", "TestRefererenceLibrary.dll");
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV2", "TestReferenceLibrary2.dll");
            string path3 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV3", "TestDifferentLibrary.dll");
            var domain = DomainManagement.Random;

            //Load B => C v1.0
            var assembly = domain.LoadPluginFromFile(path1);
            //Load A => C v2.0
            var assembly2 = domain.LoadPluginFromFile(path2);
            //Load C v3.0
            var assembly3 = domain.LoadPluginFromFile(path3);

            var result = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary2.TestReference().Get();")();
            var result2 = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary.TestReference().Get();")();
            Assert.Equal(result, result2);
            Assert.Equal("3.0.0.0", result);
        }
    }
}
