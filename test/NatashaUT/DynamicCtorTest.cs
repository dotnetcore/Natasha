using Natasha;
using Natasha.Builder;
using Natasha.Template;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("快速构建","初始化函数")]
    public class DynamicCtorTest
    {

        [Fact(DisplayName ="公有初始化函数")]
        public void TestCtor1()
        {
            CtorBuilder ctor = new CtorBuilder();
            string result = ctor.Name("Test")
                .PublicMember
                .Param<string>("initString")
                .Body("this.connection = initString;")
                .Script;

            Assert.Equal($"public Test(String initString){{{Environment.NewLine}this.connection = initString;}}", result);
        }



        [Fact(DisplayName = "私有初始化函数")]
        public void TestCtor2()
        {
            CtorBuilder ctor = new CtorBuilder();
            string result = ctor.Name("Test")
                .PrivateMember
                .Body("this.connection = initString;")
                .Script;

            Assert.Equal($"private Test(){{{Environment.NewLine}this.connection = initString;}}", result);
        }



        [Fact(DisplayName = "静态初始化函数")]
        public void TestCtor3()
        {
            CtorBuilder ctor = new CtorBuilder();
            string result = ctor.Name("Test")
                .StaticMember
                .Body("this.connection = initString;")
                .Script;

            Assert.Equal($"static Test(){{{Environment.NewLine}this.connection = initString;}}", result);
        }
    }
}
