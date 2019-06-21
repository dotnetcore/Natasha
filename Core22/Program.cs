using Natasha;
using System;
using System.Diagnostics;

namespace Core22
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

           var delegate2 = DelegateOperator<GetterDelegate>.Create("return value.ToString();");
            Console.WriteLine(delegate2(1));
           var delegate3 = "return value.ToString();".Create<GetterDelegate>();
            var delegateConvt = FastMethodOperator.New
                .Param<string>("value")
                .MethodBody($@"return value==""true"" || value==""mama"";")
                .Return<bool>()
                .Complie<Func<string, bool>>();

            Console.WriteLine(delegateConvt("mama"));

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
            Instance = new Test1();
        }

        public string Name;
        public int Age{get;set;}
        public Test1 Instance;
    }
    public class Test1{
         public string Name=""1"";
    }
}";
            //根据脚本创建动态类
            Type type = ClassBuilder.GetType(text);
            //创建动态类实例代理
            DynamicOperator instance = new DynamicOperator(type);

            if (instance["Name"].StringValue == "111")
            {
                //调用动态委托赋值
                instance["Name"].StringValue = "222";
            }
            Console.WriteLine("===");
            Console.WriteLine(instance["Instance"].OperatorValue["Name"].StringValue); 
            //调用动态类
            Console.WriteLine(instance["Name"].StringValue);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (instance["Name"].StringValue == "111")
            {
                //调用动态委托赋值
                instance["Name"].StringValue = "222";
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);


            var newInstance = DynamicOperator.GetOperator(type);
            stopwatch.Restart();
            if (newInstance["Name"].StringValue == "111")
            {
                //调用动态委托赋值
                newInstance["Name"].StringValue = "222";
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);

            newInstance = DynamicOperator.GetOperator(type);
            stopwatch.Restart();
            if (newInstance["Name"].StringValue == "111")
            {
                //调用动态委托赋值
                newInstance["Name"].StringValue = "222";
            }
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed);




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