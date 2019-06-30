using Natasha;
using System;
using Xunit;

namespace NatashaUT
{
    [Trait("动态调用1", "普通类")]
    public class DynamicEntityCallTest
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
            Type type = RuntimeComplier.GetType(text);
            //创建动态类实例代理
            var instance = EntityOperator.Create(type);
            instance.New();
            //Get动态调用
            Assert.Equal("111", instance["Name"].Get<string>());
            //调用动态委托赋值
            instance.Set("Name", "222");

            Assert.Equal("222", instance["Name"].Get<string>());
           
        }



        [Fact(DisplayName = "普通类的动态操作测试")]
        public void TestCall2()
        {
            //创建动态类实例代理
            var instance = EntityOperator<TestB>.Create();
            instance.New();
            Assert.Equal("111", instance["Name"].Get<string>());

            //调用动态委托赋值
            instance.Set("Name", "222");

            Assert.Equal("222", instance["Name"].Get<string>());
        }



        [Fact(DisplayName = "复杂类的动态操作测试")]
        public void TestCall3()
        {
            //创建动态类实例代理
            var instance = EntityOperator<TestB>.Create();
            instance.New();
            Assert.Equal("111", instance["Name"].Get<string>());

            //调用动态委托赋值
            instance.Set("Name", "222");

            Assert.Equal("222", instance["Name"].Get<string>());


            var c = instance["InstanceC"].Get<TestC>();
            Assert.Equal("abc", c.Name);


            instance["InstanceC"].Set(new TestC() { Name="bbca"});
            Assert.Equal("bbca", instance["InstanceC"].Get<TestC>().Name);

          
        }
    }
}
