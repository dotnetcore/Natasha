using Natasha.CSharp.Compiler.Component;
using System.Collections.Generic;
using System.ComponentModel;

public static class NatashaStringExtension
{

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
        var newCache = NatashaLoadContext.DefaultContext.UsingRecorder.WithExpectedUsing(exceptUsings);
        return newCache.WrapperScript(script);
    }
}

