using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharp.Core;
using System;
using System.Collections.Generic;


/// <summary>
/// 程序集编译构建器
/// </summary>
public class AssemblyCSharpBuilder 
{

    //语义过滤器
    private readonly List<Func<AssemblyCSharpBuilder, AssemblyCSharpBuilder>> _semanticAnalysistor;
    //域
    //添加语法方法
    //CSharp编译器
    //程序集编译方法





    public AssemblyCSharpBuilder AddSemanticAnalysistor(Func<AssemblyCSharpBuilder, AssemblyCSharpBuilder> func)
    {
        _semanticAnalysistor.Add(func);
        return this;
    }


    
    public AssemblyCSharpBuilder()
    {
        _semanticAnalysistor = new();
        SyntaxTrees = new();
    }

    #region 语法树相关
    public readonly List<SyntaxTree> SyntaxTrees;
    private CSharpParseOptions? _options;
    /// <summary>
    /// 配置语法树选项
    /// </summary>
    /// <param name="cSharpParseOptions"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigSyntaOptions(CSharpParseOptions cSharpParseOptions)
    {
        _options = cSharpParseOptions;
        return this;
    }
    /// <summary>
    /// 配置语法树选项
    /// </summary>
    /// <param name="cSharpParseOptionsAction"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigSyntaOptions(Func<CSharpParseOptions, CSharpParseOptions> cSharpParseOptionsAction)
    {
        _options = cSharpParseOptionsAction(new CSharpParseOptions());
        return this;
    }
    /// <summary>
    /// 添加脚本
    /// </summary>
    /// <param name="script"></param>
    public AssemblyCSharpBuilder Add(string script)
    {
        var tree = NatashaCSharpSyntax.ParseTree(script, _options);
        var exception = NatashaExceptionAnalyzer.GetSyntaxException(tree);
        if (exception != null)
        {
            throw exception;
        }
        else
        {
            SyntaxTrees.Add(tree);
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
            SyntaxTrees.Add(tree);
        }
        return this;
    }
    #endregion

}



