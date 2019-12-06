using Natasha;
using Natasha.Operator;
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
            // NAssembly nAssembly = new NAssembly("a");
            //// Show();
            //// Console.WriteLine(AssemblyManagment.Count("TempDomain"));

            // NStruct nStruct1 = new NStruct();
            // nStruct1
            //     .Namespace("Core30")
            //     .OopName("Test")
            //     .Ctor(builder => builder
            //         .PublicMember
            //         .Param<string>("name")
            //         .Body("Name=name;"))
            //     .PublicField<string>("Name");
            // var typ1e = nStruct1.GetType();
            // Console.WriteLine(AssemblyManagment.Count("Default"));


            // var domain2 = AssemblyManagment.Create("TempDomain2");
            // using (AssemblyManagment.Lock("TempDomain2"))
            // {
            //     //do sth
            //     NStruct nStruct = new NStruct();
            //     nStruct
            //         .Namespace("Core30")
            //         .OopName("Test")
            //         .Ctor(builder => builder
            //             .PublicMember
            //             .Param<string>("name")
            //             .Body("Name=name;"))
            //         .PublicField<string>("Name");
            //     var type = nStruct.GetType();

            //     nStruct = new NStruct();
            //     nStruct
            //         .Namespace("Core30")
            //         .OopName("Test1")
            //         .Ctor(builder => builder
            //             .PublicMember
            //             .Param<string>("name")
            //             .Body("Name=name;"))
            //         .PublicField<string>("Name");
            //     type = nStruct.GetType();
            // }
            // Console.WriteLine(AssemblyManagment.Count("TempDomain2"));

            // using (AssemblyManagment.CreateAndLock("TempDomain3"))
            // {
            //     //do sth
            //     NStruct nStruct = new NStruct();
            //     nStruct
            //         .Namespace("Core30")
            //         .OopName("Test")
            //         .Ctor(builder => builder
            //             .PublicMember
            //             .Param<string>("name")
            //             .Body("Name=name;"))
            //         .PublicField<string>("Name");
            //     var type = nStruct.GetType();
            // }
            // Console.WriteLine(AssemblyManagment.Count("TempDomain3"));

            //ShowQ();
            //Thread.Sleep(2000);
            //Testqq();
            //Thread.Sleep(2000);
            //TestMemoery2();
            //Testt();

            string result;
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Lib", "Sql", "ClassLibrary1.dll");
            using (DomainManagment.CreateAndLock("TempDomain11"))
            {

                var domain = DomainManagment.CurrentDomain;
                var assemebly = domain.LoadStream(path);
                var action = FastMethodOperator.Create()
                   .Using(assemebly)
                   .MethodBody(@"
try{
//MySqlConnection conn = new MySqlConnection("""");
//conn.Open();
Class1 a = new Class1();
return  a.Show();
}
catch(Exception e){
    return e.Message;
}
return default;").Return<string>()

                   .Complie<Func<string>>();
                result = action();
                Console.WriteLine(result);
            }
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
            for (int i = 0; (!DomainManagment.IsDeleted("TempDomain1")) && (i < 15); i++)
            {
                Console.WriteLine($"\t第{i}次！");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(500);

            }
            //Testt();
            Console.WriteLine(DomainManagment.IsDeleted("TempDomain1") ? "回收成功！" : "回收失败！");
        }
        public static void TestMemoery()
        {
            Console.WriteLine("Memory1");
            List<Type> list = new List<Type>();
            var domain1 = DomainManagment.Create("TempDomain1");
            for (int i = 0; i < 500; i += 1)
            {
                Console.WriteLine("new");
                NClass nStruct = new NClass();
                nStruct
                    .Namespace("Core301")
                    .OopName($"Test{i}")
                    
                    .Ctor(builder => builder
                        .PublicMember
                        .Param<string>("name")
                        .Body("Name=name;"))
                    .PublicField<string>("Name")
                 .PublicField<string>("Name1")
                 .PublicField<string>("Name2")
                 .PublicField<string>("Name3")
                 .PublicField<string>("Name4");
                list.Add(nStruct.GetType());
            }
            DomainManagment.Get("TempDomain1").Dispose();
        }

        public static void TestMemoery2()
        {
            Console.WriteLine("Memory2");
            var domain1 = DomainManagment.Create("TempDomain2");
            for (int i = 0; i < 10; i += 1)
            {
                Thread.Sleep(5000);
                Console.WriteLine("new");
                NClass nStruct = new NClass();
                nStruct
                    .Namespace("Core30")
                    .OopName($"Test{i}")

                    .Ctor(builder => builder
                        .PublicMember
                        .Param<string>("name")
                        .Body("Name=name;"))
                    .PublicField<string>("Name")
                 .PublicField<string>("Name1")
                 .PublicField<string>("Name2")
                 .PublicField<string>("Name3")
                 .PublicField<string>("Name4");
                var type = nStruct.GetType();
            }
            DomainManagment.Get("TempDomain2").Dispose();
            DomainManagment.Get("TempDomain2").Unload();
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
            Console.WriteLine(DomainManagment.IsDeleted("TempDomain") ? "回收成功！" : "回收失败！");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("启用GC回收方法！");

            for (int i = 0; (!DomainManagment.IsDeleted("TempDomain")) && (i < 15); i++)
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
            Console.WriteLine(DomainManagment.IsDeleted("TempDomain") ? "回收成功！" : "回收失败！");

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
            //        .PublicMember
            //        .Param<string>("name")
            //        .Body("Name=name;"))
            //    .PublicField<string>("Name");
            //var type = nStruct.GetType();



            var domain1 = DomainManagment.Create("TempDomain");
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
            nStruct.Complier.Domain = domain1;
            nStruct

                .Namespace("Core30")
                .OopName("Test")
                .Ctor(builder => builder
                    .PublicMember
                    .Param<string>("name")
                    .Body(@"Name=name+""3"";"))
                .PublicField<string>("Name");
            var type4 = nStruct.GetType();
           

            //nStruct = new NStruct();
            //nStruct
            //    .InDomain(domain1)
            //    .Namespace("Core30")
            //    .OopName("Test")
            //    .Ctor(builder => builder
            //        .PublicMember
            //        .Param<string>("name")
            //        .Body(@"Name=name+""1"";"))
            //    .PublicField<string>("Name");
            //var type2 = nStruct.GetType();



            var temp = FastMethodOperator.Create();
            temp.Complier.Domain = domain1;
            temp
                //.Using<Test>()
                //.Using(type)
                .Using(type4)
                //.MethodAttribute<MethodImplAttribute>("MethodImplOptions.NoInlining")
                .MethodBody(@"
Test obj = new Test(""Hello World!"");
Console.WriteLine(obj.Name);"
);
            action = temp.Complie<Action>();
            action();
            DomainManagment.Get("TempDomain").Dispose();
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
