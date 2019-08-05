using Natasha;
using Xunit;

namespace NatashaUT
{
    [Trait("快速构建", "类/结构体/接口")]
    public class DynamicBuilderTest
    {

        [Fact(DisplayName = "Builder测试1")]
        public void TestBuilder1()
        {
            OopBuilder builder = new OopBuilder();
            var script = builder
                .Using<DynamicBuilderTest>()
                .Namespace("TestNamespace")
                .OopAccess(AccessTypes.Public)
                .OopModifier(Modifiers.Static)
                .OopName("TestUt1")
                .OopBody(@"public static void Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder()
                .Script;

            Assert.Equal(@"using NatashaUT;using System;namespace TestNamespace{public static class TestUt1{public static String Name;private static Int32 _age;public static void Test(){}}}", script);
            Assert.Equal("TestUt1", builder.GetType().Name);
        }



        [Fact(DisplayName = "Builder测试2")]
        public void TestBuilder2()
        {
            OopBuilder builder = new OopBuilder();
            var script = builder
                .Namespace("TestNamespace")
                .ChangeToStruct()
                .OopAccess(AccessTypes.Private)
                .OopName("TestUt2")
                .OopBody(@"public static void Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder().Script;

            Assert.Equal(@"using System;namespace TestNamespace{private struct TestUt2{public static String Name;private static Int32 _age;public static void Test(){}}}", script);
        }



        [Fact(DisplayName = "Builder测试3")]
        public void TestBuilder3()
        {
            OopBuilder builder = new OopBuilder();
            var script = builder
                .Namespace<string>()
                .OopAccess("")
                .OopName("TestUt3")
                .ChangeToInterface()
                .Ctor(item=>item
                    .MemberAccess("public")
                    .Param<string>("name")
                    .Body("this.Name=name;"))
                .OopBody(@"public static void Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder().Script;

            Assert.Equal(@"using System;namespace System{interface TestUt3{public static String Name;private static Int32 _age;public static void Test(){}public TestUt3(String name){this.Name=name;}}}", script);
        }

    }

}
