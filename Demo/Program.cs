using Natasha;
using System;

namespace Demo
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
            /*ScriptBuilder maker = new ScriptBuilder();

            var delegateAction = maker
                .Namespace(typeof(Console))
                .Param<string>("str1")
                .Param<string>("str2")
                .Body(@"
                    string result = str1 +"" ""+ str2;
                    Console.WriteLine(result);
                                               ")
                .Return()
                .Create();

            ((Action<string, string>)delegateAction)("Hello", "World!");

            ScriptBuilder maker2 = new ScriptBuilder();
            var delegateAction2 = maker2
                .Namespace(typeof(Console))
                .Param<string>("str1")
                .Param<string>("str2")
                .Body(@"
                    string result = str1 +"" ""+ str2;
                    Console.WriteLine(result);
                                               ")
                .Return()
                .Create<Action<string, string>>();

            delegateAction2("Hello", "World!");
            */
            ScriptComplier.Init();
            ClassBuilder classBuilder = new ClassBuilder();
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
            Type type = classBuilder.GetType(text);
            //创建动态类实例代理
            DynamicInstance instance = new DynamicInstance(type);

            if (instance.Get("Name").StringValue=="111")
            {
                //设置值
                instance.StringValue = "222";
                //调用动态委托赋值
                instance.Set("Name");
            }
            //调用动态类
            Console.WriteLine(instance.Get("Name").StringValue);
            Console.WriteLine(type.Name);
            Console.ReadKey();
        }

    }
}
