using Natasha;
using Natasha.Operator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace NatashaUT
{

    [Trait("程序集编译测试", "插件")]
    public class AssemblyPluginTest
    {
#if !NETCOREAPP2_2

        [Fact(DisplayName = "解构插件")]
        public void Test4()
        {
            using (DomainManagment.CreateAndLock("TempDomain14"))
            { 

                var (Assembly, TypeCache) = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Sql", "ClassLibrary1.dll");
                Assert.Equal("ClassLibrary1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Assembly.FullName);
                Assert.Equal("Class1", TypeCache["ClassLibrary1.Class1"].Name);
                Assert.Equal("ClassLibrary1.Class1", TypeCache["ClassLibrary1.Class1"].FullName);

            }
        }


        [Fact(DisplayName = "不可回收：MySql插件")]
        public void Test1()
        {

            Assert.Equal("Unable to connect to any of the specified MySQL hosts.",GetResult1());
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.False(DomainManagment.IsDeleted("TempDADomain11"));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetResult1()
        {
            string result;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib","Sql", "ClassLibrary1.dll");
            Assert.True(File.Exists(path));
            using (DomainManagment.CreateAndLock("TempDADomain11"))
            {

                var domain = DomainManagment.CurrentDomain;
                var assemebly = domain.LoadPluginFromStream(path);
                var action = FastMethodOperator.Use(domain)
                   .Using(assemebly)
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
            Assert.True(DomainManagment.IsDeleted("TempDomain12"));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetResult2()
        {
            string result;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Static", "ClassLibrary5.dll");
            Assert.True(File.Exists(path));
            using (DomainManagment.CreateAndLock("TempDomain12"))
            {

                var domain = DomainManagment.CurrentDomain;
                var assemebly = domain.LoadPluginFromStream(path);
                var action = FastMethodOperator.Use(domain)
                   .Using(assemebly)
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
            Assert.False(DomainManagment.IsDeleted("TempDomain13"));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetResult3()
        {
            string result;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Json", "ClassLibrary6.dll");
            Assert.True(File.Exists(path));
            using (DomainManagment.CreateAndLock("TempDomain13"))
            {

                var domain = DomainManagment.CurrentDomain;
                var assemebly = domain.LoadPluginFromStream(path);
                var action = FastMethodOperator.Use(domain,
                    item =>item.Compiler.ErrorBehavior = Natasha.Error.Model.ExceptionBehavior.Log 
                )
                   .Using(assemebly)
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
