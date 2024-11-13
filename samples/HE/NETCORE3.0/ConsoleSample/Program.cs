using Natasha.CSharp.Extension.HotExecutor;
using Natasha.CSharp.HotExecutor.Component;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ConsoleSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //HE:Async
            //设置当前程序的类型 ，默认为 Console
            HEProxy.SetProjectKind(HEProjectKind.Console);

            //创建热执行的日志输出实例(如果不需要写入日志，用控制台，这句就不用写了)
            string debugFilePath = Path.Combine(VSCSProjectInfoHelper.HEOutputPath, "Debug.txt");
            HEFileLogger logger = new HEFileLogger(debugFilePath);

            //设置信息输出方式,该方法影响 DS/RS 指令的输出方式
            //默认是 Console.WriteLine 方式输出
            HEProxy.ShowMessage = async msg => {
                await logger.WriteUtf8FileAsync(msg);
            };

            //编译初始化选项，主要是 Natasha 的初始化操作.
            //Once (热编译时使用 Once  剔除该语句)
            HEProxy.SetCompileInitAction(() => {
                {
                    NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
                    NatashaManagement.Preheating((asmName, @namespace) =>

                                !string.IsNullOrWhiteSpace(@namespace) &&
                                (HEProxy.IsExcluded(@namespace)),
                                true,
                                true);
                }
            });

            //开始执行动态代理.
            //Once (热编译时使用 Once 剔除语句)
            HEProxy.Run();

            TestLocalMethod();
            HEProxyState<int>.Value += 1;
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(args[i]);
                //DS args[i]
            }


            Console.WriteLine(HEProxyState<int>.Value);
            CancellationTokenSource source = new CancellationTokenSource();

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
                    static void InnerLocalMethod()
                    {
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
            HEProxy.AppendArgs("参数5");
        }
    }
}
