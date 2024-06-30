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
    private static string? _className;
    private static string _proxyMethodName = "Main";
    private static string _argumentsMethodName = "ProxyMainArguments";
    public static string _proxyCommentDebugShow = "//DS".ToLower();
    public static string _proxyCommentReleaseShow = "//RS".ToLower();
    public static string _commentTag = "//Once".ToLower();
    private static readonly ConcurrentDictionary<string, SyntaxTree> _fileSyntaxTreeCache = new();
    private static readonly ConcurrentDictionary<string, HashSet<string>> _cs0104UsingCache = new();
    //private static readonly ConcurrentDictionary<string, CSharpParseOptions?> _mixOPLCommentCache = new();
    private static readonly object _runLock = new object();
    private static readonly List<string> _args = [];
    private static readonly VSCSharpProcessor _processor;
    private static readonly VSCSharpProjectFileWatcher _csprojWatcher;
    private static bool _isCompiling;
    private static bool _isFaildBuild;
    internal static bool IsRelease;
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
    private static readonly Func<string, string?> _triviaHandle;
    private static HEProjectKind _projectKind;
    public static bool _isHotCompiling;
    public static bool IsHotCompiling { get { return _isHotCompiling; } }

    static HEProxy()
    {
        _triviaHandle = comment =>
        {
            var commentLower = comment.ToLower();
            if (commentLower.StartsWith(_commentTag))
            {
                return string.Empty;
            }
            if (!IsRelease)
            {
                if (commentLower.StartsWith(_proxyCommentDebugShow))
                {
                    var length = _proxyCommentDebugShow.Length + 1;
                    if (comment.Length > length)
                    {
                        return CreatePreprocessorReplaceScript(GetCommentScript(comment, length));
                    }
                }
            }
            else if (commentLower.StartsWith(_proxyCommentReleaseShow))
            {
                var length = _proxyCommentReleaseShow.Length + 1;
                if (comment.Length > length)
                {
                    return CreatePreprocessorReplaceScript(GetCommentScript(comment,length));
                }
            }
            return null;
            static string GetCommentScript(string comment,int startIndex)
            {
                if (comment.EndsWith(";"))
                {
                    return comment.Substring(startIndex, comment.Length - startIndex - 1);
                }
                return comment[startIndex..];
            }
        };

        CompileInitAction = () => { NatashaManagement.Preheating(true, true); };
        _debugOptions = new CSharpParseOptions(LanguageVersion.Preview, preprocessorSymbols: ["DEBUG"]);
        _currentOptions = _debugOptions;
        _releaseOptions = new CSharpParseOptions(LanguageVersion.Preview, preprocessorSymbols: ["RELEASE"]);
        _buildLock = new();
        _mainWatcher = new();
        _processor = new();
        _csprojWatcher = new(VSCSharpProjectInfomation.CSProjFilePath);

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
                    if (_proxyMethodName != "Main")
                    {
                        ShowMessage($"Waiting for [{_proxyMethodName}] running...");
                        HotExecute().Wait();
                    }
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
#if DEBUG
            ShowMessage($"检测到空成员文件 {file}.");
#endif

            return tree;
        }

        //顶级语句处理
        root = ToplevelRewriter.Handle(root);
        //CS0104处理
        _cs0104UsingCache[file] = CS0104Analyser.Handle(root);
        if (VSCSharpProjectInfomation.EnableImplicitUsings)
        {
            //从默认Using缓存中排除 CS0104 
            root = UsingsRewriter.Handle(root, _cs0104UsingCache);
        }
        //HE命令处理
        root = MethodTriviaRewriter.Handle(root, _triviaHandle);
        //主入口处理
        root = HandleProxyMainMethod(root);
        return CSharpSyntaxTree.Create(root, _currentOptions, file, Encoding.UTF8);
    }
    private static bool _isFirstCompile = true;
    private static async Task HotExecute()
    {
        try
        {
            if (_isFirstCompile)
            {
                _isFirstCompile = false;
                CompileInitAction();
            }

            _isHotCompiling = true;
            if (IsRelease && _currentOptions != _releaseOptions)
            {
                _currentOptions = _releaseOptions;
                await ReAnalysisFiles();
            }
            else if (!IsRelease && _currentOptions != _debugOptions)
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
                UsingsRewriter.OnceInitDefaultUsing(_fileSyntaxTreeCache, _currentOptions);
            }

            var assembly = await HECompiler.ReCompile(_fileSyntaxTreeCache.Values, IsRelease);
            var types = assembly.GetTypes();
            var typeInfo = assembly.GetTypeFromShortName(_className!);
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

            var proxyMethodInfo = typeInfo.GetMethod(_proxyMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            object? instance = null;

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
            ShowMessage($"执行主入口回调方法....");
            _endCallback?.Invoke();


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
    private static CompilationUnitSyntax HandleProxyMainMethod(CompilationUnitSyntax root)
    {
        var proxyMethod = root.DescendantNodes()
                             .OfType<MethodDeclarationSyntax>()
                             .FirstOrDefault(m => m.Identifier.Text == _proxyMethodName);

        if (proxyMethod != null)
        {
            //预处理符号分析
            IsRelease = OptimizationAnalyser.Handle(proxyMethod) ?? false;
            ClassDeclarationSyntax? parentClass = proxyMethod.Parent as ClassDeclarationSyntax ?? throw new Exception($"获取 {_proxyMethodName} 方法类名出现错误！");
            _className = parentClass.Identifier.Text;
            if (proxyMethod.Body != null)
            {
                //入口重写
                var newBody = HotStatupRewrite(proxyMethod.Body);
                if (newBody != null)
                {
                    root = root.ReplaceNode(proxyMethod.Body, newBody);
                }
            }

        }

        return root;
    }
    private static BlockSyntax? HotStatupRewrite(BlockSyntax blockSyntax)
    {
        if (_projectKind == HEProjectKind.Winform)
        {
            return WinformRewriter.Handle(blockSyntax);
        }
        else if (_projectKind == HEProjectKind.WPF)
        {
            return WpfWriter.Handle(blockSyntax);
        }
        else if (_projectKind == HEProjectKind.Console)
        {
            ConsoleWriter.Handle(blockSyntax);
        }
        return null;
    }
    private static string CreatePreprocessorReplaceScript(string content)
    {
        if (_projectKind == HEProjectKind.AspnetCore || _projectKind == HEProjectKind.Console)
        {
            return $"Console.WriteLine({content});";
        }
        return $"HEProxy.ShowMessage(({content}).ToString());";
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
    public static void SetCompileInitAction(Action action)
    {
        CompileInitAction = action;
    }
    public static void SetOnceCommentTag(string comment)
    {
        _commentTag = comment.ToLower();
    }
    public static void SetDebugCommentTag(string comment)
    {
        OptimizationAnalyser._proxyCommentOPLDebug = comment.Trim().ToLower();
    }
    public static void SetReleaseCommentTag(string comment)
    {
        OptimizationAnalyser._proxyCommentOPLRelease = comment.Trim().ToLower();
    }
    public static void SetCS0104CommentTag(string comment)
    {
        CS0104Analyser._proxyCommentCS0104Using = comment.Trim().ToLower();
    }
    public static void SetProxyMethodName(string methodName)
    {
        _proxyMethodName = methodName;
    }
    public static void SetArgumentsMethodName(string methodName)
    {
        _argumentsMethodName = methodName;
    }
    #endregion
}

