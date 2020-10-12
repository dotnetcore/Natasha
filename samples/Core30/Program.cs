using Natasha.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Core30
{
    class Program
    {
        public static Action action;
        static void Main(string[] args)
        {
            
            Show1();
            Console.WriteLine(DomainManagement.IsDeleted("TempDomain11"));

            Console.ReadKey();
        }

        public static void ShowQ()
        {
            Console.WriteLine("Hello world!");
        }
        public static void Testqq()
        {
            Thread.Sleep(5000);
            TestMemoery();
            for (int i = 0; (!DomainManagement.IsDeleted("TempDomain1")) && (i < 15); i++)
            {
                Console.WriteLine($"\t第{i}次！");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(500);

            }
            //Testt();
            Console.WriteLine(DomainManagement.IsDeleted("TempDomain1") ? "回收成功！" : "回收失败！");
        }
        public static void TestMemoery()
        {
            Console.WriteLine("Memory1");
            List<Type> list = new List<Type>();
            var domain1 = DomainManagement.Create("TempDomain1");
            for (int i = 0; i < 500; i += 1)
            {
                Console.WriteLine("new");
                NClass nStruct = new NClass();
                nStruct
                    .Namespace("Core301")
                    .Name($"Test{i}")

                    .Ctor(builder => builder
                        .Public()
                        .Param<string>("name")
                        .Body("Name=name;"))
                    .PublicField<string>("Name")
                 .PublicField<string>("Name1")
                 .PublicField<string>("Name2")
                 .PublicField<string>("Name3")
                 .PublicField<string>("Name4");
                list.Add(nStruct.GetType());
            }
            DomainManagement.Get("TempDomain1").Dispose();
        }

        public static void TestMemoery2()
        {
            Console.WriteLine("Memory2");
            var domain1 = DomainManagement.Create("TempDomain2");
            for (int i = 0; i < 10; i += 1)
            {
                Thread.Sleep(5000);
                Console.WriteLine("new");
                NClass nStruct = new NClass();
                nStruct
                    .Namespace("Core30")
                    .Name($"Test{i}")

                    .Ctor(builder => builder
                        .Public()
                        .Param<string>("name")
                        .Body("Name=name;"))
                    .PublicField<string>("Name")
                 .PublicField<string>("Name1")
                 .PublicField<string>("Name2")
                 .PublicField<string>("Name3")
                 .PublicField<string>("Name4");
                var type = nStruct.GetType();
            }
            DomainManagement.Get("TempDomain2").Dispose();
            DomainManagement.Get("TempDomain2").Unload();
        }

        public static void Testt()
        {
            Console.WriteLine("隔离编译动态方法:");
            Console.WriteLine();
            Show();
            if (action != null)
            {
                Console.WriteLine("\t静态引用动态方法，增加方法代数！");
            }
            //var a = AssmblyManagment.Remove("TempDomain");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("第一次检测：");
            Console.WriteLine(DomainManagement.IsDeleted("TempDomain") ? "回收成功！" : "回收失败！");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("启用GC回收方法！");

            for (int i = 0; (!DomainManagement.IsDeleted("TempDomain")) && (i < 15); i++)
            {
                Console.WriteLine($"\t第{i}次！");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(500);
                if (i == 6)
                {
                    Console.WriteLine($"\t计数为{i}，删除静态引用！");
                    //千万别再这里调用 AssemblyManagment.Get("TempDomain").Dispose();
                    action = null;
                }

            }
            Console.WriteLine();
            Console.WriteLine();
            //Console.WriteLine(!a.IsAlive? "回收成功！":"回收失败！");
            Console.Write("第二次检测：");
            Console.WriteLine(DomainManagement.IsDeleted("TempDomain") ? "回收成功！" : "回收失败！");

            action?.Invoke();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Show1()
        {
            string result;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Sql", "ClassLibrary1.dll");
            using (DomainManagement.CreateAndLock("TempDomain11"))
            {

                var domain = DomainManagement.CurrentDomain;
                var assemebly = domain.LoadPluginFromStream(path);
                var action = FastMethodOperator.DefaultDomain()
                   .Using(assemebly)
                   .Body(@"
try{
Class1 a = new Class1();
return  a.Show();
}
catch(Exception e){
    return e.Message;
}
return default;").Return<string>()

                   .Compile<Func<string>>();
                result = action();
                Console.WriteLine(result);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Show()
        {

            //默认共享域
            NStruct nStruct = new NStruct();
            //nStruct
            //    .Namespace("Core30")
            //    .OopName("Test")
            //    .Ctor(builder => builder
            //        .PublicMember
            //        .Param<string>("name")
            //        .Body("Name=name;"))
            //    .PublicField<string>("Name");
            //var type = nStruct.GetType();



            var domain1 = DomainManagement.Create("TempDomain");
            // nStruct = new NStruct();
            // nStruct

            //     .Namespace("Core30")
            //     .OopName("Test1")
            //     .Ctor(builder => builder
            //         .PublicMember
            //         .Param<string>("name")
            //         .Body(@"Name=name+""1"";"))
            //     .PublicField<string>("Name");
            //var type1 = nStruct.GetType();
            // domain1.RemoveType(type1);

            // nStruct = new NStruct();
            // nStruct

            //     .Namespace("Core30")
            //     .OopName("Test")
            //     .Ctor(builder => builder
            //         .PublicMember
            //         .Param<string>("name")
            //         .Body(@"Name=name+""2"";"))
            //     .PublicField<string>("Name");
            // var type3 = nStruct.GetType();
            // domain1.RemoveType(type3);


            nStruct = new NStruct();
            nStruct.AssemblyBuilder.Compiler.Domain = domain1;
            nStruct

                .Namespace("Core30")
                .Name("Test")
                .Ctor(builder => builder
                    .Public()
                    .Param<string>("name")
                    .Body(@"Name=name+""3"";"))
                .PublicField<string>("Name");
            var type4 = nStruct.GetType();


            //nStruct = new NStruct();
            //nStruct
            //    .INDelegate(domain1)
            //    .Namespace("Core30")
            //    .OopName("Test")
            //    .Ctor(builder => builder
            //        .PublicMember
            //        .Param<string>("name")
            //        .Body(@"Name=name+""1"";"))
            //    .PublicField<string>("Name");
            //var type2 = nStruct.GetType();



            var temp = FastMethodOperator.DefaultDomain();
            temp.AssemblyBuilder.Compiler.Domain = domain1;
            temp
                //.Using<Test>()
                //.Using(type)
                .Using(type4)
                //.MethodAttribute<MethodImplAttribute>("MethodImplOptions.NoInlining")
                .Body(@"
Test obj = new Test(""Hello World!"");
Console.WriteLine(obj.Name);"
);
            action = temp.Compile<Action>();
            action();
            DomainManagement.Get("TempDomain").Dispose();
        }

        private static void B_Unloading(System.Runtime.Loader.AssemblyLoadContext obj)
        {
            Console.WriteLine("B Unloding!");
        }

        private static void Domain_Unloading(System.Runtime.Loader.AssemblyLoadContext obj)
        {
            Console.WriteLine("\t\t触发回收事件!");
        }
    }
}
