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

        GlobalOutputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DynamicLibraryFolders");
        if (!Directory.Exists(GlobalOutputFolder))
        {
            Directory.CreateDirectory(GlobalOutputFolder);
        }

    }




    public AssemblyCSharpBuilder() : this(Guid.NewGuid().ToString("N")) { }
    public AssemblyCSharpBuilder(string assemblyName) : base(assemblyName)
    {
        CanRetry = true;
        CompileErrorBehavior = ExceptionBehavior.Throw;
        SyntaxErrorBehavior = ExceptionBehavior.Throw;
        OutputFolder = GlobalOutputFolder;
        CustomUsingShut = false;
        RetryLimit = 0;

    }

    /// <summary>
    /// 配置编译选项
    /// </summary>
    /// <param name="action"></param>
    public void CompilerOption(Action<CompilerBase<CSharpCompilation, CSharpCompilationOptions>> action)
    {
        action?.Invoke(Compiler);
    }


    /// <summary>
    /// 配置语法树选项
    /// </summary>
    /// <param name="action"></param>
    public void SyntaxOptions(Action<SyntaxBase> action)
    {
        action?.Invoke(Syntax);
    }


    /// <summary>
    /// 增加 脚本和 using 引用到构建器中
    /// </summary>
    /// <param name="script">代码脚本</param>
    /// <param name="usings">using 引用</param>
    /// <returns></returns>
    public NatashaException Add(string script, HashSet<string> usings = default)
    {

#if DEBUG
        Stopwatch stopwatch = new Stopwatch();
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
        if (!exception.HasError || SyntaxErrorBehavior == ExceptionBehavior.Ignore)
        {
            Syntax.AddTreeToCache(tree);
            Syntax.ReferenceCache[exception.Formatter] = usings;
        }
        else
        {
            HandlerErrors(exception);
        }
#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[SyntaxTree]", "语法树错误处理耗时", 2);
#endif
        return exception;

    }



    /// <summary>
    /// 添加 语法树节点 和 using 引用到构建器中
    /// </summary>
    /// <param name="node">语法树节点</param>
    /// <param name="sets"></param>
    /// <returns></returns>
    public NatashaException Add(SyntaxTree node, HashSet<string> sets = default)
    {

        return Add(node.ToString(), sets);

    }



    /// <summary>
    /// 添加语法模板到构建器中
    /// </summary>
    /// <param name="node">语法模板</param>
    /// <returns></returns>
    public NatashaException Add(IScript node)
    {

        return Add(node.Script, node.Usings);

    }



    /// <summary>
    /// 从文件中添加代码到构建器中
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public NatashaException AddFile(string filePath)
    {

        return Add(File.ReadAllText(filePath));

    }



    /// <summary>
    /// 错误处理
    /// </summary>
    /// <param name="exception"></param>
    private void HandlerErrors(NatashaException exception)
    {

        if (SyntaxErrorBehavior == ExceptionBehavior.Throw)
        {

            throw exception;

        }
        else if (SyntaxErrorBehavior == ExceptionBehavior.Log)
        {

            LogOperator.ErrorRecoder(exception);

        }
        else if (SyntaxErrorBehavior == (ExceptionBehavior.Log | ExceptionBehavior.Throw))
        {

            LogOperator.ErrorRecoder(exception);
            throw exception;

        }

    }



    /// <summary>
    /// 获取程序集
    /// </summary>
    /// <returns></returns>
    public Assembly GetAssembly()
    {

        if (Compiler.AssemblyName == default)
        {
            Compiler.AssemblyName = Guid.NewGuid().ToString("N");
        }


        //如果是文件编译，则初始化路径
        if (Compiler.AssemblyOutputKind == AssemblyBuildKind.File)
        {

            if (OutputFolder == GlobalOutputFolder)
            {
                OutputFolder = Path.Combine(GlobalOutputFolder, Compiler.Domain.Name);
            }
            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }
            if (Compiler.OutputFilePath == default)
            {
                Compiler.OutputFilePath = Path.Combine(OutputFolder, Compiler.AssemblyName + ".dll");
                Compiler.OutputPdbPath = Path.Combine(OutputFolder, Compiler.AssemblyName + ".pdb");
            }

        }
        //进入编译流程
        var assembly = Compile();
        //如果编译出错
        if (assembly == null && Exceptions != null && Exceptions.Count > 0)
        {

            switch (CompileErrorBehavior)
            {

                case ExceptionBehavior.Log | ExceptionBehavior.Throw:
                    LogOperator.ErrorRecoder(Compiler.Compilation, Exceptions);
                    throw Exceptions[0];
                case ExceptionBehavior.Log:
                    LogOperator.ErrorRecoder(Compiler.Compilation, Exceptions);
                    break;
                case ExceptionBehavior.Throw:
                    throw Exceptions[0];
                default:
                    break;

            }

        }
        else
        {

            LogOperator.SucceedRecoder(Compiler.Compilation);
            
        }
        return assembly;

    }


}


