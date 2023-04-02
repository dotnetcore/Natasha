using Natasha.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

namespace NatashaUT
{

    [Trait("程序集编译测试", "插件")]
    public class AssemblyPluginTest : PrepareTest
    {



        
        [Fact(DisplayName = "不可回收：MySql插件")]
        public void Test1()
        {

            //Assert.Equal("Unable to connect to any of the specified MySQL hosts.",GetResult1());
            var name = GetResult1();
            for (int i = 0; i < 6; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagement.IsDeleted(name));

        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetResult1()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib","Sql", "ClassLibrary1.dll");
            Assert.True(File.Exists(path));
            var name = Guid.NewGuid().ToString("N");
            using (DomainManagement.CreateAndLock(name))
            {
               
                var domain = DomainManagement.CurrentDomain;
                
                var assembly = domain.LoadPlugin(path);
                var assemblyCount = domain.Assemblies.Count();
                var operat = NatashaOperator.MethodOperator;
                var action = operat
                    .WithCS0104Handler()
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
                var result = action();
                Assert.Equal(name, domain.Name);
                Assert.Equal(name, operat.AssemblyBuilder.Compiler.Domain.Name);
                
                domain.Dispose();
            }
            return name;
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
                Assert.Equal("TempDomain12", domain.Name);
                var assembly = domain.LoadPlugin(path);
                var assemblyCount = domain.Assemblies.Count();
                var operat = NatashaOperator.MethodOperator;
                var action = operat
                   .Body(@"Test.Instance.Name=""11""; return Test.Instance.Name;")
                   .Compile<Func<string>>();
                result = action();
                domain.Dispose();
                domain.Unload();
                Assert.Equal(assemblyCount + 1, domain.Assemblies.Count());
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
                Assert.Equal("TempDomain13", domain.Name);
                var assemebly = domain.LoadPlugin(path);
                var assemblyCount = domain.Assemblies.Count();
                var operat = NatashaOperator.MethodOperator;
                var action = operat
                   .Body(@"Class1 obj = new Class1(); return obj.Get();")
                   .Compile<Func<string>>();
                result = action();
                Assert.Equal("TempDomain13", operat.AssemblyBuilder.Compiler.Domain.Name);
                domain.Dispose();
                domain.Unload();

            }
            return result;
        }


    }
}
