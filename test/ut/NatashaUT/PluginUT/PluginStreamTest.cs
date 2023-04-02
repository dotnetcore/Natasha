using Natasha.CSharp;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace NatashaUT
{
    [Trait("插件测试", "文件方式")]
    public class PluginStreamTest : PrepareTest
    {


        [Fact(DisplayName = "单次加载插件")]
        public void LoadFromDll1()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Static", "ClassLibrary6.dll");
            var domain = DomainManagement.Random;
            var assemebly = domain.LoadPlugin(path);
            var action = NDelegate
                .UseDomain(domain)
                .AddUsing(assemebly)
                .Func<string>("Test.Instance.Name=\"11\"; return Test.Instance.Name;");
            Assert.Equal("11", action());
        }


        [Fact(DisplayName = "多次加载插件")]
        public void LoadFromDll2()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Static", "ClassLibrary6.dll");
            var domain = DomainManagement.Random;
            var assemebly = domain.LoadPlugin(path);
            assemebly = domain.LoadPlugin(path);
            assemebly = domain.LoadPlugin(path);
            assemebly = domain.LoadPlugin(path);
            var action = NDelegate
                .UseDomain(domain)
                .AddUsing(assemebly)
                .Func<string>("Test.Instance.Name=\"11\"; return Test.Instance.Name;");
            Assert.Equal("11", action());
        }


        [Fact(DisplayName = "不同依赖 - 先加载底版本")]
        public void DiffDllTest()
        {
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV1", "TestRefererenceLibrary.dll");
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV2", "TestReferenceLibrary2.dll");

            var domain = DomainManagement.Random;
            //Load B => C v1.0
            var assembly = domain.LoadPlugin(path1);
            var assembly2 = domain.LoadPlugin(path2);
            var result = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary.TestReference().Get();")();

            //Load A => C v2.0
            
            var result2 = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary2.TestReference().Get();")();
            Assert.Equal(result, result2);
            Assert.Equal("2.0.0.0", result);


        }

        [Fact(DisplayName = "不同依赖 - 先加载高版本")]
        public void DiffDllTest2()
        {
            string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV1", "TestRefererenceLibrary.dll");
            string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV1", "asmV2", "TestReferenceLibrary2.dll");
    
            var domain = DomainManagement.Random;

            //Load A => C v2.0
            var assembly2 = domain.LoadPlugin(path2);
            var result = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary2.TestReference().Get();")();

            //#if !NETCOREAPP2_2
            //Load B => C v1.0
            var assembly = domain.LoadPlugin(path1);
            var result2 = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary.TestReference().Get();")();
            Assert.Equal(result, result2);
            //#endif

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
            var assembly = domain.LoadPlugin(path1);
            var assembly2 = domain.LoadPlugin(path2);
            //Load C v3.0
            var assembly3 = domain.LoadPlugin(path3);
            var result = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary.TestReference().Get();")();


            //Load A => C v2.0
            var result2 = NDelegate.UseDomain(domain).Func<string>("return new TestRefererenceLibrary2.TestReference().Get();")();
            Assert.Equal(result, result2);

            Assert.Equal("3.0.0.0", result);
        }


        //[Fact(DisplayName = "先加载低版本程插件")]
        //public void DiffDllTest4()
        //{
        //    string path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV3", "v1", "DiffVersionLibrary.dll");
        //    string path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Diff", "fileV3", "v2", "DiffVersionLibrary.dll");
        //    var domain = DomainManagement.Random;
           
        //    //Load  v1.0
        //    var assembly = domain.LoadPlugin(path1);
        //    //Load  v2.0
        //    var assembly2 = domain.LoadPlugin(path2);


        //    var result = NDelegate.UseDomain(domain).Func<string>("return new Plugin.Show();")();
        //    var result2 = NDelegate.UseDomain(domain).Func<string>("return new Plugin.Show();")();
        //    Assert.NotEqual(result, result2);
        //    Assert.Equal("3.0.0.0", result);
        //}
    }
}
