using Natasha;
using Xunit;

namespace NatashaUT
{
    public interface ITest {
        int MethodWidthReturnInt();
        string MethodWidthReturnString();
        void MethodWidthParamsRefInt(ref int i);
        string MethodWidthParamsString(string str);
        string MethodWidthParams(int a,string str,int b);
    }
    [Trait("快速构建","接口")]
    public class DynamicInterfaceTest
    {
        [Fact(DisplayName = "接口动态实现")]
        public void InterfaceSet()
        {
            OopOperator<ITest> interfaceBuilder = new OopOperator<ITest>();
            interfaceBuilder.ClassName("UTestClass");
            interfaceBuilder["MethodWidthReturnInt"] = "return 123456;";
            interfaceBuilder["MethodWidthReturnString"] = "return \"test\";";
            interfaceBuilder["MethodWidthParamsRefInt"] = "i+=10;";
            interfaceBuilder["MethodWidthParamsString"] = "return str+\"1\";";
            interfaceBuilder["MethodWidthParams"] = "return a.ToString()+str+b.ToString();";
            interfaceBuilder.Compile();
            var test = interfaceBuilder.Create("UTestClass");
            int testi = 1;
            test.MethodWidthParamsRefInt(ref testi);
            Assert.Equal(123456, test.MethodWidthReturnInt());
            Assert.Equal("test", test.MethodWidthReturnString());
            Assert.Equal(11, testi);
            Assert.Equal("test1", test.MethodWidthParamsString("test"));
            Assert.Equal("12test12", test.MethodWidthParams(12,"test",12));

        }
    }
}
