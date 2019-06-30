using Natasha;
using NatashaUT.Model;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("动态调用1", "静态类")]
    public class DynamicStaticEntityCallTest
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
    public static class StaticTest1
    {
        static StaticTest1(){
            Name=""111"";
        }

        public static string Name;
        public static int Age{get;set;}
    }
}";
            //根据脚本创建动态类
            Type type = RuntimeComplier.GetType(text);
            //创建动态类实例代理
            var instance =StaticEntityOperator.Create(type);
            //Get动态调用
            Assert.Equal("111", instance["Name"].Get<string>());
            //调用动态委托赋值
            instance["Name"].Set("222");

            Assert.Equal("222", instance.Get<string>("Name"));

        }



        [Fact(DisplayName = "运行时静态类的动态操作测试")]
        public void TestCall2()
        {
            //创建动态类实例代理
            var instance = StaticEntityOperator.Create(typeof(StaticTestModel1));
            StaticTestModel1.Name = "111";
            Assert.Equal("111", instance["Name"].Get<string>());
            instance["Name"].Set("222");
            Assert.Equal("222", instance["Name"].Get<string>());
            StaticTestModel1.Age = 1001;
            Assert.Equal(1001, instance.Get<int>("Age"));
            StaticTestModel1.Temp = DateTime.Now;
            instance["Temp"].Set(StaticTestModel1.Temp);
            Assert.Equal(StaticTestModel1.Temp, instance["Temp"].Get<DateTime>());

        }

        [Fact(DisplayName = "运行时伪静态类的动态操作测试")]
        public void TestCall3()
        {
            //创建动态类实例代理
            var instance = StaticEntityOperator.Create(typeof(FakeStaticTestModel1));
            FakeStaticTestModel1.Name = "111";
            Assert.Equal("111", instance["Name"].Get<string>());
            instance["Name"].Set("222");
            Assert.Equal("222", instance["Name"].Get<string>());
            FakeStaticTestModel1.Age = 1001;
            Assert.Equal(1001, instance.Get<int>("Age"));
            FakeStaticTestModel1.Temp = DateTime.Now;
            instance["Temp"].Set(FakeStaticTestModel1.Temp);
            Assert.Equal(FakeStaticTestModel1.Temp, instance["Temp"].Get<DateTime>());

        }
    }
}
