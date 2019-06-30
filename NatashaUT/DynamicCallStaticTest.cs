using Natasha;
using NatashaUT.Model;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("动态调用2", "静态类")]
    public class DynamicCallStaticTest
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
    public static class StaticTest
    {
        static StaticTest(){
            Name=""111"";
        }

        public static string Name;
        public static int Age{get;set;}
    }
}";
            //根据脚本创建动态类
            Type type = RuntimeComplier.GetType(text);
            //创建动态类实例代理
            DynamicStaticOperator instance = type;
            //Get动态调用
            Assert.Equal("111", instance["Name"].StringValue);
            //调用动态委托赋值
            instance["Name"].StringValue = "222";

            Assert.Equal("222", instance["Name"].StringValue);

        }



        [Fact(DisplayName = "运行时静态类的动态操作测试")]
        public void TestCall2()
        {
            //创建动态类实例代理
            DynamicStaticOperator instance = typeof(StaticTestModel);
            StaticTestModel.Name = "111";
            Assert.Equal("111", instance["Name"].StringValue);
            instance["Name"].StringValue = "222";
            Assert.Equal("222", instance["Name"].StringValue);
            StaticTestModel.Age = 1001;
            Assert.Equal(1001, instance["Age"].IntValue);
            StaticTestModel.Temp = DateTime.Now;
            instance["Temp"].DateTimeValue = StaticTestModel.Temp;
            Assert.Equal(StaticTestModel.Temp, instance["Temp"].DateTimeValue);

        }

        [Fact(DisplayName = "运行时伪静态类的动态操作测试")]
        public void TestCall3()
        {
            //创建动态类实例代理
            DynamicStaticOperator instance = typeof(FakeStaticTestModel);
            FakeStaticTestModel.Name = "111";
            Assert.Equal("111", instance["Name"].StringValue);
            instance["Name"].StringValue = "222";
            Assert.Equal("222", instance["Name"].StringValue);
            FakeStaticTestModel.Age = 1001;
            Assert.Equal(1001, instance["Age"].IntValue);
            FakeStaticTestModel.Temp = DateTime.Now;
            instance["Temp"].DateTimeValue = FakeStaticTestModel.Temp;
            Assert.Equal(FakeStaticTestModel.Temp, instance["Temp"].DateTimeValue);

        }
    }
}
