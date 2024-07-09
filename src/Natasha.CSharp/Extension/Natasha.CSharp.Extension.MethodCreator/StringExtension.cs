using Natasha.CSharp.Extension.MethodCreator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public static class StringExtension
{

    public static NatashaSlimMethodBuilder WithSlimMethodBuilder(this string script, Action<AssemblyCSharpBuilder> config)
    {
        var builder = new NatashaSlimMethodBuilder(script);
        builder.ConfigBuilder(config);
        return builder;
    }

    public static NatashaSlimMethodBuilder WithSlimMethodBuilder(this string script, Action<NatashaLoadContext> config)
    {
        var builder = new NatashaSlimMethodBuilder(script);
        builder.ConfigBuilder(config);
        return builder;
    }

    public static NatashaSlimMethodBuilder WithSlimMethodBuilder(this string script)
    {
        var builder = new NatashaSlimMethodBuilder(script);
        builder.WithSimpleBuilder();
        return builder;
    }
}

