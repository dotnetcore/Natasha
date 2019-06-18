using Natasha;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("构建类", "遍历")]
    public class DynamicCallTest
    {
        [Fact(DisplayName = "动态类的动态操作测试")]
        public void TestCall1()
        {
            //ScriptComplier.Init();
            string text = @"using System;
using System.Collections;
using System.Linq;
using System.Text;
 
namespace HelloWorld
{
    public class Test
    {
        public Test(){
            Name=""111"";
        }

        public string Name;
        public int Age{get;set;}
    }
}";
            //根据脚本创建动态类
            Type type = ClassBuilder.GetType(text);
            //创建动态类实例代理
            DynamicOperatorBase instance = DynamicOperator.GetOperator(type);
            //Get动态调用
            Assert.Equal("111", instance["Name"].StringValue);
            //调用动态委托赋值
            instance["Name"].StringValue = "222";

            Assert.Equal("222", instance["Name"].StringValue);
           
        }



        [Fact(DisplayName = "普通类的动态操作测试")]
        public void TestCall2()
        {
            //创建动态类实例代理
            DynamicOperator<TestB> instance = new DynamicOperator<TestB>();
            Assert.Equal("111", instance["Name"].StringValue);

            //调用动态委托赋值
            instance["Name"].StringValue = "222";

            Assert.Equal("222", instance["Name"].StringValue);

        }
    }
    public class TestB
    {
        public TestB()
        {
            Name = "111";
        }
        public string Name { get; set; }
        public int Age;
    }
}
