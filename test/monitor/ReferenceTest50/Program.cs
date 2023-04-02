
using Natasha.CSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ReferenceTest50
{
    internal class Program
    {
        public static Action Temp;
        public static void TestMethod() => Console.WriteLine("AA");
        static void Main(string[] args)
        {
            C("a");
            _ = typeof(NLog.GlobalDiagnosticsContext);
            _ = typeof(Serilog.Configuration.LoggerAuditSinkConfiguration);
            _ = typeof(Nacos.Request.RequestClient);
            _ = typeof(Nacos.Microsoft.Extensions.Configuration.ConfigListener);
            _ = typeof(Nacos.AspNetCore.V2.NacosAspNetOptions);
            _ = typeof(WebApiClientCore.ApiActionInvoker);
            _ = typeof(AlibabaCloud.SDK.Dysmsapi20170525.Client);
            _ = typeof(AlibabaCloud.RPCClient.Client);
            _ = typeof(AlibabaCloud.TeaUtil.Common);
            //Check();
            //Console.WriteLine(typeof(string).Assembly == typeof(object).Assembly && typeof(string).Namespace == typeof(object).Namespace);
            //Console.WriteLine(typeof(Console).Assembly == typeof(object).Assembly && typeof(Console).Namespace == typeof(object).Namespace);

            //var assemblyNames = DependencyContext.Default.GetDefaultAssemblyNames();
            //foreach (var item in assemblyNames)
            //{
            //    Console.WriteLine(item.Name);
            //}
            //Console.ReadKey();
            //            NatashaInitializer.InitializeAndPreheating();
            //            string code = @"
            //using ReferenceTest50;
            // public class A {   
            //    /// <summary>
            //    ///  comment will raise a error.
            //    /// </summary>
            //    public string Name{get;set;}
            //    /// <summary>
            //    ///  comment will raise a error.
            //    /// </summary>
            //    public Test test;
            //}";
            //Console.WriteLine("重新加载引用后,默认域程序集数:");
            //Console.WriteLine(System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies.Count());
            //Show();
            //            //Show(code);


            //            Show2(code);
            // Check();
            //Microsoft.CodeAnalysis.Workspaces.ErrorLogger
            Check();

            NatashaInitializer.Preheating((asm,name)=>name.Contains("IO"));
            Check();
            AssemblyCSharpBuilder builder = new();
            builder.Domain = DomainManagement.Random();
            builder.EnableSemanticHandler = true;
            builder.Add(DefaultUsing.UsingScript + "public class Abved{ string Name; public int Age;}");
            var asm = builder.GetAssembly();
            Check();
            NDelegate.RandomDomain().Action("Console.WriteLine(\"hello world!\");")();
            Stopwatch stopwatch = new();
            stopwatch.Start();
            var delegateAction = FastMethodOperator.DefaultDomain()
                        .Async()
                        .Param<string>("str1")
                        .Param<string>("str2")
                        .Body(@"
                            string a=""1"";
                            string b=""1"";  
                            return ""sss"";")
                        .Return<Task<string>>()

                .Compile();
            stopwatch.Stop();
            Console.WriteLine(stopwatch.Elapsed.TotalMilliseconds);
            Console.WriteLine("Completed!");
            Console.ReadKey();
            //ILogger a;
            //a.Debug("");
            //a.INF("");
            //a.Warning("");
            //a.Error("");
            //a.Fatal("");
            // Check();

            //Check();
            //Console.WriteLine("=====================================");
            //var domain = DomainManagement.Random;
            ////domain.AddReferencesFromAssembly(typeof(object).Assembly);
            ////domain.AddReferencesFromAssembly(typeof(Console).Assembly);
            //NDelegate.UseDomain(domain).Action("Console.WriteLine(1);");
            ////NDelegate.RandomDomain(item=>item.DisableSemanticCheck()).Action("Console.WriteLine(1);");

            //stopwatch.Restart();
            //NDelegate.RandomDomain().Action("Serilog.Configuration.LoggerAuditSinkConfiguration loggerAuditSinkConfiguration = default;")();
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
            //Console.ReadKey();
            ////Check();
            //stopwatch.Restart();
            //NDelegate
            //    .RandomDomain(item=>item.DisableSemanticCheck())
            //    .SetMethod(item=>item.SkipInit().SkipInit())
            //    .Action("Serilog.Configuration.LoggerAuditSinkConfiguration loggerAuditSinkConfiguration = default;")();
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);
            //Console.ReadKey();
            //Check();
            Console.ReadKey();
        }

        public static void C(dynamic a)
        {
            var method = typeof(Program).GetMethod("C");
            var p =  method.GetParameters()[0];
            Console.WriteLine(p.Name);
        }
        public static void Check()
        {
            var assemblies = System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies;
            Console.WriteLine($"默认域程序集数量:{assemblies.Count()}");
            //var names = new HashSet<string>(assemblies.Select(item => item.GetName().Name));
            //Console.WriteLine("当前程序集:");
            //foreach (var item in names)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine(System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies.Count());
            //IEnumerable<string> paths = DependencyContext
            //        .Default
            //        .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());


            ////重复
            //Console.WriteLine("重复:");
            //foreach (var item in paths)
            //{

            //    //Console.WriteLine(Path.GetFileNameWithoutExtension(item));
            //    if (names.Contains(Path.GetFileNameWithoutExtension(item)))
            //    {
            //        Console.ForegroundColor = ConsoleColor.Green;
            //        Console.WriteLine(item);
            //        Console.ResetColor();
            //    }
            //    else
            //    {
            //        //Console.ForegroundColor = ConsoleColor.Yellow;
            //        //Console.WriteLine(item);
            //        //Console.ResetColor();
            //    }
            //}
        }
        //public static void Show()
        //{
        //    NatashaInitializer.InitializeAndPreheating();
        //    DomainBase.DefaultDomain.AssemblyReferencesCache.Clear();
        //    var assemblies = System.Runtime.Loader.AssemblyLoadContext.Default.Assemblies;
        //    foreach (var item in assemblies)
        //    {
        //        if (item.IsDynamic)
        //        {
        //            Console.WriteLine($"{item.GetName().Name} is a Dynamic Assembly!");
        //        }
        //        else
        //        {
        //            DomainBase.DefaultDomain.AddReferencesFromFileStream(item.Location);
        //        }
                
        //    }
        //    AssemblyCSharpBuilder assemblyCSharpBuilder = new AssemblyCSharpBuilder();
        //    assemblyCSharpBuilder.Add(@"public class A{  public async Task Show(){  }  }");
        //    var assembly = assemblyCSharpBuilder.GetAssembly();
        //    Console.WriteLine(assembly!=null);
        //}

        //public static void Test()
        //{

        //    //预热 Natasha
        //    //参数(非必要): 排除以 Nacos. 开头的文件引用.
        //    //NatashaInitializer.InitializeAndPreheating(asmName => asmName.StartsWith("Nacos."));

        //    //创建和捕捉域上下文, 在花括号内所有的构建都将在 myRandom 域中进行. (非主域可卸载)
        //    using (DomainManagement.CreateAndLock("myRandom"))
        //    {

        //        //准备一段字符串脚本,创建一个类
        //        string script = @"public class A
        //              { 
        //                  public A(){  Address =  ""World""; }

        //                  public string? Name{ get{ return ""Hello""; } }

        //                  public string? Address;
        //              }";

        //        //创建一个新的操作类 NatashaOperator.
        //        //MethodOperator/ClassOperator/EnumOperator/InterfaceOperator
        //        //StructOperator/RecordOperator/DelegateOperator/SharpBuilder

        //        //创建一个手动构建脚本的编译类
        //        var builder = NatashaOperator.SharpBuilder;
        //        //动态编译支持可空引用
        //        builder.EnableNullableCompile();
        //        //设置dll输出路径
        //        builder.SetDllFilePath("myRandomA.dll");
        //        //设置调试信息输出路径
        //        builder.SetPdbFilePath("myRandomA.pdb");
        //        //设置注释文件输出路径
        //        builder.SetXmlFilePath("myRandomA.xml");
        //        //覆盖全局 using
        //        script = DefaultUsing.DefaultScript + script;
        //        //添加脚本到 编译单元 中
        //        builder.Add(script);
        //        //编译成程序集
        //        var assembly = builder.GetAssembly();


        //        //创建一个委托操作类
        //        var nDelegate = NatashaOperator.DelegateOperator;

        //        //配置编译器
        //        nDelegate.AssemblyBuilder
        //            //开启成功日志记录
        //            .EnableSucceedLog()
        //            //禁用语义过滤
        //            .DisableSemanticCheck()
        //            //编译出错记录日志并抛出异常
        //            .LogAndThrowCompilerError()
        //            //语法出错记录日志并抛出异常
        //            .LogAndThrowSyntaxError()
        //            //生成注释文档
        //            .SetXmlFilePath("myDelete.xml");

        //        var action = nDelegate
        //            .SetMethod(item => item
        //                //跳过方法栈变量初始化,即[SkipInit]
        //                .SkipInit()
        //                .Summary("这是方法的注释")
        //                .SummaryParameter("arg", "传递一个字符串,连接 Name 和 Address.")
        //                .SummaryReturn("返回 Name arg Address 拼接好的字符串.")
        //                )
        //            .Func<string, string>("var obj = new A(); return obj.Name+arg+obj.Address;");

        //        Console.WriteLine(action("&"));
        //    }

        //    //卸载该域
        //    DomainManagement.Remove("myRandom");

        //}
        //public static void Show(string code)
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    NatashaCSharpCompiler natashaCSharpCompiler = new NatashaCSharpCompiler();
        //    var options = natashaCSharpCompiler.GetCompilationOptions();
        //    //NatashaInitializer.InitializeAndPreheating();
        //    var compilation = CSharpCompilation.Create("test", null, DomainBase.DefaultDomain.GetCompileReferences(), options);
        //    var tree = CSharpSyntaxTree.ParseText(code);
        //    compilation = compilation.AddSyntaxTrees(tree);
        //   // var result = compilation.Emit("1.dll", "1.pdb", "1.xml");

        //    Stream outputStream = new MemoryStream();
        //    outputStream = File.Create("1.dll");
        //    using (var xmlStream = File.Create("1.xml"))
        //    {
        //        using (var pdbStream = File.Create("1.pdb"))
        //        {
        //           var result = compilation.Emit(
        //           outputStream,
        //           pdbStream: pdbStream,
        //           xmlDocumentationStream: xmlStream
        //           );
        //            Console.WriteLine(result.Success);
        //        }
        //    }
        //    stopwatch.Stop();
        //    Console.WriteLine(stopwatch.ElapsedMilliseconds);
        //}
        //public static void Show2(string code)
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    AssemblyCSharpBuilder assemblyCSharpBuilder = new AssemblyCSharpBuilder();
        //    assemblyCSharpBuilder.Domain = DomainManagement.Random;
        //    assemblyCSharpBuilder.Add(code);
        //    assemblyCSharpBuilder.SetPdbFilePath("2.pdb");
        //    assemblyCSharpBuilder.SetXmlFilePath("1.xml");
        //    var assembly = assemblyCSharpBuilder.GetAssembly();
        //    stopwatch.Stop();
        //    Console.WriteLine(assembly!=null);
        //    Console.WriteLine(stopwatch.ElapsedMilliseconds);

        //}
    }
}
