using Natasha;
using System;

namespace Core21
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
            var delegateAction = MethodBuilder.NewMethod
                .Using(typeof(Console))
                .Param<string>("str1")
                .Param<string>("str2")
                .Body(@"
                    string result = str1 +"" ""+ str2;
                    Console.WriteLine(result);
                                               ")
                .Return()
                .Create();

            ((Action<string, string>)delegateAction)("Hello", "World!");

            var delegateAction2 = MethodBuilder.NewMethod
                .Using(typeof(Console))
                .Param<string>("str1")
                .Param<string>("str2")
                .Body(@"
                    string result = str1 +"" ""+ str2;
                    Console.WriteLine(result);
                                               ")
                .Return()
                .Create<Action<string, string>>();

            delegateAction2("Hello", "World!");

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
            DynamicInstance instance = new DynamicInstance(type);

            if (instance["Name"].StringValue == "111")
            {
                //调用动态委托赋值
                instance["Name"].StringValue = "222";
            }
            //调用动态类
            Console.WriteLine(instance["Name"].StringValue);




            //创建动态类实例代理
            DynamicInstance<TestB> instance2 = new DynamicInstance<TestB>();

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
