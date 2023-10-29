using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Component.Exception;
using Natasha.CSharp.Syntax;
using System;
using System.Collections.Generic;

/// <summary>
/// 程序集编译构建器 - 语法树相关
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{

    public readonly List<SyntaxTree> SyntaxTrees;
    private CSharpParseOptions? _options;
    private UsingLoadBehavior _parsingBehavior;
    /// <summary>
    /// 配置语法树选项
    /// </summary>
    /// <param name="cSharpParseOptions"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigSyntaxOptions(CSharpParseOptions cSharpParseOptions)
    {
        _options = cSharpParseOptions;
        return this;
    }
    /// <summary>
    /// 配置语法树选项
    /// </summary>
    /// <param name="cSharpParseOptionsAction"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigSyntaxOptions(Func<CSharpParseOptions, CSharpParseOptions> cSharpParseOptionsAction)
    {
        _options = cSharpParseOptionsAction(new CSharpParseOptions());
        return this;
    }

    public AssemblyCSharpBuilder ConfigUsingOptions(UsingLoadBehavior usingLoadBehavior)
    {
        _parsingBehavior = usingLoadBehavior;
        return this;
    }

    /// <summary>
    /// 注入脚本
    /// </summary>
    /// <param name="script">脚本代码</param>
    /// <returns></returns>
    public AssemblyCSharpBuilder Add(string script)
    {
        return Add(script, _parsingBehavior);
    }


    /// <summary>
    /// 添加脚本
    /// </summary>
    /// <param name="script"></param>
    private AssemblyCSharpBuilder AddScript(string script)
    {
        var tree = NatashaCSharpSyntax.ParseTree(script, _options);
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
    /// 添加语法树
    /// </summary>
    /// <param name="tree"></param>
    public AssemblyCSharpBuilder Add(SyntaxTree tree)
    {
        tree = NatashaCSharpSyntax.FormartTree(tree, _options);
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

    public AssemblyCSharpBuilder ClearScript()
    {
        SyntaxTrees.Clear();
        return this;
    }

}



