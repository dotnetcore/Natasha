using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using Natasha.CSharp.Extension.HotExecutor;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Xml;


public static class ProjectDynamicProxy
{
    private static string? _className;
    private static string? _proxyMethodName = "Main";
    private static string? _argumentsMethodName;
    private static readonly ConcurrentDictionary<string, SyntaxTree> _fileSyntaxTreeCache = new();
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
    internal static Action CompileInitAction;

    public static void SetCompileInitAction(Action action)
    {
        CompileInitAction = action;
    }
    public static void SetCommentTag(string comment)
    {
        _commentTag = comment;
    }
    private static UsingDirectiveSyntax[] _defaultUsingNodes = [];
    public static void SetDefaultUsingCode(UsingDirectiveSyntax[] usings)
    {
        _defaultUsingNodes = usings;
        foreach (var item in _fileSyntaxTreeCache)
        {
            var root = item.Value.GetCompilationUnitRoot();
            _fileSyntaxTreeCache[item.Key] = root.AddUsings(_defaultUsingNodes).SyntaxTree;
        }
    }
    static ProjectDynamicProxy()
    {
        CompileInitAction = () => { NatashaManagement.Preheating(true, true); };
        _debugOptions = new CSharpParseOptions(LanguageVersion.Preview, preprocessorSymbols: ["DEBUG", "RELEASE"]);
        //        var emptyTreeScript = @"internal class Program{
        //    static void Main(){ }
        //}";
        //        _emptyMainTree = CSharpSyntaxTree.ParseText(emptyTreeScript).GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>().First();
        _buildLock = new();
        _mainWatcher = new();

        _processor = new();
        _csprojWatcher = new(VSCSharpProjectInfomation.CSProjFilePath, async () =>
        {


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

        _mainWatcher.ChangeFileAction = file =>
        {
            var tree = HandleTree(file);
            if (tree != null)
            {
                _fileSyntaxTreeCache[file] = tree;
            }
        };

        _mainWatcher.DeleteFileAction = file =>
        {
            _fileSyntaxTreeCache.TryRemove(file, out _);
        };

        _mainWatcher.CreateFileAction = file =>
        {
            var tree = HandleTree(file);
            if (tree != null)
            {
                _fileSyntaxTreeCache[file] = tree;
            }
        };
        _mainWatcher.StartMonitor();
    }

    /// <summary>
    /// 重执行之前需要做的工作
    /// </summary>
    /// <param name="callback"></param>
    public static void ConfigPreHotExecut(Action callback)
    {
        _endCallback = callback;
    }

    private static readonly HashSet<IAsyncDisposable> _asyncDisposables = [];
    private static readonly HashSet<IDisposable> _disposables = [];
    public static void NeedBeDisposedObject(IEnumerable<IAsyncDisposable> disposableObjects)
    {
        _asyncDisposables.UnionWith(disposableObjects);
    }
    public static void NeedBeDisposedObject(IAsyncDisposable disposableObject)
    {
        _asyncDisposables.Add(disposableObject);
    }
    public static void NeedBeDisposedObject(IEnumerable<IDisposable> disposableObjects)
    {
        _disposables.UnionWith(disposableObjects);
    }
    public static void NeedBeDisposedObject(IDisposable disposableObject)
    {
        _disposables.Add(disposableObject);
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
                    var srcCodeFiles = Directory.GetFiles(VSCSharpProjectInfomation.MainCsprojPath, "*.cs", SearchOption.AllDirectories);

                    foreach (var file in srcCodeFiles)
                    {
                        if (VSCSharpProjectInfomation.CheckFileAvailiable(file))
                        {
                            var tree = HandleTree(file);
                            if (tree != null)
                            {
                                //Console.WriteLine(tree.ToString());
                                _fileSyntaxTreeCache[file] = tree;
                            }
                        }
                    }
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
    private static SyntaxTree? HandleTree(string file)
    {

        var content = ReadFile(file);
        var tree = NatashaCSharpSyntax.ParseTree(content, _debugOptions);
        if (tree == null)
        {
#if DEBUG

            Console.WriteLine($"检测到空文件 {file}.");
#endif
            return null;
        }
        var root = tree.GetCompilationUnitRoot()!;
        if (root.Members.Count == 0)
        {
#if DEBUG

            Console.WriteLine($"检测到空成员文件 {file}.");
#endif
            return tree;
        }
        var firstMember = root.Members[0];
        if (firstMember != null && firstMember.IsKind(SyntaxKind.GlobalStatement))
        {
#if DEBUG
            Console.WriteLine("检测到顶级语句！");
#endif
            var usings = root.Usings;
            root = root.RemoveNodes(usings, SyntaxRemoveOptions.KeepNoTrivia);
            content = "public class Program{ async static Task Main(string[] args){" + root!.ToFullString() + "}}";
            tree = NatashaCSharpSyntax.ParseTree(content, _debugOptions);
            root = tree.GetCompilationUnitRoot();
            if (!VSCSharpProjectInfomation.EnableImplicitUsings)
            {
                root = root.AddUsings([.. usings]);
            }
        }


        if (VSCSharpProjectInfomation.EnableImplicitUsings)
        {
            var usings = root.Usings;
            if (usings.Count > 0)
            {
                root = root.AddUsings(_defaultUsingNodes);
            }
        }



        var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
        if (methodDeclarations == null)
        {
            return root.SyntaxTree;
        }

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
        }

        var mainMethod = root.DescendantNodes()
                             .OfType<MethodDeclarationSyntax>()
                             .FirstOrDefault(m => m.Identifier.Text == "Main");

        if (mainMethod != null)
        {
            ClassDeclarationSyntax? parentClass = mainMethod.Parent as ClassDeclarationSyntax ?? throw new Exception("获取 Main 方法类名出现错误！");
            _className = parentClass.Identifier.Text;
        }
        return root.SyntaxTree;
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

    private static async Task HotExecute()
    {
        try
        {

            Console.Clear();
            Assembly assembly;
            if (VSCSharpProjectInfomation.EnableImplicitUsings)
            {
                if (_defaultUsingNodes.Length == 0)
                {
                    foreach (var item in _fileSyntaxTreeCache)
                    {
                        var namespaces = item
                                            .Value
                                            .GetCompilationUnitRoot()
                                            .DescendantNodes()
                                            .OfType<NamespaceDeclarationSyntax>()
                                            .Select(ns => ns.Name.ToString())
                                            .ToList();
                        HECompiler.RemoveUsings(namespaces);
                    }
                    SetDefaultUsingCode(HECompiler.GetDefaultUsingNodes());
                }
                List<SyntaxTree> needBeCompileTrees = [];
                foreach (var item in _fileSyntaxTreeCache)
                {
                    var root = item.Value.GetCompilationUnitRoot();
                    needBeCompileTrees.Add(root.SyntaxTree);
                }
                assembly = HECompiler.ReCompile(needBeCompileTrees, IsRelease);
            }
            else
            {
                assembly = HECompiler.ReCompile(_fileSyntaxTreeCache.Values, IsRelease);
            }

            var types = assembly.GetTypes();
            var typeInfo = assembly.GetTypeFromShortName(_className!);
            var methods = typeInfo.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            _endCallback?.Invoke();

            if (_asyncDisposables.Count > 0)
            {
                foreach (var disposableObject in _asyncDisposables)
                {
                    await disposableObject.DisposeAsync();
                }
                _asyncDisposables.Clear();
            }
            if (_disposables.Count > 0)
            {
                foreach (var disposableObject in _disposables)
                {
                    disposableObject.Dispose();
                }
                _disposables.Clear();
            }
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
            if (ex is NatashaException nex)
            {
                var errors = nex!.Diagnostics;
                StringBuilder errorBuilder = new();
                errorBuilder.AppendLine();
                foreach (var error in errors)
                {
                    if (error.DefaultSeverity == DiagnosticSeverity.Error)
                    {
                        errorBuilder.AppendLine(error.ToString());
                    }
                }
                Console.WriteLine($"Error during hot execution: {errorBuilder}");
            }
            else
            {
                Console.WriteLine($"Error during hot execution: {ex}");
            }

        }
        return;
    }
}

