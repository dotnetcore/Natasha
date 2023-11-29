#if NETCOREAPP3_0_OR_GREATER
using System;
using System.Reflection;
using System.Xml.Linq;


public static class NatashaAssemblyBuilderMultiExtension
{
    /// <summary>
    /// 编译单元使用随机域
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseRandomDomain(this AssemblyCSharpBuilder builder)
    {
        builder.Domain = DomainManagement.Random();
        return builder;
    }

    /// <summary>
    /// 编译单元使用新的随机域
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <param name="domainName">域名字</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseNewDomain(this AssemblyCSharpBuilder builder,string domainName)
    {
        builder.Domain = DomainManagement.Create(domainName);
        return builder;
    }

    /// <summary>
    /// 编译单元使用新的随机域
    /// </summary>
    /// <param name="builder">编译单元</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder UseDefaultDomain(this AssemblyCSharpBuilder builder)
    {
        builder.Domain = NatashaReferenceDomain.DefaultDomain;
        return builder;
    }

    /// <summary>
    /// 增加引用 和 using
    /// </summary>
    /// <param name="builder">natasha 编译单元</param>
    /// <param name="type">要添加引用的类型</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static AssemblyCSharpBuilder AddReference(this AssemblyCSharpBuilder builder, Type type, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
    {
        return AddReference(builder, type.Assembly, loadReferenceBehavior);
     }
    /// <summary>
    /// 增加引用 和 using
    /// </summary>
    /// <param name="builder">natasha 编译单元</param>
    /// <param name="assembly">程序集</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static AssemblyCSharpBuilder AddReference(this AssemblyCSharpBuilder builder, Assembly assembly, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
    {
        var name = assembly.GetName();
        var domain = builder.Domain;
        var reference = domain.References.GetSingleReference(name);
        if (reference == null)
        {
            domain.References.AddReference(assembly);
            domain.UsingRecorder.Using(assembly);
        }
        return builder;
    }
    /// <summary>
    /// 增加引用 和 using
    /// </summary>
    /// <param name="builder">natasha 编译单元</param>
    /// <param name="path">文件路径</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static AssemblyCSharpBuilder AddReference(this AssemblyCSharpBuilder builder, string path, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
    {
        var resolver = new PathAssemblyResolver([path]);
        using var mlc = new MetadataLoadContext(resolver);
        Assembly assembly = mlc.LoadFromAssemblyPath(path);
        var name = assembly.GetName();
        var domain = builder.Domain;
        var reference = domain.References.GetSingleReference(name);
        if (reference == null)
        {
            domain.References.AddReference(assembly.GetName(), path);
            domain.UsingRecorder.Using(assembly);
        }
        return builder;
    }
}

#endif