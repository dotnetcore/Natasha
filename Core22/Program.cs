using Natasha;
using Natasha.Caller;
using System;
using System.Diagnostics;
using Natasha.Method;
using System.Reflection.Emit;

namespace Core22
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }
            var action = FastMethodOperator.New
                .MethodBody("return 1+1;")
                .Return<int>()
                .Complie<Func<int>>();

            action();

            //FakeMethodOperator.New
            //    .UseMethod<Program>("Main")
            //    .StaticMethodContent($@"Console.WriteLine(""Hello World!"")")
            //    .Complie<Action<string[]>>();

            Console.WriteLine(FakeMethodOperator.New
               .UseMethod<TestB>("TestMethod")
               .StaticMethodContent($@"Console.WriteLine(""Hello World!"")")
               .Builder()
               );


            /*
             *   在此之前，你需要右键，选择工程文件，在你的.csproj里面 
             *   
             *   写上这样一句浪漫的话： 
             *   
             *      <PreserveCompilationContext>true</PreserveCompilationContext>
             */

            OopOperator<TestAbstract> abstractBuilder = new OopOperator<TestAbstract>();
            abstractBuilder.ClassName("UTestClass");
            abstractBuilder["GetName"] = "return Name;";
            abstractBuilder["GetAge"] = "return Age;";
            abstractBuilder.Compile();
            var test = abstractBuilder.Create("UTestClass");

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
            Type type = RuntimeComplier.GetType(text);


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


            var entity = EntityOperator.Create(type);
            entity.New();
            stopwatch.Restart();
            if (entity["Name"].Get<string>() == "111")
            {
                //调用动态委托赋值
                entity["Name"].Set("222");
            }
            stopwatch.Stop();
            entity.Get("Instance").Set("Name", "haha");
            Console.WriteLine(entity.Get("Instance").Get<string>("Name"));
            Console.WriteLine(stopwatch.Elapsed);

            entity = EntityOperator.Create(type);
            entity.New();
            stopwatch.Restart();
            if (entity["Name"].Get<string>() == "111")
            {
                //调用动态委托赋值
                entity["Name"].Set("222");
            }
            stopwatch.Stop();
            Console.WriteLine("new:" + stopwatch.Elapsed);


            for (int j = 0; j < 20; j++)
            {
                Console.WriteLine("===========================");
                //stopwatch.Restart();
                //for (int i = 0; i < 50000; i++)
                //{
                //    newInstance = new DynamicOperator(type);

                //    if (newInstance["Name"].StringValue == "111")
                //    {
                //        //调用动态委托赋值
                //        newInstance["Name"].StringValue = "222";
                //    }
                //}
                //stopwatch.Stop();
                //Console.WriteLine("试验 DynamicOperator:\t" + stopwatch.Elapsed);

                //stopwatch.Restart();
                //for (int i = 0; i < 50000; i++)
                //{
                //    entity = EntityOperator.Create(type);
                //    entity.New();

                //    if (entity["Name"].Get<string>() == "111")
                //    {
                //        //调用动态委托赋值
                //        entity["Name"].Set("222");
                //    }
                //}
                //stopwatch.Stop();
                //Console.WriteLine("试验 EntityOperator:\t" + stopwatch.Elapsed);


                //stopwatch.Restart();
                //for (int i = 0; i < 50000; i++)
                //{
                //    newInstance = DynamicOperator.GetOperator(type);

                //    if (newInstance["Name"].StringValue == "111")
                //    {
                //        //调用动态委托赋值
                //        newInstance["Name"].StringValue = "222";
                //    }
                //}
                //stopwatch.Stop();
                //Console.WriteLine("试验 DynamicOperator<>:\t" + stopwatch.Elapsed);



                stopwatch.Restart();
                for (int i = 0; i < 50000; i++)
                {
                    var tEntity = new TestB();
                    if (tEntity.Name == "111")
                    {
                        //调用动态委托赋值
                        tEntity.Name = "222";
                    }
                }
                stopwatch.Stop();
                Console.WriteLine("原生调用:\t\t" + stopwatch.Elapsed);




                stopwatch.Restart();
                for (int i = 0; i < 50000; i++)
                {
                    entity = EntityOperator.Create(typeof(TestB));
                    entity.New();

                    if (entity.Get<string>("Name") == "111")
                    {
                        //调用动态委托赋值
                        entity.Set("Name", "222");
                    }
                }
                stopwatch.Stop();
                Console.WriteLine("Natasha EntityOperator:\t" + stopwatch.Elapsed);


                stopwatch.Restart();
                for (int i = 0; i < 50000; i++)
                {
                    RunDynamic(new TestB());
                }
                stopwatch.Stop();
                Console.WriteLine("Dynamic :\t\t" + stopwatch.Elapsed);


                stopwatch.Restart();
                for (int i = 0; i < 50000; i++)
                {
                    var tEntity = (new TestB()).Caller();
                    if (tEntity.Get<string>("Name") == "111")
                    {
                        //调用动态委托赋值
                        tEntity.Set("Name", "222");
                    }
                }
                stopwatch.Stop();
                Console.WriteLine("Natasha Extension:\t" + stopwatch.Elapsed);
                Console.WriteLine("===========================");
            }
            /*
            //创建动态类实例代理
            DynamicOperator<TestB> instance2 = new DynamicOperator<TestB>();

            if (instance2["Name"].StringValue == "111")
            {
                //调用动态委托赋值
                instance2["Name"].StringValue = "222";
            }
            //调用动态类
            Console.WriteLine(instance2["Name"].StringValue);

            var temp = StaticEntityOperator.Create(typeof(StaticTestB));

            temp.Set("Name", "Name");
            temp.Set("Age", 1);

            Console.WriteLine(temp.Get<string>("Name"));
            Console.WriteLine(temp.Get<int>("Age"));
            Console.WriteLine(StaticTestB.Name);
            Console.WriteLine(StaticTestB.Age);
            */
            DynamicMethod method = new DynamicMethod("GetString", null, new Type[] { typeof(TestB), typeof(string) });
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, typeof(TestB).GetField("Name"));
            il.Emit(OpCodes.Ret);
            var emitAction = (Action<TestB, string>)(method.CreateDelegate(typeof(Action<TestB, string>)));

            var roslynAction = FastMethodOperator.New
                .Param<TestB>("instance")
                .Param<string>("value")
                .MethodBody("instance.Name = value;")
                .Return()
                .Complie<Action<TestB, string>>();


            stopwatch.Restart();
            for (int i = 0; i < 50000; i++)
            {
                var tEntity = new TestB();
                roslynAction(tEntity, "abc");
            }
            stopwatch.Stop();
            Console.WriteLine("Roslyn:\t" + stopwatch.Elapsed);

            stopwatch.Restart();
            for (int i = 0; i < 50000; i++)
            {
                var tEntity = new TestB();
                emitAction(tEntity,"abc");
            }
            stopwatch.Stop();
            Console.WriteLine("Emit:\t" + stopwatch.Elapsed);


            stopwatch.Restart();
            for (int i = 0; i < 50000; i++)
            {
                var tEntity = new TestB();
                roslynAction(tEntity, "abc");
            }
            stopwatch.Stop();
            Console.WriteLine("Roslyn:\t" + stopwatch.Elapsed);


            stopwatch.Restart();
            for (int i = 0; i < 50000; i++)
            {
                var tEntity = new TestB();
                emitAction(tEntity, "abc");
            }
            stopwatch.Stop();
            Console.WriteLine("Emit:\t" + stopwatch.Elapsed);

            Console.ReadKey();
        }

        public static void RunDynamic(dynamic tEntity)
        {
            if (tEntity.Name == "111")
            {
                //调用动态委托赋值
                tEntity.Name = "222";
            }
        }
    }
    public class TestB
    {
        public TestB()
        {
            Name = "111";
        }
        public string Name;
        public int Age;
        public void TestMethod()
        {

        }
    }

    public static class StaticTestB
    {
        public static string Name;
        public static int Age;
    }
}