
using ExtensionSample12;
using ExtensionSample4;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace ExtensionSample
{
    internal class Program
    {
        // [DllImport("ntdll.dll")]
        //static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref PROCESS_BASIC_INFORMATION processInformation, int processInformationLength, out int returnLength);

        //[StructLayout(LayoutKind.Sequential)]
        //struct PROCESS_BASIC_INFORMATION
        //{
        //    public IntPtr Reserved1;
        //    public IntPtr PebBaseAddress;
        //    public IntPtr Reserved2;
        //    public IntPtr Reserved3;
        //    public UIntPtr UniqueProcessId;
        //    public UIntPtr InheritedFromUniqueProcessId;
        //}

        static void Main(string[] args)
        {

            //Process currentProcess = Process.GetCurrentProcess();
            //IntPtr processHandle = currentProcess.Handle;

            //PROCESS_BASIC_INFORMATION processInformation = new PROCESS_BASIC_INFORMATION();
            //int returnLength;

            //int status = NtQueryInformationProcess(processHandle, 0, ref processInformation, Marshal.SizeOf(processInformation), out returnLength);

            //if (status >= 0)
            //{
            //    IntPtr parentProcessId = (nint)processInformation.InheritedFromUniqueProcessId;
            //    Process parentProcess = Process.GetProcessById((int)parentProcessId);
            //    if (parentProcess.MainModule != null)
            //    {
            //        var temp = Path.Combine(Path.GetFullPath(Path.GetDirectoryName(parentProcess.MainModule.FileName) + "..\\..\\..\\..\\..\\..\\"), "VC#", "Snippets");
            //        if (Directory.Exists(temp))
            //        {
            //            var directories = Directory.GetDirectories(temp, "Visual C#", SearchOption.AllDirectories);
            //            if (directories.Length > 0)
            //            {
            //               var path = directories[0];
            //            }
            //        }
            //    }
                

            //    Console.WriteLine("父进程应用程序所在的目录: " );
            //}
            //else
            //{
            //    Console.WriteLine("无法获取父进程信息。错误代码: " + status);
            //}


            HEProxyState<int>.Value += 1;
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(args[i]);
            }
            Console.WriteLine(HEProxyState<int>.Value);

            //var func = "return arg1+arg2 + C.Get();".WithAssemblyBuilder(
            //    builder => builder
            //    .UseRandomLoadContext()
            //    .UseSmartMode()
            //    .WithDebugCompile(debug => debug.WriteToAssembly())
            //    //.Add(debuggerScript)
            //    //.ConfigLoadContext(ctx => ctx
            //    //.AddReferenceAndUsingCode<object>()
            //    //.AddReferenceAndUsingCode(typeof(Debugger)))
            //    )
            //    .WithUsings(["System.Runtime.CompilerServices", "System.Runtime.InteropServices"]).ToFunc<int, int, int>();
            //Console.WriteLine(func!(1, 2));
            var types = typeof(Program).Assembly.GetTypes();
            foreach (var item in types)
            {
                Console.WriteLine(item.Name);
            }

            //Once
            for (int i = 0; i < 5; i++)
            {
                //Once
                Console.WriteLine(i);
            }

            //Console.WriteLine(File.Exists("1.txt"));
            Console.WriteLine(Class1.Get());
            Console.WriteLine((new Class4()).ToString());
            Console.WriteLine(JsonSerializer.Serialize(new Class3()));
            //Once1
            //Once
            //Once2
            Console.ReadKey();

        }

        public static void TestShow()
        {
            Console.WriteLine(2);
        }

        public static void Test()
        {

            //AssemblyCSharpBuilder assemblyCSharp = new();
            //assemblyCSharp.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>());
            //assemblyCSharp.Add("public class A{\r\npublic void Show(){\r\n}\r\n}");
            //assemblyCSharp.LogCompilationEvent += (log) => { Console.WriteLine(log.ToString()); };
            //assemblyCSharp.GetAssembly();
            //ProxyMainArguments();
            //ProxyMain(args);
           

            // TestCollection();
            //TestPage();
            //GraphQL Write
            //Console.WriteLine(addReaction.ToMutationScript());
            //GraphQL Read
            //Console.WriteLine(query.ToQueryScript());


            //TestModel testModel = new();
            //testModel.Show();

            //CompileDirector director = new CompileDirector();
            //director.ConfigBuilder(builder => builder
            //.ConfigSyntaxOptions(opt=>opt.WithPreprocessorSymbols("DEBUG"))
            //.WithDebugCompile()
            //.ConfigLoadContext(ctx=>ctx
            //    .AddReferenceAndUsingCode<object>()
            //    .AddReferenceAndUsingCode(typeof(Math))
            //    .AddReferenceAndUsingCode(typeof(File))
            //));
            //Console.WriteLine((new Class4()).Show());
            //var builder = director.CreateBuilder();
            //RunAssemblyBuilder(director);
            //RunAssemblyBuilder(director);
            //RunAssemblyBuilder(director);
            //RunAssemblyBuilder(director);
        }
        public static void ProxyMainArguments()
        {
            HEProxy.AppendArgs("参数1");
            HEProxy.AppendArgs("参数2");
            HEProxy.AppendArgs("参数3");
            HEProxy.AppendArgs("参数4");
        }

        private static void RunAssemblyBuilder(CompileDirector director)
        {
            var builder = director.CreateBuilder();
            builder.Add(@"public static class A
{  
    public static void Show()
    { 
        File.WriteAllText(""1.txt"", ""1"");
        Console.WriteLine(Math.Abs(-4));   
    }
}");
            var asm = director.GetAssembly(builder);
            asm.GetDelegateFromShortName<Action>("A", "Show")!();
        }

        /*
        public static async void TestPage()
        {
            GithubQueryData query = new()
            {
                Repository = new GithubRepository("DotNetCore", "Natasha")
                {
                    Labels = new GithubConnection<GithubLabel>()
                    {
                        Nodes =
                       [
                           new GithubLabel()
                            {
                                Id = "",
                                Name = "",
                            }
                       ]
                    }
                }
            };
            var labels = await query.GetCollectionResultAsync(repository => repository!.Labels!, 5);
            foreach (var item in labels)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();
            query.Repository!.Labels.SetNextPage();
            labels = await query.GetCollectionResultAsync(repository => repository!.Labels!, 5);
            foreach (var item in labels)
            {
                Console.WriteLine(item.Name);
            }

            query.Repository.Labels.Reset();
            labels = await query.GetCollectionResultAsync(repository => repository!.Labels!, -5);
            foreach (var item in labels)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();
            query.Repository!.Labels.SetNextPage();
            labels = await query.GetCollectionResultAsync(repository => repository!.Labels!, -5);
            foreach (var item in labels)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();
        }

        public static async void TestCollection()
        {
            GithubQueryData query = new()
            {
                Repository = new GithubRepository("DotNetCore", "Natasha")
                {
                    Labels = new GithubConnection<GithubLabel>()
                    {
                        Nodes =
                        [
                            new GithubLabel()
                            {
                                Id = "",
                                Name = "",
                            }
                        ]
                    }
                }
            };
            var labels = await query.GetCollectionResultAsync(repository => repository!.Labels!);
            foreach (var item in labels)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();
            labels = await query.GetCollectionResultAsync(repository => repository!.Labels!, 5);
            foreach (var item in labels)
            {
                Console.WriteLine(item.Name);
            }
            Console.WriteLine();
            labels = await query.GetCollectionResultAsync(repository => repository!.Labels!, -5);
            foreach (var item in labels)
            {
                Console.WriteLine(item.Name);
            }

            Console.WriteLine();
            var count = await query.GetTotalAsync(repository => repository!.Labels!);
            Console.WriteLine(count);
        }
        public static async void TestReadAndWrite()
        {
            GithubQueryData query = new()
            {
                Repository = new GithubRepository("DotNetCore", "Natasha")
                {
                    Issue = new(297)
                    {
                        Title = "",
                        Url = "",
                        Id = ""
                    }
                }
            };

            var (data, msg) = await query.GetExecuteResultAsync();
            var issue = data!.Repository!.Issue!;
            Console.WriteLine(issue.Title);
            Console.WriteLine(issue.Url);
            Console.WriteLine(issue.Id);

            //var (data, msg) = client.Query(query).Result;
            //Console.WriteLine(msg);
            //var issue = data!.repository!.issue!;
            //Console.WriteLine(issue.title);
            //Console.WriteLine(issue.url);
            //Console.WriteLine(issue.id);


            //AddReaction addReaction = new(issue.id!, GithubReactionContent.HOORAY);
            //var (result, msg) =await addReaction.GetMutationResult(client => client.SetSecretToken("xxx"));
            //if (result)
            //{
            //    Console.WriteLine("succeed");
            //}
            //创建方法实例
            GithubMutationData add = new()
            {
                AddReaction = new(issue.Id!, GithubReactionContent.HOORAY)
            };

            //获取方法结果
            (var result, msg) = await add.GetExecuteStateAsync();
            if (result)
            {
                Console.WriteLine("succeed");
            }
            else
            {
                Console.WriteLine(msg);
            }


            GithubMutationData remove = new()
            {
                //创建方法实例
                RemoveReaction = new(issue.Id!, GithubReactionContent.HOORAY)
                {
                    //创建需要返回的实体结构
                    Reaction = new()
                    {
                        Id = "",
                        Content = 0,
                        CreatedAt = DateTime.Now
                    }
                }
            };


            //获取方法结果
            (var reData, msg) = await remove.GetExecuteResultAsync();
            if (reData != null)
            {
                var reaction = reData!.RemoveReaction!.Reaction!;
                Console.WriteLine("succeed");
                Console.WriteLine(reaction.Id);
                Console.WriteLine(reaction.Content);
                Console.WriteLine(reaction.CreatedAt);
            }
            else
            {
                Console.WriteLine(msg);
            }
        }*/
    }
}
