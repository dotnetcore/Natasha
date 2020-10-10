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
using System.IO;
using System.Reflection;



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
    public AssemblyCSharpBuilder(string name) : base(name)
    {
        CanRetry = true;
        CompileErrorBehavior = ExceptionBehavior.Throw;
        SyntaxErrorBehavior = ExceptionBehavior.Throw;
        OutputFolder = GlobalOutputFolder;
        CustomUsingShut = false;
        RetryLimit = 2;

    }
    public void CompilerOption(Action<CompilerBase<CSharpCompilation, CSharpCompilationOptions>> action)
    {
        action?.Invoke(Compiler);
    }
    public void SyntaxOptions(Action<SyntaxBase> action)
    {
        action?.Invoke(Syntax);
    }

    public CompilationException Add(string script, HashSet<string> sets = default)
    {

        var tree = Syntax.LoadTreeFromScript(script);
        var exception = NatashaException.GetSyntaxException(tree);
        if (!exception.HasError || SyntaxErrorBehavior == ExceptionBehavior.Ignore)
        {
            Syntax.AddTreeToCache(tree);
            Syntax.ReferenceCache[exception.Formatter] = sets;

        }
        else
        {

            HandlerErrors(exception);

        }
        return exception;

    }




    public CompilationException Add(SyntaxTree node, HashSet<string> sets = default)
    {

        return Add(node.ToString(), sets);

    }




    public CompilationException Add(IScript node)
    {

        return Add(node.Script, node.Usings);

    }




    public CompilationException AddFile(string filePath)
    {

        return Add(File.ReadAllText(filePath));

    }




    private void HandlerErrors(CompilationException exception)
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
            if (Compiler.DllPath == default)
            {
                Compiler.DllPath = Path.Combine(OutputFolder, Compiler.AssemblyName + ".dll");
                Compiler.PdbPath = Path.Combine(OutputFolder, Compiler.AssemblyName + ".pdb");
            }

        }

        //进入编译流程
        Compile();


        //如果编译出错
        if (Compiler.AssemblyResult == null && Exceptions != null && Exceptions.Count > 0)
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
        return Compiler.AssemblyResult;

    }


}


