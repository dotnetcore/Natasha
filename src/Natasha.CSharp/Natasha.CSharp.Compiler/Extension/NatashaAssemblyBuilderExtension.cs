using System;
using Natasha.CSharp.Compiler.Component;
using System.Reflection;
using System.Security.Cryptography;

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

    /// <summary>
    /// 根据类型所在的 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="type">要添加引用的类型</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static AssemblyCSharpBuilder AddReferenceAndUsingCode(this AssemblyCSharpBuilder builder, Type type, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
    {
        return AddReferenceAndUsingCode(builder, type.Assembly, null, loadReferenceBehavior);
    }
    /// <summary>
    /// 根据类型所在的 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="type">类型</param>
    /// <param name="excludeAssembliesFunc">过滤委托</param>
    public static AssemblyCSharpBuilder AddReferenceAndUsingCode(this AssemblyCSharpBuilder builder, Type type, Func<AssemblyName, bool> excludeAssembliesFunc)
    {
        return AddReferenceAndUsingCode(builder, type.Assembly, excludeAssembliesFunc, AssemblyCompareInfomation.None);
    }

    /// <summary>
    /// 根据 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="assembly">程序集</param>
    /// <param name="excludeAssembliesFunc">过滤委托</param>
    public static AssemblyCSharpBuilder AddReferenceAndUsingCode(this AssemblyCSharpBuilder builder, Assembly assembly, Func<AssemblyName, bool> excludeAssembliesFunc)
    {
        return AddReferenceAndUsingCode(builder, assembly, excludeAssembliesFunc, AssemblyCompareInfomation.None);
    }

    /// <summary>
    /// 根据 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="assembly">程序集</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static AssemblyCSharpBuilder AddReferenceAndUsingCode(this AssemblyCSharpBuilder builder, Assembly assembly, AssemblyCompareInfomation loadReferenceBehavior)
    {
        return AddReferenceAndUsingCode(builder, assembly, null, loadReferenceBehavior);
    }

    /// <summary>
    /// 根据 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <param name="builder">Natasha 编译单元</param>
    /// <param name="assembly">程序集</param>
    /// <param name="excludeAssembliesFunc">过滤委托</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static AssemblyCSharpBuilder AddReferenceAndUsingCode(this AssemblyCSharpBuilder builder, Assembly assembly, Func<AssemblyName, bool>? excludeAssembliesFunc = null, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
    {
        builder.CheckNullLoadContext();
        var result = MetadataHelper.GetMetadataAndNamespaceFromMemory(assembly, null);
        if (result.HasValue)
        {
            builder.LoadContext!.AddReferenceAndUsing(result.Value.asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
            var assmblies = NatashaLoadContext.Creator.GetDependencyAssemblies(assembly);
            if (assmblies != null)
            {
                if (excludeAssembliesFunc!=null)
                {
                    foreach (var depAssembly in assmblies)
                    {
                        var asmName = depAssembly.GetName();
                        if (!builder.LoadContext.References.HasReference(asmName) && !excludeAssembliesFunc(asmName))
                        {
                            result = MetadataHelper.GetMetadataAndNamespaceFromMemory(depAssembly, null);
                            if (result.HasValue)
                            {
                                builder.LoadContext.AddReferenceAndUsing(asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var depAssembly in assmblies)
                    {
                        System.Diagnostics.Debug.WriteLine("-----------" + depAssembly.FullName);
                        var asmName = depAssembly.GetName();
                        if (!builder.LoadContext.References.HasReference(asmName))
                        {
                            result = MetadataHelper.GetMetadataAndNamespaceFromMemory(depAssembly, null);
                            if (result.HasValue)
                            {
                                builder.LoadContext.AddReferenceAndUsing(asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
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
        builder.CheckNullLoadContext();
        var result = MetadataHelper.GetMetadataAndNamespaceFromFile(path, null);
        if (result.HasValue)
        {
            builder.LoadContext!.AddReferenceAndUsing(result.Value.asmName, result.Value.metadata, result.Value.namespaces, loadReferenceBehavior);
        }
        return builder;
    }
}

