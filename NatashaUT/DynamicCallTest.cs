using Natasha;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NatashaUT
{
  
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
            DynamicInstance<object> instance = new DynamicInstance<object>(type);
            Assert.Equal("111", instance.Get("Name").StringValue);

            //设置值
            instance.StringValue = "222";
            //调用动态委托赋值
            instance.Set("Name");
            Assert.Equal("222", instance.Get("Name").StringValue);
           
        }

        [Fact(DisplayName = "普通类的动态操作测试")]
        public void TestCall2()
        {

            //创建动态类实例代理
            DynamicInstance<TestB> instance = new DynamicInstance<TestB>();
            Assert.Equal("111", instance.Get("Name").StringValue);
            //设置值
            instance.StringValue = "222";
            //调用动态委托赋值
            instance.Set("Name");
            Assert.Equal("222", instance.Get("Name").StringValue);

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
