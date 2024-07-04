using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public static class StringExtension
{

    public static (string script, string[] usings, AssemblyCSharpBuilder builder) WithAssemblyBuilder(this string script, Func<AssemblyCSharpBuilder, AssemblyCSharpBuilder> config)
    {
        AssemblyCSharpBuilder builder = new();
        config?.Invoke(builder);
        return (script, [], builder);
    }


    public static (string script, string[] usings, AssemblyCSharpBuilder builder) WithAssemblyBuilder(this string script, Func<NatashaLoadContext, NatashaLoadContext> config)
    {
        AssemblyCSharpBuilder builder = new();
        builder.ConfigLoadContext(config);
        return (script, [], builder);
    }

    public static (string script, string[] usings, AssemblyCSharpBuilder builder) WithSimpleBuilder(this string script)
    {
        AssemblyCSharpBuilder builder = new();
        builder.UseRandomLoadContext();
        builder.UseSimpleMode();
        builder.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode<object>());
        return (script, [], builder);
    }

    public static (string script, string[] usings, AssemblyCSharpBuilder builder) WithMetadata<T>(in this (string script, string[] usings, AssemblyCSharpBuilder builder) buildInfo)
    {
        return (buildInfo.script, buildInfo.usings, buildInfo.builder.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode(typeof(T))));
    }
    public static (string script, string[] usings, AssemblyCSharpBuilder builder) WithMetadata(in this (string script, string[] usings, AssemblyCSharpBuilder builder) buildInfo, Type type)
    {
        return (buildInfo.script, buildInfo.usings, buildInfo.builder.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode(type)));
    }
    public static (string script, string[] usings, AssemblyCSharpBuilder builder) WithUsings(in this (string script, string[] usings, AssemblyCSharpBuilder builder) buildInfo,params string[] usings)
    {
        return (buildInfo.script, usings, buildInfo.builder);
    }
    public static T ToDelegate<T>(in this (string script, string[] usings, AssemblyCSharpBuilder builder) buildInfo, string modifier = "") where T: Delegate
    {
        var scriptBuilder = buildInfo.builder;
        var className = $"N{Guid.NewGuid():N}";
        var methodInfo = typeof(T).GetMethod("Invoke")!;

        var returnTypeScript = methodInfo.ReturnType.GetDevelopName();
        var parameterScript = new StringBuilder();

        var methodParameters = methodInfo.GetParameters();
        for (int i = 0; i < methodParameters.Length; i+=1)
        {
            var paramType = methodParameters[i].ParameterType;
            scriptBuilder.ConfigLoadContext(ctx => ctx.AddReferenceAndUsingCode(paramType));
            parameterScript.Append($"{paramType.GetDevelopName()} arg{i+1},");
        }
        if (parameterScript.Length > 0)
        {
            parameterScript.Length -= 1;
        }
        
        StringBuilder usingCode = new();
        foreach (var item in buildInfo.usings)
	    {
           
            usingCode.AppendLine($"using {item};");
            
        }
        buildInfo.builder.Add($"{usingCode} public static class {className} {{ public static {(modifier ?? string.Empty)} {returnTypeScript} Invoke({parameterScript}){{ {buildInfo.script} }} }}");
        var asm = buildInfo.builder.GetAssembly();
        var type = asm.GetType(className);
        if (type != null)
        {
               return (T)Delegate.CreateDelegate(typeof(T), type.GetMethod("Invoke")!);
        }
        throw new Exception($"未找到 {className} 类型！");
    }
    public static T ToAsyncDelegate<T>(in this (string script, string[] usings, AssemblyCSharpBuilder builder) buildInfo) where T : Delegate
    {
        return ToDelegate<T>(buildInfo, "async");
    }
    public static T ToUnsafeDelegate<T>(in this (string script, string[] usings, AssemblyCSharpBuilder builder) buildInfo) where T : Delegate
    {
        return ToDelegate<T>(buildInfo, "unsafe");
    }
    public static T ToUnsafeAsyncDelegate<T>(in this (string script, string[] usings, AssemblyCSharpBuilder builder) buildInfo) where T : Delegate
    {
        return ToDelegate<T>(buildInfo, "unsafe async");
    }
}

