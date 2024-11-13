using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Natasha.CSharp.Compiler.Utils;
using System.Collections.Generic;
using System.Diagnostics;

public static class NatashaStringExtension
{
    /// <summary>
    /// 给当前类脚本增加私有注解
    /// </summary>
    /// <param name="script"></param>
    /// <param name="objects">可以是私有成员实例/命名空间字符串“interal.aa”/私有类型</param>
    /// <returns></returns>
    public static CompilationUnitSyntax ToAccessPrivateTree(this string script, params object[] objects)
    {
        var tree = CSharpSyntaxTree.ParseText(script);
        var treeRoot = tree.GetCompilationUnitRoot();
        var rootResult = NatashaPrivateAssemblySyntaxHelper.Handle(tree.GetCompilationUnitRoot(), objects);
        if (rootResult == null)
        {
            return treeRoot;
        }
        return rootResult;
    }

    public static string ToReadonlyScript(this string field)
    {
#if NET8_0_OR_GREATER
        return $"Unsafe.AsRef(in {field})";
#else
        return $"Unsafe.AsRef({field})";
#endif
    }

    /// <summary>
    /// 使用默认域的 using 包装脚本.
    /// </summary>
    /// <param name="script">被所有 using 包装的脚本</param>
    /// <param name="exceptUsings">需要被排除的 using code</param>
    /// <returns></returns>
    public static string WithAllUsingCode(this string script, params string[] exceptUsings)
    {
        return WithAllUsingCode(script, exceptUsings);
    }
    /// <summary>
    /// 使用默认域的 using 包装脚本.
    /// </summary>
    /// <param name="script">被所有 using 包装的脚本</param>
    /// <param name="exceptUsings">需要被排除的 using code</param>
    /// <returns></returns>
    public static string WithAllUsingCode(this string script, IEnumerable<string> exceptUsings)
    {
        var newCache = NatashaLoadContext.DefaultContext.UsingRecorder.WithExceptUsing(exceptUsings);
        return newCache.WrapperScript(script);
    }
}

