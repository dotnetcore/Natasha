using Natasha;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT
{
    public class DynamicCtorTest
    {
        [Fact(DisplayName ="公有初始化函数")]
        public void TestCtor1()
        {
            CtorTemplate ctor = new CtorTemplate();
            string result = ctor.Name("Test")
                .Access(AccessTypes.Public)
                .Param<string>("initString")
                .Body("this.connection = initString;")
                .Builder();

            Assert.Equal("public Test(String initString){this.connection = initString;}", result);
        }
        [Fact(DisplayName = "私有初始化函数")]
        public void TestCtor2()
        {
            CtorTemplate ctor = new CtorTemplate();
            string result = ctor.Name("Test")
                .Access(AccessTypes.Private)
                .Body("this.connection = initString;")
                .Builder();

            Assert.Equal("private Test(){this.connection = initString;}", result);
        }
        [Fact(DisplayName = "静态初始化函数")]
        public void TestCtor3()
        {
            CtorTemplate ctor = new CtorTemplate();
            string result = ctor.Name("Test")
                .Modifier(Modifiers.Static)
                .Body("this.connection = initString;")
                .Builder();

            Assert.Equal("static Test(){this.connection = initString;}", result);
        }
    }
}
