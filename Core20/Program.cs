using Natasha;
using System;

namespace Core20
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             *   在此之前，你需要右键，选择工程文件，在你的.csproj里面 
             *   
             *   写上这样一句浪漫的话： 
             *   
             *      <PreserveCompilationContext>true</PreserveCompilationContext>
             */
          

            string text = @"namespace HelloWorld
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
            DynamicOperator instance = type;

            if (instance["Name"].StringValue == "111")
            {
                //调用动态委托赋值
                instance["Name"].StringValue = "222";
            }
            //调用动态类
            Console.WriteLine(instance["Name"].StringValue);

            TestB b = new TestB();
            b.Name = "abc";
            var result = CloneOperator.Clone(b);


            //创建动态类实例代理
            DynamicOperator<TestB> instance2 = new DynamicOperator<TestB>();

            if (instance2["Name"].StringValue == "111")
            {
                //调用动态委托赋值
                instance2["Name"].StringValue = "222";
            }
            //调用动态类
            Console.WriteLine(instance2["Name"].StringValue);


            Console.ReadKey();
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
