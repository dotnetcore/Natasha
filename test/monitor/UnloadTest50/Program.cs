using Natasha.CSharp;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnloadTest20
{
    class Program
    {
        const int count = 100;
        static Func<int[]>[] func = new Func<int[]>[100];
        static void Main(string[] args)
        {
            Console.WriteLine("Roslyn 预热；");
            var a = NInstance.Creator<Program>();
            Console.WriteLine("3秒后开始编译；");
            Thread.Sleep(3000);
            Test();
            Console.WriteLine("请记录内存，5秒后开始释放；");
            Thread.Sleep(5000);
            Release();
            Console.WriteLine("请记录内存，5秒后开始回收；");
            Thread.Sleep(5000);
            RunGc();
            Console.WriteLine("存活检测，3秒后开始；");
            Thread.Sleep(3000);
            CheckAlive();
            Console.WriteLine("程序跑完！");
            Console.ReadKey();
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void CheckAlive()
        {

            for (int i = 0; i < count; i++)
            {
                if (!DomainManagement.IsDeleted("test" + i.ToString()))
                {
                    Console.WriteLine($"{i} is aliave!");
                }

            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RunGc()
        {
            for (int i = 0; i < 10; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Release()
        {
            func = null;
            for (int i = 0; i < count; i++)
            {
                var handler = DomainManagement.Get("test" + i.ToString());
                handler.Dispose();
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Test()
        {

            for (int i = 0; i < count; i++)
            {
                var ad = DomainManagement.Create("test" + i.ToString());
                var builder = FastMethodOperator.DefaultDomain();
                builder.AssemblyBuilder.Domain = ad;
                func[i] = builder.Body($@"
int[] a = new int[40960];
for(int i =0;i<40960;i++){{a[i]=i;}}
string temp = @""111111111111111111111111111111111111111111111111111111111111111111111111
2222222222222222222222222222222222223333333333333333333333333333
2222222222222222222222222
44444444444444444444444444444444444444444444444444444444444444444444
555555555555555555555555555555555555555555555555555555555555
6666666666666666666666666666666666666666666666666666666666
77777777777777777777777777777777777777777777722221111111111111111111111111111111"";
Console.WriteLine(""HelloWorld111111111111111111111111111111111111111!"");
return a;
").Compile<Func<int[]>>();
                func[i]();
            }
        }
    }
}
