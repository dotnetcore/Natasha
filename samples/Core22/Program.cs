using Natasha.CSharp;
using System;
using System.Diagnostics;
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
            var action = FastMethodOperator.DefaultDomain()
                .Body("return 1+1;")
                .Return<int>()
                .Compile<Func<int>>();

            action();


            FakeMethodOperator.RandomDomain(item=> {
                item.AssemblyName = "1";
                item.OutputToFile = true;
                })
               .UseMethod<TestB>("TestMethod")
               .StaticMethodBody($@"Console.WriteLine(""Hello World!"");")
               .Compile<Action>();


            FakeMethodOperator.RandomDomain(item => {
                item.AssemblyName = "1";
                item.OutputToFile = true;
            })
                .UseMethod<TestB>("TestMethod")
                .MethodBody($@"Console.WriteLine(""Hello World!"");")
                .Compile<Action>(new TestA());

            


            /*
             *   在此之前，你需要右键，选择工程文件，在你的.csproj里面 
             *   
             *   写上这样一句浪漫的话： 
             *   
             *      <PreserveCompilationContext>true</PreserveCompilationContext>
             */

            //ProxyOperator<TestAbstract> abstractBuilder = new ProxyOperator<TestAbstract>();
            //abstractBuilder.OopName("UTestClass");
            //abstractBuilder["GetName"] = "return Name;";
            //abstractBuilder["GetAge"] = "return Age;";
            //abstractBuilder.Compile();
            //var test = abstractBuilder.CreateProxy("UTestClass");

            //var delegate2 = NDelegateOperator<GetterDelegate>.Delegate("return value.ToString();");
            //Console.WriteLine(delegate2(1));
            //var delegate3 = "return value.ToString();".Delegate<GetterDelegate>();
            //var delegateConvt = FastMethodOperator.Create()
            //    .Param<string>("value")
            //    .MethodBody($@"return value==""true"" || value==""mama"";")
            //    .Return<bool>()
            //    .Compile<Func<string, bool>>();

            //Console.WriteLine(delegateConvt("mama"));

           

            DynamicMethod method = new DynamicMethod("GetString", null, new Type[] { typeof(TestB), typeof(string) });
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, typeof(TestB).GetField("Name"));
            il.Emit(OpCodes.Ret);
            var emitAction = (Action<TestB, string>)(method.CreateDelegate(typeof(Action<TestB, string>)));

            var roslynAction = FastMethodOperator.DefaultDomain()
                .Param<TestB>("instance")
                .Param<string>("value")
                .Body("instance.Name = value;")
                .Compile<Action<TestB, string>>();


            Stopwatch stopwatch = new Stopwatch();
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
        public int Age1;
        public int Age2;
        public int Age3;
        public int Age4;
        public int Age5;
        public int Age6;

        public int Age7;
        public int Age8;
        public int Age9;
        public int Age10;
        public int Age11;
        public int Age12;
        public int Age13;
        public int Age21;
        public int Age31;
        public int Age41;
        public int Age51;
        public int Age61;

        public int Age71;
        public int Age81;
        public int Age91;
        public int Age211;
        public int Age311;
        public int Age411;
        public int Age511;
        public int Age611;

        public int Age711;
        public int Age811;
        public int Age911;
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