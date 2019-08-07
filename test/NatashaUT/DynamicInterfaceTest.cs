using Natasha;
using NatashaUT.Model;
using System;
using Xunit;

namespace NatashaUT
{
    
    [Trait("快速构建","代理")]
    public class DynamicInterfaceTest
    {
        [Fact(DisplayName = "接口动态实现")]
        public void InterfaceSet()
        {

            ProxyOperator<ITest> builder = new ProxyOperator<ITest>();
            builder.OopName("ITestClass");
            builder["MethodWidthReturnInt"] = "return 123456;";
            builder["MethodWidthReturnString"] = "return \"test\";";
            builder["MethodWidthParamsRefInt"] = "i+=10;";
            builder["MethodWidthParamsString"] = "return str+\"1\";";
            builder["MethodWidthParams"] = "b=10;return a.ToString()+str+b.ToString();";
            builder.Compile();
            var test = builder.Create("ITestClass");
            int testi = 1;
            int param3;
            test.MethodWidthParamsRefInt(ref testi);
            Assert.Equal("ITestClass", builder.TargetType.GetDevelopName());
            Assert.Equal(123456, test.MethodWidthReturnInt());
            Assert.Equal("test", test.MethodWidthReturnString());
            Assert.Equal(11, testi);
            Assert.Equal("test1", test.MethodWidthParamsString("test"));
            Assert.Equal("12test10", test.MethodWidthParams(12,"test",out param3));
            Assert.Equal(10, param3);

        }




        [Fact(DisplayName = "抽象类动态实现")]
        public void AbstractSet()
        {

            ProxyOperator<ATest> builder = new ProxyOperator<ATest>();
            builder.OopName("ATestClass");
            builder["MethodWidthReturnInt"] = "return 123456;";
            builder["MethodWidthReturnString"] = "return \"test\";";
            builder["MethodWidthParamsRefInt"] = "i+=10;";
            builder["MethodWidthParamsString"] = "return str+\"1\";";
            builder["MethodWidthParams"] = "b=10;return a.ToString()+str+b.ToString();";
            builder.Compile();
            var test = builder.Create("ATestClass");
            int testi = 1;
            int param3;
            test.MethodWidthParamsRefInt(ref testi);
            Assert.Equal("ATestClass", builder.TargetType.GetDevelopName());
            Assert.Equal(123456, test.MethodWidthReturnInt());
            Assert.Equal("test", test.MethodWidthReturnString());
            Assert.Equal(11, testi);
            Assert.Equal("test1", test.MethodWidthParamsString("test"));
            Assert.Equal("12test10", test.MethodWidthParams(12, "test", out param3));
            Assert.Equal(10, param3);

        }




        [Fact(DisplayName = "虚方法动态实现")]
        public void VirtualSet()
        {

            ProxyOperator<VTest> builder = new ProxyOperator<VTest>();
            builder.OopName("VTestClass");
            builder["MethodWidthReturnInt"] = "return 123456;";
            builder["MethodWidthReturnString"] = "return \"test\";";
            builder["MethodWidthParamsRefInt"] = "i+=10;";
            builder["MethodWidthParamsString"] = "return str+\"1\";";
            builder["MethodWidthParams"] = "b=10;return a.ToString()+str+b.ToString();";
            builder.Compile();
            var test = builder.Create("VTestClass");
            int testi = 1;
            int param3;
            test.MethodWidthParamsRefInt(ref testi);
            Assert.Equal("VTestClass", builder.TargetType.GetDevelopName());
            Assert.Equal(123456, test.MethodWidthReturnInt());
            Assert.Equal("test", test.MethodWidthReturnString());
            Assert.Equal(11, testi);
            Assert.Equal("test1", test.MethodWidthParamsString("test"));
            Assert.Equal("12test10", test.MethodWidthParams(12, "test", out param3));
            Assert.Equal(10, param3);

        }




        [Fact(DisplayName = "普通类动态实现")]
        public void NormalSet()
        {

            ProxyOperator<NTest> builder = new ProxyOperator<NTest>();
            builder.OopName("NTestClass");
            builder["MethodWidthReturnInt"] = "return 123456;";
            builder["MethodWidthReturnString"] = "return \"test\";";
            builder["MethodWidthParamsRefInt"] = "i+=10;";
            builder["MethodWidthParamsString"] = "return str+\"1\";";
            builder["MethodWidthParams"] = "b=10;return a.ToString()+str+b.ToString();";
            builder.Compile();
            dynamic test = Activator.CreateInstance(builder.TargetType);
            int testi = 1;
            int param3;
            test.MethodWidthParamsRefInt(ref testi);
            Assert.Equal("NTestClass", builder.TargetType.GetDevelopName());
            Assert.Equal(123456, test.MethodWidthReturnInt());
            Assert.Equal("test", test.MethodWidthReturnString());
            Assert.Equal(11, testi);
            Assert.Equal("test1", test.MethodWidthParamsString("test"));
            Assert.Equal("12test10", test.MethodWidthParams(12, "test", out param3));
            Assert.Equal(10, param3);

        }




    }
}
