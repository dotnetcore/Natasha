using Natasha.CSharp;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using Xunit;

namespace NatashaUT
{

    [Trait("程序集编译测试", "插件")]
    public class AssemblyPluginTest : PrepareTest
    {

#if !NETCOREAPP2_2

        
        [Fact(DisplayName = "不可回收：MySql插件")]
        public void Test1()
        {

            //Assert.Equal("Unable to connect to any of the specified MySQL hosts.",GetResult1());
            GetResult1();
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagement.IsDeleted("TempDADomain11"));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetResult1()
        {
            string result;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib","Sql", "ClassLibrary1.dll");
            Assert.True(File.Exists(path));
            using (DomainManagement.CreateAndLock("TempDADomain11"))
            {

                var domain = DomainManagement.CurrentDomain;
                var assemebly = domain.LoadPluginFromStream(path);
                var action = FastMethodOperator.UseDomain(domain)
                   .Body(@"
try{
Class1 a = new Class1();
return  a.Show();
}
catch(Exception e){
    Console.WriteLine(e.Message);  
    return e.Message;
}
return default;").Return<string>()

                   .Compile<Func<string>>();
                result = action();
                domain.Dispose();
            }
            return result;
        }


        [Fact(DisplayName = "可回收：静态引用")]
        public void Test2()
        {

            Assert.Equal("11",GetResult2());
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagement.IsDeleted("TempDomain12"));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetResult2()
        {
            string result;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Static", "ClassLibrary5.dll");
            Assert.True(File.Exists(path));
            using (DomainManagement.CreateAndLock("TempDomain12"))
            {

                var domain = DomainManagement.CurrentDomain;
                var assemebly = domain.LoadPluginFromStream(path);
                var action = FastMethodOperator.UseDomain(domain)
                   .Body(@"Test.Instance.Name=""11""; return Test.Instance.Name;")
                   .Compile<Func<string>>();
                result = action();
                domain.Dispose();
                domain.Unload();
            }
            return result;
        }



        [Fact(DisplayName = "不可回收：Json.net")]
        public void Test3()
        {

            Assert.Equal("{\"Name\":\"11\"}", GetResult3());
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.False(DomainManagement.IsDeleted("TempDomain13"));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetResult3()
        {
            string result;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Json", "ClassLibrary6.dll");
            Assert.True(File.Exists(path));
            using (DomainManagement.CreateAndLock("TempDomain13"))
            {

                var domain = DomainManagement.CurrentDomain;
                var assemebly = domain.LoadPluginFromStream(path);
                var action = FastMethodOperator.UseDomain(domain,
                    item =>item.LogCompilerError() 
                )
                   .Body(@"Class1 obj = new Class1(); return obj.Get();")
                   .Compile<Func<string>>();
                result = action();
                domain.Dispose();
                domain.Unload();
            }
            return result;
        }

#endif

    }
}
