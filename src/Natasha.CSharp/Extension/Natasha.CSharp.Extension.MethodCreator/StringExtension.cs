﻿using System;
using System.Collections.Generic;
using System.Text;

public static class StringExtension
{

    public static (string script, AssemblyCSharpBuilder builder) WithAssemblyBuilder(this string script, Func<AssemblyCSharpBuilder, AssemblyCSharpBuilder> config)
    {
        AssemblyCSharpBuilder builder = new();
        config?.Invoke(builder);
        return (script, builder);
    }
    public static (string script, AssemblyCSharpBuilder builder) WithAssemblyBuilder(this string script, Func<NatashaLoadContext, NatashaLoadContext> config)
    {
        AssemblyCSharpBuilder builder = new();
        builder.ConfigLoadContext(config);
        return (script, builder);
    }

    public static T? ToDelegate<T>(in this (string script, AssemblyCSharpBuilder builder) buildInfo, string? modifier = null) where T: Delegate
    {

        var className = $"N{Guid.NewGuid():N}";
        var methodInfo = typeof(T).GetMethod("Invoke")!;

        var returnTypeScript = methodInfo.ReturnType.GetDevelopName();
        var parameterScript = new StringBuilder();

        var methodParameters = methodInfo.GetParameters();
        for (int i = 0; i < methodParameters.Length; i+=1)
        {
            parameterScript.Append($"{methodParameters[i].ParameterType.GetDevelopName()} arg{i+1},");
        }
        if (parameterScript.Length > 0)
        {
            parameterScript.Length -= 1;
        }

        buildInfo.builder.Add($"public static class {className} {{ public static {(modifier ?? string.Empty)} {returnTypeScript} Invoke({parameterScript}){{ {buildInfo.script} }} }}");
        var asm = buildInfo.builder.GetAssembly();
        var type = asm.GetType(className);
        if (type != null)
        {
               return (T)Delegate.CreateDelegate(typeof(T), type.GetMethod("Invoke")!);
        }
        return null;
    }
    public static T? ToAsyncDelegate<T>(in this (string script, AssemblyCSharpBuilder builder) buildInfo) where T : Delegate
    {
        return ToDelegate<T>(buildInfo, "async");
    }
    public static T? ToUnsafeDelegate<T>(in this (string script, AssemblyCSharpBuilder builder) buildInfo) where T : Delegate
    {
        return ToDelegate<T>(buildInfo, "unsafe");
    }
    public static T? ToUnsafeAsyncDelegate<T>(in this (string script, AssemblyCSharpBuilder builder) buildInfo) where T : Delegate
    {
        return ToDelegate<T>(buildInfo, "unsafe async");
    }
}

