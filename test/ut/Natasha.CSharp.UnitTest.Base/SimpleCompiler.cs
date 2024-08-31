using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;


public static class SimpleCompiler
{
    public static Assembly GetAssemblyForUT(this string script,Action<AssemblyCSharpBuilder>? config = null)
    {
        AssemblyCSharpBuilder builder = new();
        builder.UseRandomLoadContext();
        config?.Invoke(builder);
        builder.Add(script);
        return builder.GetAssembly();
    }
}

