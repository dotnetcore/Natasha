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
            Assert.True(AssemblyManagment.IsDeleted("SingleDomainAsmTest1"));
#endif

        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        internal string ForTest1()
        {
            var domain = AssemblyManagment.Create("SingleDomainAsmTest1");
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
            AssemblyManagment.Get("SingleDomainAsmTest1").Dispose();
            return @delegate("hello");


        }




        [Fact(DisplayName = "多程序集覆盖")]
        public void Test2()
        {
            var domain = AssemblyManagment.Create("SingleDomainAsmTest2");
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

            Assert.True(AssemblyManagment.IsDeleted("SingleDomainAsmTest1"));
#endif

        }
    }
}
