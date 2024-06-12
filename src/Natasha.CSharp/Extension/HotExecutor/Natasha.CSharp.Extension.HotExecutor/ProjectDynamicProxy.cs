using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using Natasha.CSharp.Extension.HotExecutor;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using static System.Reflection.Metadata.Ecma335.MethodBodyStreamEncoder;


public static class ProjectDynamicProxy
{
    private static string? _className;
    private static string? _proxyMethodName = "Main";
    private static string? _argumentsMethodName;
    private static readonly ConcurrentDictionary<string, SyntaxTree> _fileCache = new();
    private static readonly object _runLock = new object();
    private static readonly List<string> _args = [];
    private static readonly VSCSharpProcessor _processor;
    private static readonly VSCSharpProjectFileWatcher _csprojWatcher;
    private static bool _isCompiling;
    private static bool _isFaildBuild;
    internal static bool IsRelease;
    private readonly static CSharpParseOptions _debugOptions;
    private static Action? _endCallback;
    private static readonly HESpinLock _buildLock;
    private static readonly VSCSharpMainFileWatcher _mainWatcher;
    private static string _commentTag = "//Once";
    //private static readonly MethodDeclarationSyntax _emptyMainTree;
    //private static string _callMethod;

    public static void SetCommentTag(string comment)
    {
        _commentTag = comment;
    }
    static ProjectDynamicProxy()
    {
        NatashaManagement.Preheating(true, true);
        _debugOptions = new CSharpParseOptions(LanguageVersion.Preview, preprocessorSymbols: ["DEBUG", "RELEASE"]);
//        var emptyTreeScript = @"internal class Program{
//    static void Main(){ }
//}";
//        _emptyMainTree = CSharpSyntaxTree.ParseText(emptyTreeScript).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().First();
        _buildLock = new();
        _mainWatcher = new();
        
        _processor = new();
        _csprojWatcher = new(VSCSharpFolder.CSProjFilePath, async () => {


            if (_buildLock.GetLock())
            {
                _isCompiling = true;
#if DEBUG
                Console.WriteLine("准备重新构建！");
#endif
                
                if (await _processor.BuildProject())
                {
                    var needReBuildAgain = false;
                    while (!_isCompiling)
                    {
                        needReBuildAgain = true;
                        _isCompiling = true;
                        await Task.Delay(1000);
                    }
                    if (needReBuildAgain)
                    {
#if DEBUG
                        Console.WriteLine("抢占成功，将重新编译！");
#endif
                        _buildLock.ReleaseLock();
                        _csprojWatcher!.Notify();
                        return;
                    }

#if DEBUG
                    Console.WriteLine("构建成功，准备启动！");
#endif
                    if (await _processor.Run())
                    {
#if DEBUG
                        Console.WriteLine("启动成功，退出当前程序！");
#endif
                        //try
                        //{
                        //    Environment.Exit(0);
                        //}
                        //catch (Exception ex)
                        //{
                        //}
                    }
                }
                else
                {
                    _isFaildBuild = true;
                    _buildLock.ReleaseLock();
#if DEBUG
                    Console.WriteLine("构建失败！");
#endif
                }
            }
            else
            {
                _isCompiling = false;
                Console.WriteLine("检测到多次更改触发多次抢占编译，当前将尽可能抢占编译权限！");
            }

        });
        _csprojWatcher.StartMonitor();

        _mainWatcher.PreFunction = () =>
        {
            if (_isFaildBuild)
            {
                _csprojWatcher.Notify();
                return true;
            }
            return false;
        };

        _mainWatcher.AfterFunction = HotExecute;

        _mainWatcher.ChangeFileAction = file => {
            var content = ReadFile(file);
            var tree = HandleTree(content);
            _fileCache[file] = tree;
        };

        _mainWatcher.DeleteFileAction = file => {
            _fileCache.TryRemove(file, out _);
        };

        _mainWatcher.CreateFileAction = file =>
        {
            var content = ReadFile(file);
            var tree = HandleTree(content);
            _fileCache[file] = tree;
        };
        _mainWatcher.StartMonitor();
    }
    
    /// <summary>
    /// 重执行之前需要做的工作
    /// </summary>
    /// <param name="callback"></param>
    public static void ConfigEndBackcall(Action callback)
    {
        _endCallback = callback;
    }

    public static void NeedBeDisposedObject(params IAsyncDisposable[] disposableObjects)
    {
        _endCallback = async () => 
        {
            foreach (var disposableObject in disposableObjects)
            {
                await disposableObject.DisposeAsync();
            }
        };
    }

    public static void BuildWithRelease()
    {
        IsRelease = true;
    }

    public static void BuildWithDebug()
    {
        IsRelease = false;
    }
    
    public static void AppendArgs(string arg)
    {
        _args.Add(arg);
    }
    public static void AppendArgs(params string[] args)
    {
        _args.AddRange(args);
    }
    public static void ClearArgs()
    {
        _args.Clear();
    }

    private static bool _isFirstRun = true;
    public static void Run(string? argumentsMethodName = "ProxyMainArguments")
    {
        if (_isFirstRun)
        {
            lock (_runLock)
            {
                if (_isFirstRun)
                {
                    _isFirstRun = false;
                    _argumentsMethodName = argumentsMethodName;
                    var srcCodeFiles = Directory.GetFiles(VSCSharpFolder.MainCsprojPath, "*.cs", SearchOption.AllDirectories);

                    foreach (var file in srcCodeFiles)
                    {
                        if (VSCSharpFolder.CheckFileAvailiable(file))
                        {
                            var content = ReadFile(file);
                            var tree = HandleTree(content);
                            var root = tree.GetRoot();
                            _fileCache[file] = root.SyntaxTree;
                        }
                    }

                    //HotExecute();
                }
            }
        }
#if DEBUG
        else
        {
            Console.WriteLine("已经初始化过了！");
        }
#endif

    }
    private static SyntaxTree HandleTree(string content)
    {
        var tree = NatashaCSharpSyntax.ParseTree(content, _debugOptions);
        var root = tree.GetRoot();

        var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
        Dictionary<MethodDeclarationSyntax, MethodDeclarationSyntax> replaceMethodCache = [];
        foreach (var methodDeclaration in methodDeclarations)
        {
            var removeIndexs = new HashSet<int>();
            // 获取方法体
            var methodBody = methodDeclaration.Body;
            if (methodBody == null)
            {
                continue;
            }
            // 遍历方法体中的语句
            for (int i = 0; i < methodBody.Statements.Count; i++)
            {
                // 获取当前语句
                var statement = methodBody.Statements[i];
                if (statement is ExpressionStatementSyntax)
                {
                    var trivias = statement.GetLeadingTrivia();
                    foreach (var trivia in trivias)
                    {
                        if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) && trivia.ToString().Trim().StartsWith(_commentTag))
                        {
                            removeIndexs.Add(i);
                            break;
                        }
                    }
                }
            }

            // 如果找到，创建新的方法体列表并排除该语句
            if (removeIndexs.Count > 0)
            {
                var newMethodBody = new List<StatementSyntax>(methodBody.Statements.Where((s, index) => !removeIndexs.Contains(index)));
                replaceMethodCache[methodDeclaration] = methodDeclaration.WithBody(SyntaxFactory.Block(newMethodBody));
            }
        }

        foreach (var item in replaceMethodCache)
        {
//#if DEBUG
//            Console.WriteLine();
//            Console.WriteLine("方法：");
//            Console.WriteLine(item.Key.ToFullString());
//            Console.WriteLine("替换为：");
//            Console.WriteLine(item.Value.ToFullString());
//            Console.WriteLine();
//#endif
            root = root.ReplaceNode(item.Key, item.Value);
            tree = root.SyntaxTree;
        }

        var mainMethod = root.DescendantNodes()
                             .OfType<MethodDeclarationSyntax>()
                             .FirstOrDefault(m => m.Identifier.Text == "Main");

        if (mainMethod != null)
        {
            ClassDeclarationSyntax? parentClass = mainMethod.Parent as ClassDeclarationSyntax ?? throw new Exception("获取 Main 方法类名出现错误！");
            _className = parentClass.Identifier.Text;
        }
        return tree;
    }
    //use FileStream read a text file
    private static string ReadFile(string file)
    {
        FileStream stream;
        do
        {
            try
            {
                stream = new(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                StringBuilder stringBuilder = new();
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                }
                stream.Dispose();
                return stringBuilder.ToString();
            }
            catch (Exception)
            {
#if DEBUG
            Console.WriteLine("命中文件锁！");
#endif

                Thread.Sleep(200);
            }


        } while (true);

    }

    private static Task HotExecute()
    {
        try
        {
            Console.Clear();
            var assembly = HECompiler.ReCompile(_fileCache.Values,IsRelease);
            var types = assembly.GetTypes();
            var typeInfo = assembly.GetTypeFromShortName(_className!);
            var methods = typeInfo.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            
            _endCallback?.Invoke();
            if (methods.Any(item => item.Name == _argumentsMethodName))
            {
                _args.Clear();
                var argumentMethodInfo = methods.First(item => item.Name == _argumentsMethodName);
                argumentMethodInfo.Invoke(null, []);
            }

            var proxyMethodInfo = typeInfo.GetMethod(_proxyMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            object? instance = null;
            try
            {
                
                if (!proxyMethodInfo.IsStatic)
                {
                    instance = Activator.CreateInstance(typeInfo);
                }

                if (proxyMethodInfo.GetParameters().Length > 0)
                {
                    proxyMethodInfo.Invoke(instance, [_args.ToArray()]);
                }
                else
                {
                    proxyMethodInfo.Invoke(instance, []);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during hot execution: {ex}");

        }
        return Task.CompletedTask;
    }
}

