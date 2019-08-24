using Natasha;
using Natasha.Operator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Core30
{
    class Program
    {
        public static Action action;
        static void Main(string[] args)
        {
            NAssembly nAssembly = new NAssembly("a");
            Show();
            Console.WriteLine(AssemblyManagment.Count("TempDomain"));

            NStruct nStruct1 = new NStruct();
            nStruct1
                .Namespace("Core30")
                .OopName("Test")
                .Ctor(builder => builder
                    .MemberAccess(AccessTypes.Public)
                    .Param<string>("name")
                    .Body("Name=name;"))
                .PublicField<string>("Name");
            var typ1e = nStruct1.GetType();
            Console.WriteLine(AssemblyManagment.Count("Default"));


            var domain2 = AssemblyManagment.Create("TempDomain2");
            using (AssemblyManagment.Lock("TempDomain2"))
            {
                //do sth
                NStruct nStruct = new NStruct();
                nStruct
                    .Namespace("Core30")
                    .OopName("Test")
                    .Ctor(builder => builder
                        .MemberAccess(AccessTypes.Public)
                        .Param<string>("name")
                        .Body("Name=name;"))
                    .PublicField<string>("Name");
                var type = nStruct.GetType();

                nStruct = new NStruct();
                nStruct
                    .Namespace("Core30")
                    .OopName("Test1")
                    .Ctor(builder => builder
                        .MemberAccess(AccessTypes.Public)
                        .Param<string>("name")
                        .Body("Name=name;"))
                    .PublicField<string>("Name");
                type = nStruct.GetType();
            }
            Console.WriteLine(AssemblyManagment.Count("TempDomain2"));

            using (AssemblyManagment.CreateAndLock("TempDomain3"))
            {
                //do sth
                NStruct nStruct = new NStruct();
                nStruct
                    .Namespace("Core30")
                    .OopName("Test")
                    .Ctor(builder => builder
                        .MemberAccess(AccessTypes.Public)
                        .Param<string>("name")
                        .Body("Name=name;"))
                    .PublicField<string>("Name");
                var type = nStruct.GetType();
            }
            Console.WriteLine(AssemblyManagment.Count("TempDomain3"));

            //ShowQ();
            //Thread.Sleep(2000);
            //Testqq();
            //Thread.Sleep(2000);
            //TestMemoery2();
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
            for (int i = 0; (!AssemblyManagment.IsDelete("TempDomain1")) && (i < 15); i++)
            {
                Console.WriteLine($"\t第{i}次！");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(500);

            }
            //Testt();
            Console.WriteLine(AssemblyManagment.IsDelete("TempDomain1") ? "回收成功！" : "回收失败！");
        }
        public static void TestMemoery()
        {
            Console.WriteLine("Memory1");
            List<Type> list = new List<Type>();
            var domain1 = AssemblyManagment.Create("TempDomain1");
            for (int i = 0; i < 500; i += 1)
            {
                Console.WriteLine("new");
                NClass nStruct = new NClass();
                nStruct
                    .Namespace("Core301")
                    .OopName($"Test{i}")
                    
                    .Ctor(builder => builder
                        .MemberAccess(AccessTypes.Public)
                        .Param<string>("name")
                        .Body("Name=name;"))
                    .PublicField<string>("Name")
                 .PublicField<string>("Name1")
                 .PublicField<string>("Name2")
                 .PublicField<string>("Name3")
                 .PublicField<string>("Name4");
                list.Add(nStruct.GetType());
            }
            AssemblyManagment.Get("TempDomain1").Dispose();
        }

        public static void TestMemoery2()
        {
            Console.WriteLine("Memory2");
            var domain1 = AssemblyManagment.Create("TempDomain2");
            for (int i = 0; i < 10; i += 1)
            {
                Thread.Sleep(5000);
                Console.WriteLine("new");
                NClass nStruct = new NClass();
                nStruct
                    .Namespace("Core30")
                    .OopName($"Test{i}")

                    .Ctor(builder => builder
                        .MemberAccess(AccessTypes.Public)
                        .Param<string>("name")
                        .Body("Name=name;"))
                    .PublicField<string>("Name")
                 .PublicField<string>("Name1")
                 .PublicField<string>("Name2")
                 .PublicField<string>("Name3")
                 .PublicField<string>("Name4");
                var type = nStruct.GetType();
            }
            AssemblyManagment.Get("TempDomain2").Dispose();
            AssemblyManagment.Get("TempDomain2").Unload();
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
            Console.WriteLine(AssemblyManagment.IsDelete("TempDomain") ? "回收成功！" : "回收失败！");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("启用GC回收方法！");

            for (int i = 0; (!AssemblyManagment.IsDelete("TempDomain")) && (i < 15); i++)
            {
                Console.WriteLine($"\t第{i}次！");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(500);
                if (i == 6)
                {
                    //Console.WriteLine($"\t计数为{i}，删除静态引用！");
                    //千万别再这里调用 AssemblyManagment.Get("TempDomain").Dispose();
                    // action = null;
                }

            }
            Console.WriteLine();
            Console.WriteLine();
            //Console.WriteLine(!a.IsAlive? "回收成功！":"回收失败！");
            Console.Write("第二次检测：");
            Console.WriteLine(AssemblyManagment.IsDelete("TempDomain") ? "回收成功！" : "回收失败！");

            action?.Invoke();
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
            //        .MemberAccess(AccessTypes.Public)
            //        .Param<string>("name")
            //        .Body("Name=name;"))
            //    .PublicField<string>("Name");
            //var type = nStruct.GetType();



            var domain1 = AssemblyManagment.Create("TempDomain");
            nStruct = new NStruct();
            nStruct
 
                .Namespace("Core30")
                .OopName("Test1")
                .Ctor(builder => builder
                    .MemberAccess(AccessTypes.Public)
                    .Param<string>("name")
                    .Body(@"Name=name+""1"";"))
                .PublicField<string>("Name");
           var type1 = nStruct.GetType();
            domain1.RemoveType(type1);

            nStruct = new NStruct();
            nStruct

                .Namespace("Core30")
                .OopName("Test")
                .Ctor(builder => builder
                    .MemberAccess(AccessTypes.Public)
                    .Param<string>("name")
                    .Body(@"Name=name+""2"";"))
                .PublicField<string>("Name");
            var type3 = nStruct.GetType();


            nStruct = new NStruct();
            nStruct

                .Namespace("Core30")
                .OopName("Test")
                .Ctor(builder => builder
                    .MemberAccess(AccessTypes.Public)
                    .Param<string>("name")
                    .Body(@"Name=name+""3"";"))
                .PublicField<string>("Name");
            var type4 = nStruct.GetType();
            domain1.RemoveType(type3);

            //nStruct = new NStruct();
            //nStruct
            //    .InDomain(domain1)
            //    .Namespace("Core30")
            //    .OopName("Test")
            //    .Ctor(builder => builder
            //        .MemberAccess(AccessTypes.Public)
            //        .Param<string>("name")
            //        .Body(@"Name=name+""1"";"))
            //    .PublicField<string>("Name");
            //var type2 = nStruct.GetType();



            var temp = FastMethodOperator.New
          
                //.Using<Test>()
                //.Using(type)
                .Using(type1)
                .Using(type3)
                .Using(type4)
                //.MethodAttribute<MethodImplAttribute>("MethodImplOptions.NoInlining")
                .MethodBody(@"
Test obj = new Test(""Hello World!"");
Console.WriteLine(obj.Name);"
);
            action = temp.Complie<Action>();
            action();
            //AssemblyManagment.Get("TempDomain").Dispose();
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
