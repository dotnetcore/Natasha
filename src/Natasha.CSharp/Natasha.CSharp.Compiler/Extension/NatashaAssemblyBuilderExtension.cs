﻿public static class NatashaAssemblyBuilderExtension
{
    public static AssemblyCSharpBuilder SetOutputFolder(this AssemblyCSharpBuilder builder, string folder)
    {
        builder.OutputFolder = folder;
        return builder;
    }
    public static AssemblyCSharpBuilder SetDllFilePath(this AssemblyCSharpBuilder builder, string dllFilePath)
    {
        builder.DllFilePath = dllFilePath;
        return builder;
    }
    public static AssemblyCSharpBuilder SetPdbFilePath(this AssemblyCSharpBuilder builder, string pdbFilePath)
    {
        builder.PdbFilePath = pdbFilePath;
        return builder;
    }
    public static AssemblyCSharpBuilder SetCommentFilePath(this AssemblyCSharpBuilder builder, string commentFilePath)
    {
        builder.CommentFilePath = commentFilePath;
        return builder;
    }
    /// <summary>
    /// 编译单元使用随机域
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseRandomDomain(this AssemblyCSharpBuilder builder)
    {
        builder.LoadContext = DomainManagement.Random();
        return builder;
    }

    /// <summary>
    /// 编译单元使用新的随机域
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <param name="domainName">域名字</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseNewDomain(this AssemblyCSharpBuilder builder, string domainName)
    {
        builder.LoadContext = DomainManagement.Create(domainName);
        return builder;
    }

    /// <summary>
    /// 编译单元使用新的随机域
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseDefaultDomain(this AssemblyCSharpBuilder builder)
    {
        builder.LoadContext = NatashaLoadContext.DefaultContext;
        return builder;
    }

}

