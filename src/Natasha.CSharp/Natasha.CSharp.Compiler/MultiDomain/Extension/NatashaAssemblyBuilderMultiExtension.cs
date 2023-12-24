#if NETCOREAPP3_0_OR_GREATER
using Natasha.CSharp.Compiler.Public.Component.Metadata;
using System;
using System.Reflection;
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
    public static AssemblyCSharpBuilder UseNewDomain(this AssemblyCSharpBuilder builder, string domainName)
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
    /// 增加引用 和 using, 会自动查重。
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="type">要添加引用的类型</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static AssemblyCSharpBuilder AddReferenceAndUsingCode(this AssemblyCSharpBuilder builder, Type type, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
    {
        return AddReferenceAndUsingCode(builder, type.Assembly, loadReferenceBehavior);
    }

    /// <summary>
    /// 增加引用 和 using
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="assembly">程序集</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static AssemblyCSharpBuilder AddReferenceAndUsingCode(this AssemblyCSharpBuilder builder, Assembly assembly, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
    {
        var result = MetadataHelper.GetMetadataAndNamespaceFromMemory(assembly, null);
        if (result.HasValue)
        {
            builder.Domain.AddReferenceAndUsing(result.Value.asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
        }
        return builder;
    }

    /// <summary>
    /// 增加引用 和 using
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="path">文件路径</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static AssemblyCSharpBuilder AddReferenceAndUsingCode(this AssemblyCSharpBuilder builder, string path, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
    {
        var result = MetadataHelper.GetMetadataAndNamespaceFromFile(path, null);
        if (result.HasValue)
        {
            builder.Domain.AddReferenceAndUsing(result.Value.asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
        }
        return builder;
    }
    /// <summary>
    /// 将程序集中的元数据引用加到编译单元中
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="assembly">程序集</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder AddDependencyReferences(this AssemblyCSharpBuilder builder, Assembly assembly)
    {
        return builder.WithDependencyReferences(assembly.GetDependencyReferences());
    }
    /// <summary>
    /// 将类型所在的程序集中的元数据引用加到编译单元中
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="type">类型</param>
    /// <returns></returns>
    public static AssemblyCSharpBuilder AddDependencyReferences(this AssemblyCSharpBuilder builder, Type type)
    {
        return builder.WithDependencyReferences(type.GetDependencyReferences());
    }
}

#endif