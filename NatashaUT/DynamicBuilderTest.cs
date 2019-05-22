using Natasha;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT
{
    public class DynamicBuilderTest
    {
        [Fact(DisplayName = "Builder测试1")]
        public void TestBuilder1()
        {
            ClassBuilder builder = new ClassBuilder();
            var script = builder
                .Using<DynamicBuilderTest>()
                .Namespace("TestNamespace")
                .Public().Static().ClassName("TestUt1")
                .Body(@"public static void Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder();

            Assert.Equal(@"using NatashaUT;using System;namespace TestNamespace{public static class TestUt1{public static String Name;private static Int32 _age;public static void Test(){}}}", script);
        }
        [Fact(DisplayName = "Builder测试2")]
        public void TestBuilder2()
        {
            ClassBuilder builder = new ClassBuilder();
            var script = builder
                .Namespace("TestNamespace")
                .Private().Abstract().ClassName("TestUt2")
                .Body(@"public static void Test(){}")
                .PublicStaticField<string>("Name")
                .PrivateStaticField<int>("_age")
                .Builder();

            Assert.Equal(@"using System;namespace TestNamespace{private abstract class TestUt2{public static String Name;private static Int32 _age;public static void Test(){}}}", script);
        }
    }
}
