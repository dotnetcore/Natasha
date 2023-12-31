using Natasha.CSharp.Compiler.Public.Component.Metadata;
using System.Reflection;
using System;

public static class NatashaAssemblyBuilderExtension
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

            var assemblyNames = assembly.GetReferencedAssemblies();
            if (assemblyNames != null && assemblyNames.Length > 0)
            {
                foreach (var asmName in assemblyNames)
                {
                    if (asmName != null && !builder.Domain.References.HasReference(asmName))
                    {
                        var depAssembly = Assembly.Load(asmName);
                        if (depAssembly != null)
                        {
                            result = MetadataHelper.GetMetadataAndNamespaceFromMemory(depAssembly, null);
                            if (result.HasValue)
                            {
                                builder.Domain.AddReferenceAndUsing(asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
                            }
                        }
                    }
                }

            }
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
}

