using Natasha;
using Xunit;

namespace NatashaUT
{
    [Trait("快速构建", "类")]
    public class DynamicBuilderTest
    {

        [Fact(DisplayName = "Builder测试1")]
        public void TestBuilder1()
        {
            ClassBuilder builder = new ClassBuilder();
            var script = builder
                .Using<DynamicBuilderTest>()
                .Namespace("TestNamespace")
                .ClassAccess(AccessTypes.Public)
                .ClassModifier(Modifiers.Static)
                .ClassName("TestUt1")
                .ClassBody(@"public static void Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder()
                .Script;

            Assert.Equal(@"using NatashaUT;using System;namespace TestNamespace{public static class TestUt1{public static String Name;private static Int32 _age;public static void Test(){}}}", script);
        }



        [Fact(DisplayName = "Builder测试2")]
        public void TestBuilder2()
        {
            ClassBuilder builder = new ClassBuilder();
            var script = builder
                .Namespace("TestNamespace")
                .ClassAccess(AccessTypes.Private)
                .ClassModifier(Modifiers.Abstract)
                .ClassName("TestUt2")
                .ClassBody(@"public static void Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder().Script;

            Assert.Equal(@"using System;namespace TestNamespace{private abstract class TestUt2{public static String Name;private static Int32 _age;public static void Test(){}}}", script);
        }
    }
}
