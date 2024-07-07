using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using Natasha.CSharp.Extension.HotExecutor;
using Natasha.CSharp.HotExecutor.Component;
using Natasha.CSharp.HotExecutor.Component.SyntaxUtils;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

public static class HEProxy
{
    private static string _argumentsMethodName = "ProxyMainArguments";
    private static readonly ConcurrentDictionary<string, SyntaxTree> _fileSyntaxTreeCache = new();
    private static readonly ConcurrentDictionary<string, HashSet<string>> _cs0104UsingCache = new();
    //private static readonly ConcurrentDictionary<string, CSharpParseOptions?> _mixOPLCommentCache = new();
    private static readonly object _runLock = new object();
    private static readonly List<string> _args = [];
    private static readonly VSCSharpProcessor _processor;
    private static readonly VSCSharpProjectFileWatcher _csprojWatcher;
    private static bool _isCompiling;
    private static bool _isFaildBuild;
    private readonly static CSharpParseOptions _debugOptions;
    private readonly static CSharpParseOptions _releaseOptions;
    private static CSharpParseOptions _currentOptions;
    private static Action? _preCallback;
    private static Action? _endCallback;
    private static readonly HESpinLock _buildLock;
    private static readonly VSCSharpMainFileWatcher _mainWatcher;
    internal static Action CompileInitAction;
    public static Action<string> ShowMessage = Console.WriteLine;
    public static readonly HashSet<string> NatashaExtGlobalUsing = [];
    private static readonly HashSet<IAsyncDisposable> _asyncDisposables = [];
    private static readonly HashSet<IDisposable> _disposables = [];
    private static readonly List<CancellationTokenSource> _cancellations = [];
    private static bool _isFirstRun = true;
    private static HEProjectKind _projectKind;
    public static bool _isHotCompiling;
    public static bool IsHotCompiling { get { return _isHotCompiling; } }

    private static readonly ProxyMethodPlugin _proxyMethodPlugin = new("Main");
    private static readonly AsyncTriviaPlugin _asyncTriviaPlugin = new("//HE:Async");
    private static readonly OptimizationTriviaPlugin _optimizationTriviaPlugin = new("//HE:Debug","//HE:Release");
    private static readonly CS0104TriviaPlugin _cs0104TriviaPlugin = new("//HE:CS0104");
    private static readonly OutputTriviaPlugin _outputTriviaPlugin = new("//DS","//RS",() => _optimizationTriviaPlugin.IsRelease);
    private static readonly HETreeMethodRewriter _treeMethodRewriter = new();
    private static readonly HETreeTriviaRewriter _treeTrivialRewriter = new();
    public static bool IsRelease { get { return _optimizationTriviaPlugin.IsRelease; } }
    static HEProxy()
    {

        CompileInitAction = () => { NatashaManagement.Preheating(true, true); };
        _debugOptions = new CSharpParseOptions(LanguageVersion.Preview, preprocessorSymbols: ["DEBUG"]);
        _currentOptions = _debugOptions;
        _releaseOptions = new CSharpParseOptions(LanguageVersion.Preview, preprocessorSymbols: ["RELEASE"]);
        _buildLock = new();
        _mainWatcher = new();
        _processor = new();
        _csprojWatcher = new(VSCSharpProjectInfomation.CSProjFilePath);
        _proxyMethodPlugin
            .RegisteTriviaPlugin(_asyncTriviaPlugin)
            .RegisteTriviaPlugin(_optimizationTriviaPlugin);
        _treeMethodRewriter
            .RegistePlugin(_proxyMethodPlugin);
        _treeTrivialRewriter
            .RegistePlugin(_outputTriviaPlugin);
        if (VSCSharpProjectInfomation.EnableImplicitUsings)
        {
            _treeTrivialRewriter.RegistePlugin(_cs0104TriviaPlugin);
        }  
    }
    private static void DeployCSProjWatcher()
    {
        _csprojWatcher.SetExecute(async () =>
        {

            if (_buildLock.GetLock())
            {
                _isCompiling = true;
#if DEBUG
                ShowMessage("准备重新构建！");
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
                        ShowMessage("抢占成功，将重新编译！");
                        _buildLock.ReleaseLock();
                        _csprojWatcher!.Notify();
                        return;
                    }

                    ShowMessage("构建成功，准备启动！");
                    if (await _processor.Run())
                    {
#if DEBUG
                        ShowMessage("启动成功，退出当前程序！");
#endif
                    }
                }
                else
                {
                    _isFaildBuild = true;
                    _buildLock.ReleaseLock();
                    ShowMessage("构建失败！");

                }
            }
            else
            {
                _isCompiling = false;
                ShowMessage("检测到多次更改触发多次抢占编译，当前将尽可能抢占编译权限！");
            }

        });
    }
    private static void DeployMainWatcher()
    {
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

        _mainWatcher.ChangeFileAction = async file =>
        {
            var tree = await HandleTree(file);
            if (tree != null)
            {
                _fileSyntaxTreeCache[file] = tree;
            }
        };

        _mainWatcher.DeleteFileAction = file =>
        {
            _fileSyntaxTreeCache.TryRemove(file, out _);
        };

        _mainWatcher.CreateFileAction = async file =>
        {
            var tree = await HandleTree(file);
            if (tree != null)
            {
                _fileSyntaxTreeCache[file] = tree;
            }
        };
        _mainWatcher.DeployMonitor();
    }

    public static void Run()
    {
        if (_isFirstRun)
        {
            lock (_runLock)
            {
                if (_isFirstRun)
                {
                    _isFirstRun = false;
                    ReAnalysisFiles().Wait();
                    DeployCSProjWatcher();
                    DeployMainWatcher();
                    _mainWatcher.StartMonitor();
                    _csprojWatcher.StartMonitor();
                }
            }
        }
#if DEBUG
        else
        {
            ShowMessage("已经初始化过了！");
        }
#endif

    }
    private async static Task<SyntaxTree?> HandleTree(string file)
    {
        var tree = NatashaCSharpSyntax.ParseTree(await HEFileHelper.ReadUtf8FileAsync(file), file, _currentOptions, Encoding.UTF8);
        if (tree == null)
        {
#if DEBUG
            ShowMessage($"检测到空文件 {file}.");
#endif

            return null;
        }

        var root = tree.GetCompilationUnitRoot()!;
        if (root.Members.Count == 0)
        {
            return tree;
        }

        //顶级语句处理
        root = ToplevelHandler.Handle(root);
        root = OnceHandler.Handle(root);
        
        if (VSCSharpProjectInfomation.EnableImplicitUsings)
        {
            _cs0104TriviaPlugin.Initialize();
        }
        //语法树重写
        root = NatashaCSharpSyntax
            .ParseTree(
                _treeTrivialRewriter
                    .Visit(root)
                    .ToFullString()
                    .Replace(_outputTriviaPlugin.CommentPrefix, "")
                , file
                , _currentOptions
                , Encoding.UTF8)
            .GetCompilationUnitRoot();

        //方法重写
        root = (CompilationUnitSyntax)_treeMethodRewriter.Visit(root);
        ShowMessage(root.ToFullString());

        //CS0104处理
        if (VSCSharpProjectInfomation.EnableImplicitUsings)
        {
            _cs0104UsingCache[file] = _cs0104TriviaPlugin.ExcludeUsings;
            //从默认Using缓存中排除 CS0104 
            root = UsingsHandler.Handle(root, _cs0104UsingCache);
            return CSharpSyntaxTree.Create(root, _currentOptions, file, Encoding.UTF8);
        }
        return root.SyntaxTree;
    }


    private static bool _isFirstCompile = true;
    private static async Task HotExecute()
    {
        try
        {
            CleanErrorFiles();
        }
        catch (Exception ex)
        {
            ShowMessage("清除 HEOutput 文件夹时出错："+ ex.Message);
        }

        try
        {
            if (_isFirstCompile)
            {
                _isFirstCompile = false;
                CompileInitAction();
            }

            _isHotCompiling = true;
            if (_optimizationTriviaPlugin.IsRelease && _currentOptions != _releaseOptions)
            {
                _currentOptions = _releaseOptions;
                await ReAnalysisFiles();
            }
            else if (!_optimizationTriviaPlugin.IsRelease && _currentOptions != _debugOptions)
            {
                _currentOptions = _debugOptions;
                await ReAnalysisFiles();
            }
            if (_projectKind == HEProjectKind.Console || _projectKind == HEProjectKind.AspnetCore)
            {
                Console.Clear();
            }

            if (VSCSharpProjectInfomation.EnableImplicitUsings)
            {
                UsingsHandler.OnceInitDefaultUsing(_fileSyntaxTreeCache, _currentOptions);
            }

            var assembly = await HECompiler.ReCompile(_fileSyntaxTreeCache.Values, _optimizationTriviaPlugin.IsRelease);
            var types = assembly.GetTypes();
            var typeInfo = assembly.GetTypeFromShortName(_proxyMethodPlugin.ClassName!);
            var methods = typeInfo.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            ShowMessage($"执行主入口前导方法....");
            _preCallback?.Invoke();

            if (_cancellations.Count > 0)
            {
                foreach (var item in _cancellations)
                {
                    if (item.Token.CanBeCanceled && !item.Token.IsCancellationRequested)
                    {
                        item.Cancel();
                    }
                }
                _cancellations.Clear();
            }

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

            var proxyMethodInfo = typeInfo.GetMethod(_proxyMethodPlugin.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            object? instance = null;

            if (!proxyMethodInfo.IsStatic)
            {
                instance = Activator.CreateInstance(typeInfo);
            }

            if (_asyncTriviaPlugin.IsAsync)
            {
                Task mainTask;

                if (proxyMethodInfo.GetParameters().Length > 0)
                {
                    mainTask = Task.Run(() =>
                    {

                        proxyMethodInfo.Invoke(instance, [_args.ToArray()]);

                    });
                }
                else
                {
                    mainTask = Task.Run(() =>
                    {
                        proxyMethodInfo.Invoke(instance, []);
                    });
                }
                mainTask.Exception?.Handle(ex => { ShowMessage($"Error during hot execution: {ex}"); return true; });
            }
            else
            {
                if (proxyMethodInfo.GetParameters().Length > 0)
                {
                    proxyMethodInfo.Invoke(instance, [_args.ToArray()]);
                }
                else
                {
                    proxyMethodInfo.Invoke(instance, []);
                }
            }


            ShowMessage($"执行主入口回调方法....");
            _endCallback?.Invoke();
            ShowMessage($"入口回调方法执行完毕.");

        }
        catch (Exception ex)
        {

            ShowMessage($"热编译运行失败....");
            if (ex is NatashaException nex)
            {
                var code = nex.Formatter;
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
                ShowMessage($"Error during hot execution: {errorBuilder}");
                File.WriteAllText(Path.Combine(VSCSharpProjectInfomation.HEOutputPath, "error." + Guid.NewGuid().ToString("N") + ".txt"), nex.Formatter);
            }
            else
            {
                ShowMessage($"Error during hot execution: {ex}");
            }


        }
        _isHotCompiling = false;
        return;
    }
    #region 辅助方法区

    private static void CleanErrorFiles()
    {
        var files = Directory.GetFiles(VSCSharpProjectInfomation.HEOutputPath);
        foreach (var file in files)
        {
            if (Path.GetFileName(file).StartsWith("error."))
            {
                File.Delete(file);
            }
        }
    }
    private async static Task ReAnalysisFiles()
    {
#if DEBUG
        ShowMessage("重新扫描文件！");
#endif


        _fileSyntaxTreeCache.Clear();

        var srcCodeFiles = Directory.GetFiles(VSCSharpProjectInfomation.MainCsprojPath, "*.cs", SearchOption.AllDirectories);

        foreach (var file in srcCodeFiles)
        {
            if (VSCSharpProjectInfomation.CheckFileAvailiable(file))
            {
                var tree = await HandleTree(file);
                if (tree != null)
                {
                    _fileSyntaxTreeCache[file] = tree;
                }
            }
        }
    }

    
    #endregion

    #region 普通 API 区

    public static void SetProjectKind(HEProjectKind kind)
    {
        _projectKind = kind;
    }

    /// <summary>
    /// 重执行主逻辑之前需要做的工作
    /// </summary>
    /// <param name="callback"></param>
    public static void SetPreHotExecut(Action callback)
    {
        _preCallback = callback;
    }
    /// <summary>
    /// 重执行主逻辑之后需要做的工作
    /// </summary>
    /// <param name="callback"></param>
    public static void SetAftHotExecut(Action callback)
    {
        _endCallback = callback;
    }
    public static void NeedBeCancelObject(CancellationTokenSource cancelObject)
    {
        _cancellations.Add(cancelObject);
    }
    public static void NeedBeCancelObject(IEnumerable<CancellationTokenSource> cancelObjects)
    {
        _cancellations.AddRange(cancelObjects);
    }
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
    public static void SetCompileInitAction(Action action)
    {
        CompileInitAction = action;
    }
    public static void SetOnceCommentTag(string comment)
    {
        OnceHandler.SetOnceComment(comment);
    }
    public static void SetDebugCommentTag(string comment)
    {
        _optimizationTriviaPlugin.SetDebugCommentTag(comment);
    }
    public static void SetReleaseCommentTag(string comment)
    {
        _optimizationTriviaPlugin.SetReleaseCommentTag(comment);
    }
    public static void SetAsyncCommentTag(string comment)
    {
        _asyncTriviaPlugin.SetAsyncCommentTag(comment);
    }
    public static void SetCS0104CommentTag(string comment)
    {
        _cs0104TriviaPlugin.SetMatchComment(comment);
    }
    
    public static void SetArgumentsMethodName(string methodName)
    {
        _argumentsMethodName = methodName;
    }

    public static void SetDebugOutputCommentTag(string debugComment)
    {
        _outputTriviaPlugin.SetDebugOutputCommentTag(debugComment);
    }
    public static void SetReleaseOutputCommentTag(string releaseComment)
    {
        _outputTriviaPlugin.SetReleaseOutputCommentTag(releaseComment);
    }

    public static void ExcludeGlobalUsing(string usingCode)
    {
        NatashaExtGlobalUsing.Add(usingCode);
    }
    public static bool IsExcluded(string usingCode)
    {
        return NatashaExtGlobalUsing.Contains(usingCode);
    }

    public static void RegisteTrivialPlugin(TriviaSyntaxPluginBase plugin)
    {
        _treeTrivialRewriter.RegistePlugin(plugin);
    }
    public static void RegisteMethodPlugin(MethodSyntaxPluginBase plugin)
    {
        _treeMethodRewriter.RegistePlugin(plugin);
    }
    #endregion
}

