using Natasha.CSharp.Compiler.Component;
using Natasha.DynamicLoad.Base;
using System;
using System.IO;

public static class NatashaAssemblyBuilderExtension
{
    public static AssemblyCSharpBuilder SetOutputFolder(this AssemblyCSharpBuilder builder, string folder)
    {
        builder.OutputFolder = folder;
        return builder;
    }
    public static AssemblyCSharpBuilder SetDllFilePath(this AssemblyCSharpBuilder builder, string dllFilePath)
    {
        var folder = Path.GetDirectoryName(dllFilePath);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        builder.DllFilePath = dllFilePath;
        return builder;
    }
    public static AssemblyCSharpBuilder SetPdbFilePath(this AssemblyCSharpBuilder builder, string pdbFilePath)
    {
        var folder = Path.GetDirectoryName(pdbFilePath);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        builder.PdbFilePath = pdbFilePath;
        return builder;
    }
    public static AssemblyCSharpBuilder SetCommentFilePath(this AssemblyCSharpBuilder builder, string commentFilePath)
    {
        var folder = Path.GetDirectoryName(commentFilePath);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        builder.CommentFilePath = commentFilePath;
        return builder;
    }
    /// <summary>
    /// 编译单元使用随机域
    /// </summary>
    /// <remarks>
    /// 使用 .UseRandomLoadContext 方法替代当前方法.
    /// </remarks>
    /// <param name="builder">编译单元</param>
    /// <returns></returns>
    [Obsolete("为了规范 API 命名，建议您使用 UseRandomLoadContext.", false)]
    public static AssemblyCSharpBuilder UseRandomDomain(this AssemblyCSharpBuilder builder)
    {
        builder.LoadContext = DomainManagement.Random();
        return builder;
    }

    /// <summary>
    /// 编译单元创建指定名称的域，若同名的域已存在且未被回收，则返回已存在的域上下文.
    /// </summary>
    /// <remarks>
    /// 使用 .UseNewLoadContext 方法替代当前方法.
    /// </remarks>
    /// <param name="builder">编译单元</param>
    /// <param name="domainName">域名字</param>
    /// <returns></returns>
    [Obsolete("为了规范 API 命名，建议您使用 UseNewLoadContext.", false)]
    public static AssemblyCSharpBuilder UseNewDomain(this AssemblyCSharpBuilder builder, string domainName)
    {
        builder.LoadContext = DomainManagement.Create(domainName);
        return builder;
    }

    /// <summary>
    /// 编译单元使用默认域
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <returns></returns>
    /// <remarks>
    /// 使用 .UseDefaultLoadContext 方法替代当前方法.
    /// </remarks>
    [Obsolete("为了规范 API 命名，建议您使用 UseDefaultLoadContext.", false)]
    public static AssemblyCSharpBuilder UseDefaultDomain(this AssemblyCSharpBuilder builder)
    {
        builder.LoadContext = NatashaLoadContext.DefaultContext;
        return builder;
    }

    /// <summary>
    /// 编译单元使用随机域上下文
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseRandomLoadContext(this AssemblyCSharpBuilder builder, NatashaUsingCache? usingCache = null)
    {
        builder.LoadContext = DomainManagement.Random(usingCache);
        return builder;
    }

    /// <summary>
    /// 编译单元使用名称创建新的域上下文.
    /// </summary>
    /// <remarks>
    /// (注：若 [同名的域] 未被回收，则返回已存在的域上下文.)
    /// </remarks>
    /// <param name="builder">编译单元</param>
    /// <param name="domainName">域名字</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseNewLoadContext(this AssemblyCSharpBuilder builder, string domainName, NatashaUsingCache? usingCache = null)
    {
        builder.LoadContext = DomainManagement.Create(domainName, usingCache);
        return builder;
    }

    /// <summary>
    /// 使用现有的加载上下文
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <param name="loadContext">加载上下文</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseExistLoadContext(this AssemblyCSharpBuilder builder, NatashaLoadContext loadContext)
    {
        builder.LoadContext = loadContext;
        return builder;
    }
    public static AssemblyCSharpBuilder UseExistLoadContext(this AssemblyCSharpBuilder builder, INatashaDynamicLoadContextBase domain)
    {
        if (domain.Name == null)
        {
            throw new NullReferenceException("domain.Name 为空, 无法从缓存中找到该值！");
        }
        builder.LoadContext = DomainManagement.Create(domain.Name);
        return builder;
    }

    /// <summary>
    /// 编译单元使用默认域上下文
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseDefaultLoadContext(this AssemblyCSharpBuilder builder)
    {
        builder.LoadContext = NatashaLoadContext.DefaultContext;
        return builder;
    }

}

