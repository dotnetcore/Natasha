using PluginBase;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

namespace NatashaFunctionUT.Domain.Load
{
    [Trait("基础功能测试", "插件与域")]
    public class UnloadTest
    {
       
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string CreateAndUnload()
        {
            NatashaDomain? domain = default;
            using (DomainManagement.Create("au_test").CreateScope())
            {
                domain = DomainManagement.CurrentDomain;
                Assert.Equal("au_test", domain.Name);
            }
            domain.Dispose();
            return "au_test";
        }

        [Fact(DisplayName = "域的创建与卸载")]
        public static void ContextAndUnload()
        {

            var name = CreateAndUnload();
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagement.IsDeleted(name));

        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string LoadPluginAndUnload()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Domain", "Reference", "Libraries", "DNDV1.dll");
            NatashaDomain? domain = default;
            using (DomainManagement.Create("au_test_plugin").CreateScope())
            {
                domain = DomainManagement.CurrentDomain;
                Assert.Equal("au_test_plugin", domain.Name);
                var assembly = domain.LoadPluginWithHighDependency(path, item => item.Name != null && item.Name.Contains("PluginBase"));
                //var type = assembly.GetTypes().Where(item => item.Name == "P1").First();
                //IPluginBase plugin = (IPluginBase)(Activator.CreateInstance(type)!);
                //强制加载所有引用
                //var result = plugin!.PluginMethod1();
            }
            domain.Dispose();
            return "au_test_plugin";
        }

        [Fact(DisplayName = "域的清理与卸载")]
        public static void ClearAndUnload()
        {
            
            var name = LoadPluginAndUnload();
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagement.IsDeleted(name));

        }
    }
}
