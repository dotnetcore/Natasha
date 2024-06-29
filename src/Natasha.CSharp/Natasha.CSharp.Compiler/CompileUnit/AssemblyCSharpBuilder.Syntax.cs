using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Compiler.Component;
using Natasha.CSharp.Compiler.Component.Exception;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 程序集编译构建器 - 语法树相关
/// </summary>
public sealed partial class AssemblyCSharpBuilder
{

    public readonly List<SyntaxTree> SyntaxTrees;
    private CSharpParseOptions? _options;
    private UsingLoadBehavior _parsingBehavior;
    private Func<string, string>? _scriptHandle;

    public AssemblyCSharpBuilder ConfigScriptHandle(Func<string, string> scriptHandle)
    {
        _scriptHandle = scriptHandle;
        return this;
    }
    /// <summary>
    /// 配置语法树选项.
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <param name="cSharpParseOptions">直接传入语法树选项</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder ConfigSyntaxOptions(CSharpParseOptions cSharpParseOptions)
    {
        _options = cSharpParseOptions;
        return this;
    }
    /// <summary>
    /// 配置语法树选项
    /// (注：该方法为一次性方法，使用过 Clear 方法后需要重新调用以达到您预期的配置)
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <param name="cSharpParseOptionsAction">传入语法树配置逻辑</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder ConfigSyntaxOptions(Func<CSharpParseOptions, CSharpParseOptions> cSharpParseOptionsAction)
    {
        _options = cSharpParseOptionsAction(new CSharpParseOptions());
        return this;
    }

    /// <summary>
    /// 设置 using 引用行为，默认为 None （同 WithoutCombineUsingCode）.
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <param name="usingLoadBehavior"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithCombineUsingCode(UsingLoadBehavior usingLoadBehavior)
    {
        _parsingBehavior = usingLoadBehavior;
        return this;
    }
    /// <summary>
    /// 自定义 using 引用.
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder WithoutCombineUsingCode()
    {
        _parsingBehavior = UsingLoadBehavior.None;
        return this;
    }

    /// <summary>
    /// 注入代码并拼接using，拼接 using 的逻辑与 WithCombineUsingCode 方法设置有关.
    /// </summary>
    /// <remarks>
    /// (注：在开启智能模式后，将自动拼接主域与当前域的 using.)
    /// </remarks>
    /// <param name="script">C#代码</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder Add(string script, Encoding? encoding = null)
    {
        return Add(script, _parsingBehavior, encoding);
    }

    /// <summary>
    /// 添加脚本
    /// </summary>
    /// <param name="script">c#脚本代码</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    private AssemblyCSharpBuilder AddScript(string script, Encoding? encoding = null)
    {
        if (_scriptHandle != null)
        {
            script = _scriptHandle(script);
        }
        var tree = NatashaCSharpSyntax.ParseTree(script, _options, encoding);
        var exception = NatashaExceptionAnalyzer.GetSyntaxException(tree);
        if (exception != null)
        {
            _exception = exception;
            throw exception;
        }
        else
        {
            lock (SyntaxTrees)
            {
                SyntaxTrees.Add(tree);
            }
        }
        return this;
    }

    /// <summary>
    /// 快速添加语法树，无检查.
    /// </summary>
    /// <param name="script"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder FastAddScriptWithoutCheck(string script, Encoding? encoding = null)
    {
        SyntaxTrees.Add(NatashaCSharpSyntax.ParseTree(script, _options, encoding));
        return this;
    }

    /// <summary>
    /// 添加语法树.
    /// </summary>
    /// <remarks>
    /// (注：如果希望跳过检查和格式化，可以使用 FastAddScriptWithoutCheck 方法.)
    /// </remarks>
    /// <param name="tree"></param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder Add(SyntaxTree tree, Encoding? encoding = null)
    {
        tree = NatashaCSharpSyntax.FormartTree(tree, _options, encoding);
        var exception = NatashaExceptionAnalyzer.GetSyntaxException(tree);
        if (exception != null)
        {
            throw exception;
        }
        else
        {
            lock (SyntaxTrees)
            {
                SyntaxTrees.Add(tree);
            }
        }
        return this;
    }
    /// <summary>
    /// 清除编译单元内的语法树
    /// </summary>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder ClearScript()
    {
        SyntaxTrees.Clear();
        return this;
    }

    /// <summary>
    /// 注入代码并拼接using
    /// </summary>
    /// <param name="script">C#代码</param>
    /// <param name="usingLoadBehavior">using 拼接行为</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
    public AssemblyCSharpBuilder Add(string script, UsingLoadBehavior usingLoadBehavior, Encoding? encoding = null)
    {
        switch (usingLoadBehavior)
        {
            case UsingLoadBehavior.WithDefault:
                return AddScript(NatashaLoadContext.DefaultContext.UsingRecorder + script, encoding);
            case UsingLoadBehavior.WithCurrent:
                if (Domain!.Name == NatashaLoadContext.DefaultName)
                {
                    return AddScript(NatashaLoadContext.DefaultContext.UsingRecorder + script, encoding);
                }
                return AddScript(LoadContext!.UsingRecorder + script, encoding);
            case UsingLoadBehavior.WithAll:
                if (Domain!.Name == NatashaLoadContext.DefaultName)
                {
                    return AddScript(NatashaLoadContext.DefaultContext.UsingRecorder + script, encoding);
                }
                StringBuilder usingBuilder = new();
                foreach (var item in LoadContext!.UsingRecorder._usings)
                {
                    if (!NatashaLoadContext.DefaultContext.UsingRecorder.HasUsing(item))
                    {
                        usingBuilder.AppendLine($"using {item};");
                    }
                }
                return AddScript(NatashaLoadContext.DefaultContext.UsingRecorder.ToString() + usingBuilder + script, encoding);
            default:
                return AddScript(script, encoding);
        }
    }

}



