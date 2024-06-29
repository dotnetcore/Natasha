using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Host;
using Natasha.CSharp.Compiler.Component;
using Natasha.CSharp.Extension.HotExecutor;
using Natasha.CSharp.HotExecutor.Component;
using System;
using System.Buffers.Text;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

public static class HEProxy
{
    private static string? _className;
    private static string _proxyMethodName = "Main";
    private static string _proxyCommentDebugShow = "//DS".ToLower();
    private static string _proxyCommentReleaseShow = "//RS".ToLower();
    private static string _proxyCommentOPLDebug = "//HE:Debug".ToLower();
    private static string _proxyCommentOPLRelease = "//HE:Release".ToLower();
    private static string _proxyCommentCS0104Using = "//HE:CS0104".ToLower();
    private static string _argumentsMethodName = "ProxyMainArguments";
    private static string _commentTag = "//Once".ToLower();
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
    public static readonly HashSet<string> ExtGlobalUsing = [];
    private static UsingDirectiveSyntax[] _defaultUsingNodes = [];
    private static readonly HashSet<IAsyncDisposable> _asyncDisposables = [];
    private static readonly HashSet<IDisposable> _disposables = [];
    private static readonly List<CancellationTokenSource> _cancellations = [];
    private static bool _isFirstRun = true;

    static HEProxy()
    {

        CompileInitAction = () => { NatashaManagement.Preheating(true, true); };
        _debugOptions = new CSharpParseOptions(LanguageVersion.Preview, preprocessorSymbols: ["DEBUG"]);
        _currentOptions = _debugOptions;
        _releaseOptions = new CSharpParseOptions(LanguageVersion.Preview, preprocessorSymbols: ["RELEASE"]);
        _buildLock = new();
        _mainWatcher = new();
        _processor = new();
        _csprojWatcher = new(VSCSharpProjectInfomation.CSProjFilePath, async () =>
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
                    ReAnalysisFiles();
                    if (_proxyMethodName != "Main")
                    {
                        ShowMessage($"Waiting for [{_proxyMethodName}] running...");
                        HotExecute().Wait();
                    }
                    _mainWatcher.StartMonitor();
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
    private static SyntaxTree? HandleTree(string file)
    {
        var tree = NatashaCSharpSyntax.ParseTree(ReadUtf8File(file), file, _currentOptions, Encoding.UTF8);
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

        root = HandleToplevelNodes(file, root);
        HandleCSO1O4Usings(file, root);
        if (VSCSharpProjectInfomation.EnableImplicitUsings)
        {
            root = HandleImplicitUsings(file, root);
        }
        root = HandlePickedProxyMethod(root);
        root = HandleMethodReplace(root);
        Debug.WriteLine(root.ToFullString());
        return GetOptimizationLevelNode(file, root, Encoding.UTF8);
    }

    private static HEProjectKind _projectKind;
    public static void SetProjectKind(HEProjectKind kind)
    {
        _projectKind = kind;
    }
    private static async Task HotExecute()
    {
        try
        {

            if (IsRelease && _currentOptions != _releaseOptions)
            {
                _currentOptions = _releaseOptions;
                ReAnalysisFiles();
            }
            else if (!IsRelease && _currentOptions != _debugOptions)
            {
                _currentOptions = _debugOptions;
                ReAnalysisFiles();
            }
            if (_projectKind == HEProjectKind.Console || _projectKind == HEProjectKind.AspnetCore)
            {
                Console.Clear();
            }
            if (VSCSharpProjectInfomation.EnableImplicitUsings)
            {
                //implicit usings fill
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
            }

            var assembly = HECompiler.ReCompile(_fileSyntaxTreeCache.Values, IsRelease);
            ShowMessage($"获取元数据....");
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
                ShowMessage($"执行主入口回调方法....");
                _endCallback?.Invoke();
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

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
        return;
    }

    #region 辅助方法区
    public static void SetDefaultUsingCode(UsingDirectiveSyntax[] usings)
    {
        _defaultUsingNodes = usings;
        foreach (var item in _fileSyntaxTreeCache)
        {
            var root = item.Value.GetCompilationUnitRoot();
            _fileSyntaxTreeCache[item.Key] = GetOptimizationLevelNode(item.Key, HandleImplicitUsings(item.Key, root));
        }
    }
    private static void ReAnalysisFiles()
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
                var tree = HandleTree(file);
                if (tree != null)
                {
                    _fileSyntaxTreeCache[file] = tree;
                }
            }
        }
    }
    private static void ResetOptimizationLevel()
    {
        foreach (var item in _fileSyntaxTreeCache)
        {
            _fileSyntaxTreeCache[item.Key] = CSharpSyntaxTree.Create(item.Value.GetCompilationUnitRoot(), _currentOptions, item.Value.FilePath, Encoding.UTF8);
        }
    }
    /*
    private static void ResetDefaultOptimizationLevel()
    {
        foreach (var item in _mixOPLCommentCache)
        {
            if (_mixOPLCommentCache.TryGetValue(item.Key,out var value))
            {
                if (value == null)
                {
                    if (_fileSyntaxTreeCache.TryGetValue(item.Key,out var syntax))
                    {
                        _fileSyntaxTreeCache[item.Key] = CSharpSyntaxTree.Create(syntax.GetCompilationUnitRoot(), _currentOptions, syntax.FilePath, Encoding.UTF8);
                    }
                }
#if DEBUG
                else
                {
                    Debug.WriteLine(item.Key + "存在值！");
                }
#endif

            }
        }
    }*/
    private static SyntaxTree GetOptimizationLevelNode(string file, CompilationUnitSyntax root,Encoding? encoding = null)
    {
        return CSharpSyntaxTree.Create(root, _currentOptions, file, encoding == null? Encoding.UTF8 : encoding);
    }

    private static string ReadUtf8File(string file)
    {
        if (!File.Exists(file))
        {
#if DEBUG
            ShowMessage($"不存在文件：{file}");
#endif
            return string.Empty;
        }
        StreamReader stream;
        do
        {
            try
            {
                stream = new(file, Encoding.UTF8);
                var content = stream.ReadToEnd();
                stream.Dispose();
                return content;
            }
            catch (Exception ex)
            {
#if DEBUG
                ShowMessage("命中文件锁！");
#endif

                Thread.Sleep(200);
            }


        } while (true);

    }
    public static void WriteUtf8File(string file, string msg)
    {
        StreamWriter stream;
        do
        {
            try
            {
                stream = new(file,true, Encoding.UTF8);
                stream.Write(msg);
                stream.Dispose();
                return;
            }
            catch (Exception)
            {
#if DEBUG
                ShowMessage("命中文件锁！");
#endif

                Thread.Sleep(200);
            }


        } while (true);

    }
    private static CompilationUnitSyntax HandleToplevelNodes(string file, CompilationUnitSyntax root)
    {
        var firstMember = root.Members[0];
        if (firstMember != null && firstMember.IsKind(SyntaxKind.GlobalStatement))
        {
#if DEBUG
            ShowMessage("检测到顶级语句！");
#endif
            var usings = root.Usings;
            root = root.RemoveNodes(usings, SyntaxRemoveOptions.KeepExteriorTrivia)!;
            var content = "public class Program{ async static Task Main(string[] args){" + root!.ToFullString() + "}}";
            var tree = NatashaCSharpSyntax.ParseTree(content, file, _currentOptions);
            root = tree.GetCompilationUnitRoot();
            root = root.AddUsings([.. usings]);
//#if DEBUG
//            Debug.WriteLine("代理顶级语句：");
//            Debug.WriteLine(root.ToFullString());
//#endif
        }
        return root;
    }
    private static CompilationUnitSyntax HandleImplicitUsings(string file, CompilationUnitSyntax root)
    {
        List<UsingDirectiveSyntax> usingList = [];
        if (_cs0104UsingCache.TryGetValue(file, out var sets))
        {
            if (sets.Count > 0)
            {
                foreach (var node in _defaultUsingNodes)
                {
                    var name = node.Name!.ToString();
                    if (!sets.Contains(name))
                    {
                        usingList.Add(node);
                    }
#if DEBUG
                    else
                    {
                        ShowMessage($"排除 {name}");
                    }
#endif
                }
                return root.AddUsings([.. usingList]);
            }
        }
        return root.AddUsings(_defaultUsingNodes);
        //var usings = root.Usings;
        //if (usings.Count > 0)
        //{
        //    _cs0104FileUsingCache[file] = new HashSet<string>(usings
        //        .Where(item => item.Name != null)
        //        .Select(item => item.Name!.ToFullString()));
        //}
    }
    private static CompilationUnitSyntax HandleMethodReplace(CompilationUnitSyntax root)
    {

        var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
        if (methodDeclarations == null)
        {
            return root;
        }
        Dictionary<MethodDeclarationSyntax, MethodDeclarationSyntax> replaceMethodCache = [];
        foreach (var methodDeclaration in methodDeclarations)
        {
            var methodBody = methodDeclaration.Body;
            if (methodBody == null)
            {
                continue;
            }
            var newStatments = GetNewStatementSyntax(methodBody);

            if (newStatments.HasValue && newStatments.Value.Count > 0)
            {
                replaceMethodCache[methodDeclaration] = methodDeclaration.WithBody(SyntaxFactory.Block(newStatments));
            }
        }


        foreach (var item in replaceMethodCache)
        {
            //#if DEBUG
            //            Debug.WriteLine();
            //            Debug.WriteLine("方法：");
            //            Debug.WriteLine(item.Key.ToFullString());
            //            Debug.WriteLine("替换为：");
            //            Debug.WriteLine(item.Value.ToFullString());
            //            Debug.WriteLine();
            //#endif
            root = root.ReplaceNode(item.Key, item.Value);
        }
        return root;

        static SyntaxList<StatementSyntax>? GetNewStatementSyntax(BlockSyntax methodBody)
        {


            var removeIndexs = new HashSet<int>();
            // 获取方法体
            Dictionary<int, List<StatementSyntax>> addStatementCache = [];
            if (methodBody.OpenBraceToken.HasLeadingTrivia)
            {
                HandleTriviaComment(methodBody.OpenBraceToken.LeadingTrivia, -1, false);
            }
            var statementCount = methodBody.Statements.Count;
            // 遍历方法体中的语句
            for (int i = 0; i < statementCount; i++)
            {
                // 获取当前语句
                var statement = methodBody.Statements[i];

                if (statement.HasLeadingTrivia)
                {
                    var trivias = statement.GetLeadingTrivia();
                    HandleTriviaComment(trivias, i, true);
                }

                if (statement.IsKind(SyntaxKind.LocalFunctionStatement))
                {
                    var localStatementSyntax = (LocalFunctionStatementSyntax)statement;
                    var body = localStatementSyntax.Body;
                    if (body != null)
                    {
                        var newSyntaxList = GetNewStatementSyntax(body);
                        if (newSyntaxList.HasValue && newSyntaxList.Value.Count > 0)
                        {
                            if (!addStatementCache.TryGetValue(i, out var statementList))
                            {
                                addStatementCache[i] = [];
                            }
                            addStatementCache[i].Add(SyntaxFactory
                                .LocalFunctionStatement(
                                    localStatementSyntax.Modifiers,
                                    localStatementSyntax.ReturnType,
                                    localStatementSyntax.Identifier,
                                    localStatementSyntax.TypeParameterList,
                                    localStatementSyntax.ParameterList,
                                    localStatementSyntax.ConstraintClauses,
                                    SyntaxFactory.Block(newSyntaxList),
                                    localStatementSyntax.ExpressionBody
                                 ));
                            removeIndexs.Add(i);
                        }
                        
                    }
                }
                else if (statement.IsKind(SyntaxKind.LocalDeclarationStatement))
                {
                    var localStatementSyntax = (LocalDeclarationStatementSyntax)statement;
                    var lambdaExpression = localStatementSyntax
                        .DescendantNodes()
                        .OfType<ParenthesizedLambdaExpressionSyntax>()
                        .FirstOrDefault();
                    if (lambdaExpression!=null)
                    {
                        var body = lambdaExpression.Block;
                        if (body != null)
                        {
                            var newSyntaxList = GetNewStatementSyntax(body);
                            if (newSyntaxList.HasValue && newSyntaxList.Value.Count > 0)
                            {
                                if (!addStatementCache.TryGetValue(i, out var statementList))
                                {
                                    addStatementCache[i] = [];
                                }
                                addStatementCache[i]
                                    .Add(localStatementSyntax.ReplaceNode(
                                        lambdaExpression, lambdaExpression.WithBlock(SyntaxFactory.Block(newSyntaxList))));
                                removeIndexs.Add(i);
                            }
                        }
                    }
                }


                

                if (statement.HasTrailingTrivia)
                {
                    var trivias = statement.GetTrailingTrivia();
                    HandleTriviaComment(trivias, i, true);
                }
                
            }

            if (methodBody.CloseBraceToken.HasLeadingTrivia)
            {
                HandleTriviaComment(methodBody.CloseBraceToken.LeadingTrivia, -2, false);
            }
            // 如果找到，创建新的方法体列表并排除该语句
            if (removeIndexs.Count > 0 || addStatementCache.Count > 0)
            {
                var statements = methodBody.Statements;
                List<StatementSyntax> newStatments = [];
                if (addStatementCache.TryGetValue(-1, out var headList))
                {
                    newStatments.AddRange(headList);
                }
                if (statements.Count != 0)
                {
                    for (int i = 0; i < statements.Count; i++)
                    {
                        if (addStatementCache.ContainsKey(i))
                        {
                            newStatments.AddRange(addStatementCache[i]);
                        }
                        if (!removeIndexs.Contains(i))
                        {
                            newStatments.Add(statements[i]);
                        }
                    }
                }
                if (addStatementCache.TryGetValue(-2, out var tailList))
                {
                    newStatments.AddRange(tailList);
                }
                return new SyntaxList<StatementSyntax>(newStatments);
            }

            return null;
            void HandleTriviaComment(SyntaxTriviaList trivias, int i, bool needDeleted)
            {
                foreach (var trivia in trivias)
                {

                    if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                    {
                        var commentText = trivia.ToString();
                        if (commentText.Length > 2)
                        {
                            var commentLowerText = commentText.Trim().ToLower();
                            if (!IsRelease)
                            {
                                if (commentLowerText.StartsWith(_proxyCommentDebugShow))
                                {
                                    var length = _proxyCommentDebugShow.Length + 1;
                                    if (commentText.Length > length)
                                    {
                                        var statementNode = CreatePreprocessorConsoleWriteLineSyntaxNode(
                                        commentText.Substring(length, commentText.Length - length));
                                        if (!addStatementCache.TryGetValue(i, out var statementList))
                                        {
                                            addStatementCache[i] = [];
                                        }
                                        addStatementCache[i].Add(statementNode);
                                    }
                                }
                            }
                            else
                            {
                                if (commentLowerText.StartsWith(_proxyCommentReleaseShow))
                                {
                                    var length = _proxyCommentReleaseShow.Length + 1;
                                    if (commentText.Length > length)
                                    {
                                        var statementNode = CreatePreprocessorConsoleWriteLineSyntaxNode(
                                        commentText.Substring(length, commentText.Length - length));
                                        if (!addStatementCache.TryGetValue(i, out var statementList))
                                        {
                                            addStatementCache[i] = [];
                                        }
                                        addStatementCache[i].Add(statementNode);
                                    }
                                }
                            }
                            if (needDeleted)
                            {
                                if (commentLowerText.StartsWith(_commentTag))
                                {
                                    removeIndexs.Add(i);
                                }
                            }
                        }
                    }
                }
            }
        }

    }

    private static StatementSyntax CreatePreprocessorConsoleWriteLineSyntaxNode(string content)
    {
        if (_projectKind == HEProjectKind.AspnetCore || _projectKind == HEProjectKind.Console)
        {
            return SyntaxFactory.ParseStatement($"Console.WriteLine({content});");
        }
        return SyntaxFactory.ParseStatement($"HEProxy.ShowMessage(({content}).ToString());");
    }
    private static CompilationUnitSyntax HandlePickedProxyMethod(CompilationUnitSyntax root)
    {
        var proxyMethod = root.DescendantNodes()
                             .OfType<MethodDeclarationSyntax>()
                             .FirstOrDefault(m => m.Identifier.Text == _proxyMethodName);

        if (proxyMethod != null)
        {
            HandleOptimizationLevel(proxyMethod);
            ClassDeclarationSyntax? parentClass = proxyMethod.Parent as ClassDeclarationSyntax ?? throw new Exception($"获取 {_proxyMethodName} 方法类名出现错误！");
            _className = parentClass.Identifier.Text;
            if (proxyMethod.Body!=null)
            {
                var newBody = HandleProjectInjectCode(proxyMethod.Body);
                if (newBody != null)
                {
                    root = root.ReplaceNode(proxyMethod.Body, newBody);
                }
            }
            
        }

        return root;
    }
    private static BlockSyntax? HandleProjectInjectCode(BlockSyntax blockSyntax)
    {
        if (_projectKind == HEProjectKind.Winform)
        {
            StatementSyntax? runNode = null;
            foreach (var item in blockSyntax.Statements)
            {
                if (item.ToString().StartsWith("Application.Run"))
                {
                    runNode = item;
                }
            }
            if (runNode != null)
            {
                return blockSyntax.ReplaceNode(runNode, [SyntaxFactory.ParseStatement(@$"HEProxy.SetAftHotExecut(() => {{
                    Task.Run(() => {{
                        for (int i = 0; i < Application.OpenForms.Count; i++)
                        {{
                            var form = Application.OpenForms[i];
                            if (form != null)
                            {{
                                try
                                {{
                                    form.Dispose();
                                }}
                                catch
                                {{

                                }}
                            }}
                        }}
try{{
    {runNode}
}}catch(Exception ex)
{{
    HEProxy.ShowMessage(ex.Message);
}}
                        
                    }});
                }});")]);
            }
        }
        else if (_projectKind == HEProjectKind.WPF)
        {
            return blockSyntax.WithStatements([SyntaxFactory.ParseStatement(@$"HEProxy.SetAftHotExecut(() => {{
                    Task.Run(() => {{
                       for (int i = 0; i < Application.Current.Windows.Count; i++)
                       {{
                            var window = Application.Current.Windows[i];
                            try{{
                                window.Close();
                            }}catch{{

                            }}
                        }}
                       Application.Current.Run();
                    }});
                }});")]);
        }
        else if (_projectKind == HEProjectKind.Console)
        {
            if (blockSyntax.Statements.Count>0)
            {
                var node = blockSyntax.Statements.Last();
                if (node.ToString() =="Console.ReadKey();")
                {
                    return blockSyntax.RemoveNode(node, SyntaxRemoveOptions.KeepExteriorTrivia);
                }
            }
        }
        return null;
    }
    private static void HandleOptimizationLevel(MethodDeclarationSyntax methodNode)
    {
        var body = methodNode.Body;
        if (body == null)
        {
            return;
        }
        foreach (SyntaxNode node in body.DescendantNodesAndSelf())
        {
            foreach (SyntaxTrivia trivia in node.GetLeadingTrivia())
            {
                if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                {
                    var commentText = trivia.ToString().Trim().ToLower();
                    if (commentText.StartsWith(_proxyCommentOPLDebug))
                    {
                        IsRelease = false;
                        return;
                    }
                    else if (commentText.StartsWith(_proxyCommentOPLRelease))
                    {
                        IsRelease = true;
                        return;
                    }
                }
            }
        }
    }

    private static void HandleCSO1O4Usings(string file, CompilationUnitSyntax root)
    {
        HashSet<string> tempSets = [];
        HashSet<SyntaxTrivia> triviaSets = [];
        foreach (SyntaxNode node in root.DescendantNodesAndSelf())
        {
            foreach (SyntaxTrivia trivia in node.GetLeadingTrivia())
            {
                if (triviaSets.Contains(trivia))
                {
                    continue;
                }
                if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                {
                    var commentText = trivia.ToString().Trim().ToLower();
                    if (commentText.Length > _proxyCommentCS0104Using.Length)
                    {
                        if (commentText.StartsWith(_proxyCommentCS0104Using))
                        {
                            triviaSets.Add(trivia);
                            //#if DEBUG
                            //                            Debug.WriteLine($"找到剔除点：{commentText}");
                            //                            Debug.WriteLine($"整个节点为：{node.ToFullString()}");
                            //#endif
                            var usingStrings = trivia.ToString().Trim().Substring(_proxyCommentCS0104Using.Length, commentText.Length - _proxyCommentCS0104Using.Length);
                            tempSets.UnionWith(usingStrings.Split([';'], StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()));
                        }
                    }
                }
            }
        }
        _cs0104UsingCache[file] = tempSets;
    }
    /*
    private static void HandleOptimizationLevel(string file, CompilationUnitSyntax root) {

        foreach (SyntaxNode node in root.DescendantNodesAndSelf())
        {
            foreach (SyntaxTrivia trivia in node.GetLeadingTrivia())
            {
                if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
                {

                    if (trivia.ToString().Trim().ToLower().StartsWith(_proxyOPLCommentDebug))
                    {
#if DEBUG
                        Debug.WriteLine($"{file} 中找到匹配的 DEBUG 节点: {trivia}");
                        Thread.Sleep(5000);
#endif
                        _mixOPLCommentCache[file] = _debugOptions;
                        return;
                    }
                    else if (trivia.ToString().Trim().ToLower().StartsWith(_proxyOPLCommentRelease))
                    {
#if DEBUG
                        Debug.WriteLine($"{file} 中找到匹配的 RELEASE 节点: {trivia}");
                        Thread.Sleep(5000);
#endif

                        _mixOPLCommentCache[file] = _releaseOptions;
                        return;
                    }
                }
            }
            _mixOPLCommentCache[file] = null;
        }
    }*/
    #endregion

    #region 普通 API 区
    /// <summary>
    /// 重执行之前需要做的工作
    /// </summary>
    /// <param name="callback"></param>
    public static void SetPreHotExecut(Action callback)
    {
        _preCallback = callback;
    }
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
        _proxyCommentOPLDebug = comment.Trim().ToLower();
    }
    public static void SetReleaseCommentTag(string comment)
    {
        _proxyCommentOPLRelease = comment.Trim().ToLower();
    }
    public static void SetCS0104CommentTag(string comment)
    {
        _proxyCommentCS0104Using = comment.Trim().ToLower();
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

