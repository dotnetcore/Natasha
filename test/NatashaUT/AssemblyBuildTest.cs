using Natasha;
using Natasha.Operator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace NatashaUT
{
    [Trait("程序集编译测试", "单域")]
    public class AssemblyBuildTest
    {
        [Fact(DisplayName = "多程序集协作")]
        public void Test1()
        {
            //ForTest1();
            Assert.Equal("HelloTest", ForTest1());
#if NETCOREAPP3_0
            for (int i = 0; i < 10; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Assert.True(DomainManagment.IsDeleted("SingleDomainAsmTest1"));
#endif

        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        internal string ForTest1()
        {
            var domain = DomainManagment.Create("SingleDomainAsmTest1");
            var assembly = domain.CreateAssembly("AsmTest1");

            var @interface = assembly
                .CreateInterface("IAsmT1")
                .Using("System")
                .OopAccess(AccessTypes.Public)
                .OopBody("string ShowMethod(string str);");


            var @class = assembly
                .CreateClass("ClassAsm")
                .Using("System")
                .OopAccess(AccessTypes.Public)
                .Inheritance("IAsmT1")
                .Method(method => method
                    .MemberAccess(AccessTypes.Public)
                    .Name("ShowMethod")
                    .Param<string>("str")
                    .Body("return str+AsmEnum.Test.ToString();")
                    .Return<string>()
                 );

            var @enum = assembly
                .CreateEnum("AsmEnum")
                .OopAccess(AccessTypes.Public)
                .EnumField("Test")
                .EnumField("Test1")
                .EnumField("Test2", 1);

            var result = assembly.Complier();
            var type = assembly.GetType("ClassAsm");

            var builder = FastMethodOperator.New;
            builder.Complier.Domain = domain;
            var @delegate = builder.Using(type).MethodBody(@"
            ClassAsm obj = new ClassAsm();
            return obj.ShowMethod(""Hello"");
            ").Complie<Func<string, string>>();
            DomainManagment.Get("SingleDomainAsmTest1").Dispose();
            return @delegate("hello");


        }




        [Fact(DisplayName = "多程序集覆盖")]
        public void Test2()
        {
            var domain = DomainManagment.Create("SingleDomainAsmTest2");
            var assembly = domain.CreateAssembly("AsmTest1");

            var @interface = assembly
                .CreateInterface("IAsmT1")
                .Using("System")
                .OopAccess(AccessTypes.Public)
                .OopBody("string ShowMethod(string str);");


            var @class = assembly
                .CreateClass("ClassAsm")
                .Using("System")
                .OopAccess(AccessTypes.Public)
                .Inheritance("IAsmT1")
                .Method(method => method
                    .MemberAccess(AccessTypes.Public)
                    .Name("ShowMethod")
                    .Param<string>("str")
                    .Body("return str+AsmEnum.Test.ToString();")
                    .Return<string>()
                 );


            var @enum = assembly
               .CreateEnum("AsmEnum")
               .OopAccess(AccessTypes.Public)
               .EnumField("Test")
               .EnumField("Test1")
               .EnumField("Test2", 1);


            var result = assembly.Complier();
            var type = assembly.GetType("ClassAsm");
            domain.RemoveType(type);


            assembly = domain.CreateAssembly("AsmTest2");
            @interface = assembly
                .CreateInterface("IAsmT1")
                .Using("System")
                .OopAccess(AccessTypes.Public)
                .OopBody("string ShowMethod(string str);");


            @class = assembly
                .CreateClass("ClassAsm")
                .Using("System")
                .OopAccess(AccessTypes.Public)
                .Inheritance("IAsmT1")
                .Method(method => method
                    .MemberAccess(AccessTypes.Public)
                    .Name("ShowMethod")
                    .Param<string>("str")
                    .Body("return str+AsmEnum.Test.ToString()+\"1\";")
                    .Return<string>()
                 );


            @enum = assembly
               .CreateEnum("AsmEnum")
               .OopAccess(AccessTypes.Public)
               .EnumField("Test")
               .EnumField("Test1")
               .EnumField("Test2", 1);


            result = assembly.Complier();
            type = assembly.GetType("ClassAsm");




            var builder = FastMethodOperator.New;
            builder.Complier.Domain = domain;
            var @delegate = builder.Using(type).MethodBody(@"
ClassAsm obj = new ClassAsm();
return obj.ShowMethod(""Hello"");
").Complie<Func<string, string>>();

            Assert.Equal("HelloTest1", @delegate("hello"));
#if NETCOREAPP3_0
            domain.Dispose();
            for (int i = 0; i < 10; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            Assert.True(DomainManagment.IsDeleted("SingleDomainAsmTest1"));
#endif

        }

#if NETCOREAPP3_0
        [Fact(DisplayName = "域锁与管理")]
        public void Test3()
        {
            using (DomainManagment.CreateAndLock("CDomain1"))
            {
                var domain = DomainManagment.CurrentDomain;
                Assert.Equal("CDomain1", domain.Name);
            }
        }

        [Fact(DisplayName = "HelloWorldTest")]
        public void TestHelloWorld()
        {

            using (DomainManagment.CreateAndLock("MyDomain"))
            {

                var domain = DomainManagment.CurrentDomain;
                var assembly = domain.CreateAssembly("MyAssembly");


                //创建一个接口
                assembly
                    .CreateInterface("InterfaceTest")
                    .Using("System")
                    .OopAccess(AccessTypes.Public)
                    .OopBody("string ShowMethod(string str);");


                //创建一个类并实现​接口
                assembly
                   .CreateClass("TestClass​")
                   .Using("System")
                   .OopAccess(AccessTypes.Public)
                   .Inheritance("InterfaceTest")
                   .Method(method => method
                     .MemberAccess(AccessTypes.Public)
                     .Name("ShowMethod")
                     .Param<string>("str")
                     .Body("return str+\" World!\";")
                     .Return<string>());

                var result = assembly.Complier();
                var type = assembly.GetType("TestClass");

                //单独创建一个程序集​方法
               var func = FastMethodOperator.New​
                  .Using(type)
                  .MethodBody(@"
TestClass obj = new TestClass​();
return obj.ShowMethod(arg);")
                  .Complie<Func<string, string>>();
                Assert.Equal("Hello World!", func("Hello"));
            }
            
        }
#endif
    }
}
