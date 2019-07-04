using Natasha;
using Xunit;

namespace NatashaUT
{
    [Trait("快速构建","初始化函数")]
    public class DynamicCtorTest
    {

        [Fact(DisplayName ="公有初始化函数")]
        public void TestCtor1()
        {
            CtorTemplate ctor = new CtorTemplate();
            string result = ctor.Name("Test")
                .MemberAccess(AccessTypes.Public)
                .Param<string>("initString")
                .Body("this.connection = initString;")
                .Builder().Script;

            Assert.Equal("public Test(String initString){this.connection = initString;}", result);
        }



        [Fact(DisplayName = "私有初始化函数")]
        public void TestCtor2()
        {
            CtorTemplate ctor = new CtorTemplate();
            string result = ctor.Name("Test")
                .MemberAccess(AccessTypes.Private)
                .Body("this.connection = initString;")
                .Builder().Script;

            Assert.Equal("private Test(){this.connection = initString;}", result);
        }



        [Fact(DisplayName = "静态初始化函数")]
        public void TestCtor3()
        {
            CtorTemplate ctor = new CtorTemplate();
            string result = ctor.Name("Test")
                .MemberModifier(Modifiers.Static)
                .Body("this.connection = initString;")
                .Builder().Script;

            Assert.Equal("static Test(){this.connection = initString;}", result);
        }
    }
}
