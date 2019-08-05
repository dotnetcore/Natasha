using Natasha;
using Natasha.Builder;
using System;
using Xunit;

namespace NatashaUT
{

    [Trait("字段构建", "")]
    public class FieldTest
    {

        [Fact(DisplayName = "静态字段1")]
        public void Test1()
        {
            FieldBuilder template = new FieldBuilder();
            var result = template
                .MemberAttribute("[Test]")
                .MemberAccess("public")
                .MemberModifier(Modifiers.Static)
                .FieldName("Name")
                .FieldType<string>()
                .Builder().Script;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                Assert.Equal("[Test]\npublic static String Name;", result);
            }
            else
            {
                Assert.Equal("[Test]\r\npublic static String Name;", result);
            }
            
        }



        [Fact(DisplayName = "静态字段2")]
        public void Test2()
        {
            FieldBuilder template = new FieldBuilder();
            var result = template
                .MemberAttribute("[Test][Test1]")
                .MemberAccess(AccessTypes.Public)
                .MemberModifier(Modifiers.Static)
                .FieldName("Age")
                .FieldType(typeof(int))
                .Builder().Script;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                Assert.Equal("[Test][Test1]\npublic static Int32 Age;", result);
            }
            else
            {
                Assert.Equal("[Test][Test1]\r\npublic static Int32 Age;", result);
            }
            
        }




        [Fact(DisplayName = "普通字段1")]
        public void Test3()
        {
            FieldBuilder template = new FieldBuilder();
            var result = template
                .MemberAttribute("[Test]")
                .MemberAccess("public")
                .FieldName("Name")
                .FieldType<string>()
                .Builder().Script;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                Assert.Equal("[Test]\npublic String Name;", result);
            }
            else
            {
                Assert.Equal("[Test]\r\npublic String Name;", result);
            }
            
        }



        [Fact(DisplayName = "普通字段2")]
        public void Test4()
        {
            FieldBuilder template = new FieldBuilder();
            var result = template
                .MemberAttribute<ClassDataAttribute>()
                .MemberAccess("public")
                .FieldName("Name")
                .FieldType<string>()
                .Builder().Script;

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                Assert.Equal("[ClassDataAttribute]\npublic String Name;", result);
            }
            else
            {
                Assert.Equal("[ClassDataAttribute]\r\npublic String Name;", result);
            }
            
        }

    }
}
