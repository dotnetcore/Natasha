using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleSample
{
    internal class Program
    {

        static void Main(string[] args)
        {
            //HE:Async
            TestLocalMethod();
            HEProxyState<int>.Value += 1;
            for (int i = 0; i < args.Length; i++)
            {
                //DS args[i]
            }


            Console.WriteLine(HEProxyState<int>.Value);
            CancellationTokenSource source = new();
            
            source.ToHotExecutor();
            while (!source.IsCancellationRequested)
            {
                Thread.Sleep(1000);
                //DS "In while loop!"
            }
            Console.ReadKey();

            static void TestLocalMethod()
            {
                //DS "In TestLocalMethod"
                Action lambda = () =>
                {
                    //DS "In lambda action"
                    InnerLocalMethod();
                    static void InnerLocalMethod() {
                        Action lambda2 = () =>
                        {


                            //DS "In lambda2"
                            var temp = 6;
                            //DS temp+1
                            //DS temp+1 == 1
                            //DS "Out lambda2"
                        };
                        //DS "In InnerLocalMethod"
                        lambda2();
                        //DS "Out InnerLocalMethod"

                    }
                    //DS "Out lambda action"
                };
                lambda();
                //DS "Out TestLocalMethod"
            }
        }
        public static void ProxyMainArguments()
        {
            HEProxy.AppendArgs("参数1");
            HEProxy.AppendArgs("参数2");
            HEProxy.AppendArgs("参数3");
            HEProxy.AppendArgs("参数4");
        }
    }
}
