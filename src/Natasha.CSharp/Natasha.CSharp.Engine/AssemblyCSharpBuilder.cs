using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha;
using Natasha.CSharpEngine;
using Natasha.CSharpEngine.Error;
using Natasha.CSharpEngine.Log;
using Natasha.Error;
using Natasha.Error.Model;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;


/// <summary>
/// 程序集编译构建器
/// </summary>
public class AssemblyCSharpBuilder : NatashaCSharpEngine
{

    /// <summary>
    /// 默认的输出文件夹
    /// </summary>
    public static string GlobalOutputFolder;
    static AssemblyCSharpBuilder()
    {

        GlobalOutputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "DynamicLibraryFolders");
        if (!Directory.Exists(GlobalOutputFolder))
        {
            Directory.CreateDirectory(GlobalOutputFolder);
        }

    }

    public ExceptionBehavior CompileErrorBehavior;
    public ExceptionBehavior SyntaxErrorBehavior;
    public bool NeedSucceedLog;

    public AssemblyCSharpBuilder() : this(Guid.NewGuid().ToString("N")) { }
    public AssemblyCSharpBuilder(string assemblyName) : base(assemblyName)
    {
        CanRetry = true;
        CompileErrorBehavior = ExceptionBehavior.OnlyThrow;
        SyntaxErrorBehavior = ExceptionBehavior.OnlyThrow;
        OutputFolder = GlobalOutputFolder;
        CustomUsingShut = false;
        NeedSucceedLog = false;
        RetryLimit = 0;
        Compiler.NullableCompileOption = NullableContextOptions.Enable;

    }

    /// <summary>
    /// 配置编译选项
    /// </summary>
    /// <param name="action"></param>
    public void CompilerOption(Action<NatashaCSharpCompiler> action)
    {
        action.Invoke(Compiler);
    }


    /// <summary>
    /// 配置语法树选项
    /// </summary>
    /// <param name="action"></param>
    public void SyntaxOptions(Action<SyntaxBase> action)
    {
        action.Invoke(Syntax);
    }


    /// <summary>
    /// 增加 脚本和 using 引用到构建器中
    /// </summary>
    /// <param name="script">代码脚本</param>
    /// <param name="usings">using 引用</param>
    /// <returns></returns>
    public void Add(string script, HashSet<string>? usings = default)
    {

#if DEBUG
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif
        var tree = Syntax.ConvertToTree(script);
#if DEBUG
        Console.WriteLine();
        stopwatch.StopAndShowCategoreInfo("[SyntaxTree]", "语法树转换耗时", 2);
        stopwatch.Restart();
#endif
        var exception = NatashaExceptionAnalyzer.GetSyntaxException(tree);
#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[SyntaxTree]", "语法树判错耗时", 2);
        stopwatch.Restart();
#endif
        if (!exception.HasError)
        {
            Syntax.AddTreeToCache(tree);
            Syntax.ReferenceCache[exception.Formatter!] = usings;
        }
        else
        {
            HandlerErrors(exception);
        }
#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[SyntaxTree]", "语法树错误处理耗时", 2);
#endif

    }



    /// <summary>
    /// 添加 语法树节点 和 using 引用到构建器中
    /// </summary>
    /// <param name="node">语法树节点</param>
    /// <param name="sets"></param>
    /// <returns></returns>
    public void Add(SyntaxTree node, HashSet<string>? sets = default)
    {

        Add(node.ToString(), sets);

    }



    /// <summary>
    /// 添加语法模板到构建器中
    /// </summary>
    /// <param name="node">语法模板</param>
    /// <returns></returns>
    public void Add(IScript node)
    {

        Add(node.Script, node.Usings);

    }



    /// <summary>
    /// 从文件中添加代码到构建器中
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public void AddFile(string filePath)
    {

        Add(File.ReadAllText(filePath));

    }



    /// <summary>
    /// 错误处理
    /// </summary>
    /// <param name="exception"></param>
    private void HandlerErrors(NatashaException exception)
    {

        if (SyntaxErrorBehavior == ExceptionBehavior.OnlyThrow)
        {

            throw exception;

        }
        else if (SyntaxErrorBehavior == ExceptionBehavior.LogAndThrow)
        {

            LogOperator.ErrorRecoder(exception);
            throw exception;

        }

    }



    public void UseNatashaFileOut()
    {
        if (OutputFolder == GlobalOutputFolder)
        {
            OutputFolder = Path.Combine(GlobalOutputFolder, Compiler.Domain.Name!);
        }
        if (!Directory.Exists(OutputFolder))
        {
            Directory.CreateDirectory(OutputFolder);
        }
        Compiler.DllFilePath = Path.Combine(OutputFolder, $"{Compiler.AssemblyName}.dll");
        Compiler.PdbFilePath = Path.Combine(OutputFolder, $"{Compiler.AssemblyName}.pdb");
        Compiler.XmlFilePath = Path.Combine(OutputFolder, $"{Compiler.AssemblyName}.xml");
    }


    /// <summary>
    /// 获取程序集
    /// </summary>
    /// <returns></returns>
    public Assembly GetAssembly()
    {

        //如果是文件编译，则初始化路径
        //if (Compiler.AssemblyOutputKind == AssemblyBuildKind.File)
        //{

        //    if (OutputFolder == GlobalOutputFolder)
        //    {
        //        OutputFolder = Path.Combine(GlobalOutputFolder, Compiler.Domain.Name!);
        //    }
        //    if (!Directory.Exists(OutputFolder))
        //    {
        //        Directory.CreateDirectory(OutputFolder);
        //    }
        //    if (Compiler.DllFilePath == string.Empty)
        //    {
        //        Compiler.DllFilePath = Path.Combine(OutputFolder, $"{Compiler.AssemblyName}.dll");
        //        Compiler.PdbFilePath = Path.Combine(OutputFolder, $"{Compiler.AssemblyName}.pdb");
        //    }

        //}
        //进入编译流程
        var assembly = Compile();
        //如果编译出错
        if (assembly == null && Exceptions.Count>0)
        {

            switch (CompileErrorBehavior)
            {

                case ExceptionBehavior.LogAndThrow:
                    LogOperator.ErrorRecoder(Compiler.Compilation!, Exceptions);
                    throw Exceptions[0];
                case ExceptionBehavior.OnlyThrow:
                    throw Exceptions[0];
                default:
                    break;

            }

        }
        else if(NeedSucceedLog)
        {

            LogOperator.SucceedRecoder(Compiler.Compilation!);
            
        }
        return assembly!;

    }


}


