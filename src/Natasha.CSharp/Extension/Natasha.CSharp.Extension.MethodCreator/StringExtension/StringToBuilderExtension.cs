using Natasha.CSharp.Extension.MethodCreator;

public static class StringToBuilderExtension
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
    public static NatashaSlimMethodBuilder WithSmartMethodBuilder(this string script)
    {
        var builder = new NatashaSlimMethodBuilder(script);
        builder.WithSmartBuilder();
        return builder;
    }

    public static NatashaSlimMethodBuilder WithoutUsings(this string script, params string[] usings)
    {
        var builder = new NatashaSlimMethodBuilder(script);
        builder.WithSmartBuilder();
        builder.WithoutUsings(usings);
        return builder;
    }
    public static NatashaSlimMethodBuilder WithMetadata(this string script, Type type)
    {
        var builder = new NatashaSlimMethodBuilder(script);
        builder.WithSimpleBuilder();
        builder.WithMetadata(type);
        return builder;
    }
    public static NatashaSlimMethodBuilder WithMetadata<T>(this string script)
    {
        var builder = new NatashaSlimMethodBuilder(script);
        builder.WithSimpleBuilder();
        builder.WithMetadata<T>();
        return builder;
    }
}

