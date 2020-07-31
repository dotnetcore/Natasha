using Natasha;
using Natasha.CSharp;
using NatashaUT.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;

namespace NatashaUT
{
    [Trait("程序集编译测试", "单域")]
    public class AssemblyTest : PrepareTest
    {
        [Fact(DisplayName = "多程序集协作")]
        public void Test1()
        {
            //ForTest1();
            Assert.Equal("HelloTest", ForTest1());
#if !NETCOREAPP2_2
            for (int i = 0; i < 10; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagement.IsDeleted("SingleDomainAsmTest1"));
#endif

        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        internal string ForTest1()
        {

            var domain = DomainManagement.Create("SingleDomainAsmTest1");
            var assembly = domain.CreateAssembly("AsmTest1");

            var @interface = assembly
                .CreateInterface("IAsmT1")
                .Using("System")
                .Public()
                .BodyAppend("string ShowMethod(string str);");


            var @class = assembly
                .CreateClass("ClassAsm")
                .Using("System")
                .Public()
                .Inheritance("IAsmT1")
                .Method(method => method
                    .Public()
                    .Name("ShowMethod")
                    .Param<string>("str")
                    .Body("return str+AsmEnum.Test.ToString();")
                    .Return<string>()
                 );

            var @enum = assembly
                .CreateEnum("AsmEnum")
                .Public()
                .EnumField("Test")
                .EnumField("Test1")
                .EnumField("Test2", 1);

            var result = assembly.GetAssembly();
            var type = assembly.GetTypeFromFullName(@class.NamespaceScript+"."+"ClassAsm");

            var builder = FastMethodOperator.DefaultDomain();
            builder.AssemblyBuilder.Compiler.Domain = domain;
            var @delegate = builder.Body(@"
            ClassAsm obj = new ClassAsm();
            return obj.ShowMethod(""Hello"");
            ").Compile<Func<string, string>>();
            DomainManagement.Get("SingleDomainAsmTest1").Dispose();
            return @delegate("hello");


        }




        [Fact(DisplayName = "多程序集覆盖")]
        public void Test2()
        {
            var domain = DomainManagement.Create("SingleDomainAsmTest2");
            var assembly = domain.CreateAssembly("AsmTest1");

            var @interface = assembly
                .CreateInterface("IAsmT1")
                .Using("System")
                .Public()
                .BodyAppend("string ShowMethod(string str);");


            var @class = assembly
                .CreateClass("ClassAsm")
                .Using("System")
                .Public()
                .Inheritance("IAsmT1")
                .Method(method => method
                    .Public()
                    .Name("ShowMethod")
                    .Param<string>("str")
                    .Body("return str+AsmEnum.Test.ToString();")
                    .Return<string>()
                 );


            var @enum = assembly
               .CreateEnum("AsmEnum")
               .Public()
               .EnumField("Test")
               .EnumField("Test1")
               .EnumField("Test2", 1);


            var result = assembly.GetAssembly();
            var type = assembly.GetTypeFromFullName(@class.NamespaceScript + "."+"ClassAsm");
            domain.Remove(type.Assembly);


            assembly = domain.CreateAssembly("AsmTest2");
            @interface = assembly
                .CreateInterface("IAsmT1")
                .Using("System")
                .Public()
                .BodyAppend("string ShowMethod(string str);");


            @class = assembly
                .CreateClass("ClassAsm")
                .Using("System")
                .Public()
                .Inheritance("IAsmT1")
                .Method(method => method
                    .Public()
                    .Name("ShowMethod")
                    .Param<string>("str")
                    .Body("return str+AsmEnum.Test.ToString()+\"1\";")
                    .Return<string>()
                 );


            @enum = assembly
               .CreateEnum("AsmEnum")
               .Public()
               .EnumField("Test")
               .EnumField("Test1")
               .EnumField("Test2", 1);


            result = assembly.GetAssembly();
            type = assembly.GetTypeFromFullName(@class.NamespaceScript+".ClassAsm");


            var builder = FastMethodOperator.DefaultDomain();
            builder.AssemblyBuilder.Compiler.Domain = domain;
            var @delegate = builder.Using(type).Body(@"
ClassAsm obj = new ClassAsm();
return obj.ShowMethod(""Hello"");
").Compile<Func<string, string>>();

            Assert.Equal("HelloTest1", @delegate("hello"));
#if !NETCOREAPP2_2
            @delegate.DisposeDomain();
            for (int i = 0; i < 10; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Assert.True(DomainManagement.IsDeleted("SingleDomainAsmTest1"));
#endif

        }

#if !NETCOREAPP2_2
        [Fact(DisplayName = "自定义域解构编译")]
        public void Test4()
        {
            using (DomainManagement.CreateAndLock("TempDomain15"))
            {

                var (Assembly, Exception) = @"
using System;
namespace TT{

public class Test{


}

}
";
                Assert.Equal("Test", Assembly.GetExportedTypes()[0].Name);

            }
        }

#endif
        [Fact(DisplayName = "共享域解构编译")]
        public void Test5()
        {
                var (Assembly, Exception) = @"
using System;
namespace TT{
public class Test{}
}
";
                Assert.Equal("Test", Assembly.GetExportedTypes()[0].Name);
        }

        
        [Fact(DisplayName = "同域同程序集覆盖")]
        public void Test6()
        {
            
            string assemblyName = "tsda";
            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder(assemblyName);
            builder.Compiler.Domain = DomainManagement.Create("a");
            builder.Add("public class TSDA{}");
            var assembly = builder.GetAssembly();
            assembly.RemoveReferences();
            Assert.NotNull(assembly);
            builder = new AssemblyCSharpBuilder();
            builder.Compiler.Domain = DomainManagement.Create("a");
            builder.Add("public class TSDA{}");
            var assembly1 = builder.GetAssembly();
            Assert.NotEqual(assembly, assembly1);
            var func = NDelegate.CreateDomain("a").Func<string>("return typeof(TSDA).Assembly.FullName;");
            Assert.Equal(assembly1.FullName, func());

        }



#if !NETCOREAPP2_2

        [Fact(DisplayName = "域锁与管理")]
        public void Test3()
        {
            using (DomainManagement.CreateAndLock("CDomain1"))
            {
                var domain = DomainManagement.CurrentDomain;
                Assert.Equal("CDomain1", domain.Name);
            }
        }

        [Fact(DisplayName = "HelloWorldTest")]
        public void TestHelloWorld()
        {

            using (DomainManagement.CreateAndLock("MyDomain"))
            {

                var domain = DomainManagement.CurrentDomain;
                var assembly = domain.CreateAssembly("MyAssembly");


                //创建一个接口
                assembly
                    .CreateInterface("InterfaceTest")
                    .Using("System")
                    .Public()
                    .BodyAppend("string ShowMethod(string str);");


                //创建一个类并实现​接口
               var nameSpace = assembly
                   .CreateClass("TestClass​")
                   .Using("System")
                   .Public()
                   .Inheritance("InterfaceTest")
                   .Method(method => method
                     .Public()
                     .Name("ShowMethod")
                     .Param<string>("str")
                     .Body("return str+\" World!\";")
                     .Return<string>()).NamespaceScript;

                var result = assembly.GetAssembly();
                var type = assembly.GetTypeFromShortName("TestClass");

                //单独创建一个程序集​方法
               var func = FastMethodOperator.UseDomain(domain)
                  .Body(@"
TestClass obj = new TestClass​();
return obj.ShowMethod(arg);")
                  .Compile<Func<string, string>>();
                Assert.Equal("Hello World!", func("Hello"));
            }
            
        }
#endif
    }
}
