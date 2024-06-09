using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Component;
using Natasha.CSharp.Extension.HotExecutor;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;


public static class ProjectDynamicProxy
{
    private static readonly FileSystemWatcher _delAndCreateAndRenameWatcher;
    //private static readonly FileSystemWatcher _changeWatcher;
    private static string? _mainFile;
    private static string? _className;
    private static string? _proxyMethodName;
    private static string? _argumentsMethodName;
    private static readonly ConcurrentDictionary<string, SyntaxTree> _fileCache = new();
    private static readonly object _fileLock = new object();
    private static readonly List<string> _args = [];
    private static readonly VSCSharpProcessor _processor;
    private static readonly VSCSharpProjectFileWatcher _csprojWatcher;
    private static bool _isCompiling;
    private static readonly AssemblyCSharpBuilder _builderCache;
    private static bool _isFaildBuild;
    internal static bool IsRelease;
    private readonly static CSharpParseOptions _debugOptions;
    static ProjectDynamicProxy()
    {

        _builderCache = new();
        _builderCache.UseRandomLoadContext();
        _builderCache.UseSmartMode();
        _builderCache.WithoutSemanticCheck();
        _builderCache.WithPreCompilationOptions();
        _builderCache.WithoutPreCompilationReferences();
        _builderCache.WithoutCombineUsingCode();
        _debugOptions = new CSharpParseOptions(LanguageVersion.Preview, preprocessorSymbols: ["DEBUG","RELEASE"]);
        _processor = new();
        _csprojWatcher = new(VSCSharpFolder.CSProjFilePath, async () => {


            if (GetLock())
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
                        ReleaseLock();
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
                        try
                        {
                            Environment.Exit(0);
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                }
                else
                {
                    _isFaildBuild = true;
                    ReleaseLock();
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

        _delAndCreateAndRenameWatcher = new FileSystemWatcher
        {
            Path = VSCSharpFolder.MainCsprojPath,
            Filter = "*.cs",
            EnableRaisingEvents = true,
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.LastWrite
        };

        //_changeWatcher = new FileSystemWatcher
        //{
        //    Path = VSCSharpFolder.MainCsprojPath,
        //    Filter = "*.cs",
        //    EnableRaisingEvents = true,
        //    IncludeSubdirectories = true,
        //    NotifyFilter = NotifyFilters.LastWrite
        //};

        AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
        {
            _delAndCreateAndRenameWatcher.Dispose();
        };

        //_changeWatcher.Changed += async (sender, e) =>
        //{
        //    Debug.WriteLine($"Changed: {e.FullPath}");
        //    Console.WriteLine($"Changed: {e.ChangeType }{e.FullPath}");
        //    if (e.ChangeType == WatcherChangeTypes.Changed)
        //    {
        //        if (CheckFileAvailiable(e.FullPath))
        //        {
        //            ChangeFile(e.FullPath);
        //            await HotExecute();
        //        }
        //    }
        //};

        _delAndCreateAndRenameWatcher.Created += async (sender, e) =>
        {
            if (_isFaildBuild)
            {
                _csprojWatcher.Notify();
                return;
            }
#if DEBUG
       Console.WriteLine($"Created: {e.FullPath}");
#endif
            if (CheckFileAvailiable(e.FullPath))
            {
                CreateFile(e.FullPath);
                await HotExecute();
            }

        };

        _delAndCreateAndRenameWatcher.Deleted += async (sender, e) =>
        {
            if (_isFaildBuild)
            {
                _csprojWatcher.Notify();
                return;
            }
#if DEBUG
       Console.WriteLine($"Deleted: {e.FullPath}");
#endif
            if (CheckFileAvailiable(e.FullPath))
            {
                DeleteFile(e.FullPath);
                await HotExecute();
            }
        };

        _delAndCreateAndRenameWatcher.Renamed += async (sender, e) =>
        {
            if (_isFaildBuild)
            {
                _csprojWatcher.Notify();
                return;
            }
#if DEBUG
        Console.WriteLine($"Renamed: {e.OldFullPath} -> {e.FullPath}");
#endif


            if (e.OldFullPath.EndsWith(".cs"))
            {
                if (e.FullPath.EndsWith(".cs"))
                {
                    if (CheckFileAvailiable(e.FullPath))
                    {
                        CreateFile(e.FullPath);
                    }
                    if (CheckFileAvailiable(e.OldFullPath))
                    {
                        DeleteFile(e.OldFullPath);
                    }
                    await HotExecute();
                }
                else if (e.FullPath.StartsWith(e.OldFullPath) && e.FullPath.EndsWith(".TMP"))
                {
                    if (CheckFileAvailiable(e.OldFullPath))
                    {
                        ChangeFile(e.OldFullPath);
                        await HotExecute();
                    }
                }

            }
        };

        _delAndCreateAndRenameWatcher.Error += Error;

        static void CreateFile(string file)
        {
            var content = ReadFile(file);
            if (file == _mainFile)
            {
                return;
            }
            var tree = NatashaCSharpSyntax.ParseTree(content, _debugOptions);
            _fileCache[file] = tree;
        }

        static void DeleteFile(string file)
        {
            _fileCache.TryRemove(file, out _);
        }

        static void ChangeFile(string file)
        {
            var content = ReadFile(file);
            var tree = NatashaCSharpSyntax.ParseTree(content, _debugOptions);
            _fileCache[file] = tree;
        }

        static void RenameFile(string file)
        {
            DeleteFile(file);
            CreateFile(file);
        }

        
    }
    
    
    public static void BuildWithRelease()
    {
        IsRelease = true;
    }

    public static void BuildWithDebug()
    {
        IsRelease = false;
    }
    
    
    private static int _lockCount = 0;



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool GetLock()
    {
        return Interlocked.CompareExchange(ref _lockCount, 1, 0) == 0;

    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void GetAndWaitLock()
    {
        while (Interlocked.CompareExchange(ref _lockCount, 1, 0) != 0)
        {
            Thread.Sleep(20);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ReleaseLock()
    {

        _lockCount = 0;

    }
    private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
    {
        _delAndCreateAndRenameWatcher.Dispose();
    }

    private static void Error(object sender, ErrorEventArgs e)
    {
        PrintException(e.GetException());
    }
    private static void PrintException(Exception? ex)
    {
        if (ex != null)
        {
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine("Stacktrace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            PrintException(ex.InnerException);
        }
    }

    static bool CheckFileAvailiable(string file)
    {
        if (file.StartsWith(VSCSharpFolder.ObjPath) || file.StartsWith(VSCSharpFolder.BinPath))
        {
            return false;
        }
        return true;
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
    public static void Run(string proxyMethodName = "ProxyMain", string? argumentsMethodName= "ProxyMainArguments")
    {
        _proxyMethodName = proxyMethodName;
        _argumentsMethodName = argumentsMethodName;
        var srcCodeFiles = Directory.GetFiles(VSCSharpFolder.MainCsprojPath, "*.cs", SearchOption.AllDirectories);

        foreach (var file in srcCodeFiles)
        {
            if (CheckFileAvailiable(file))
            {
                var content = ReadFile(file);
                //Console.WriteLine(file);
                //Console.WriteLine(content);
                //Console.WriteLine("------------");
                var tree = NatashaCSharpSyntax.ParseTree(content, null);
                var mainMethod = tree.GetRoot().DescendantNodes()
                                     .OfType<MethodDeclarationSyntax>()
                                     .FirstOrDefault(m => m.Identifier.Text == "Main");

                if (mainMethod != null)
                {
                    ClassDeclarationSyntax? parentClass = mainMethod.Parent as ClassDeclarationSyntax ?? throw new Exception("获取 Main 方法类名出现错误！");
                    _className = parentClass.Identifier.Text;

                    var proxyMethod = tree.GetRoot().DescendantNodes()
                                     .OfType<MethodDeclarationSyntax>()
                                     .FirstOrDefault(m => m.Identifier.Text == proxyMethodName);

                    if (proxyMethod == null)
                    {
                        throw new Exception($"{_className} 中未找到 {proxyMethodName} 代理方法！");
                    }
                    _mainFile = file;
                }
                _fileCache[file] = tree;
            }
        }

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
            _builderCache.WithRandomAssenblyName();
            _builderCache.SyntaxTrees.Clear();
            _builderCache.SyntaxTrees.AddRange(_fileCache.Values);
            if (IsRelease)
            {
                _builderCache.WithReleaseCompile();
            }
            else
            {
                _builderCache.WithDebugCompile();
            }
            var assembly = _builderCache.GetAssembly();
            var types = assembly.GetTypes();
            var typeInfo = assembly.GetTypeFromShortName(_className!);
            var methods = typeInfo.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
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
            if (proxyMethodInfo.GetParameters().Length == 1)
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
            Console.WriteLine($"Error during hot execution: {ex}");
        }
        return Task.CompletedTask;
    }
}

