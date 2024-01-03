using Natasha.CSharp.Compiler.Component;
using Natasha.DynamicLoad.Base;
using System;
using System.Reflection;

public static partial class NatashaManagement
{
    public static void Preheating<T>(Func<AssemblyName?, string?, bool>? excludeReferencesFunc,
        bool useRuntimeUsing = false,
        bool useRuntimeReference = false,
        bool useFileCache = false) where T : INatashaDynamicLoadContextCreator, new ()
    {
        RegistDomainCreator<T>();
        NatashaInitializer.Preheating(excludeReferencesFunc, useRuntimeUsing, useRuntimeReference, useFileCache);
    }

    public static void Preheating<T>(
        bool useRuntimeUsing = false,
        bool useRuntimeReference = false,
        bool useFileCache = false) where T : INatashaDynamicLoadContextCreator, new()
    {
        RegistDomainCreator<T>();
        NatashaInitializer.Preheating(null, useRuntimeUsing, useRuntimeReference, useFileCache);
    }

    /// <summary>
    /// 和 NatashaInitializer.Preheating(); 一样
    /// </summary>
    public static void Preheating(
        Func<AssemblyName?, string?, bool>? excludeReferencesFunc,
        bool useRuntimeUsing = false, 
        bool useRuntimeReference = false,
        bool useFileCache = false)
    {
        NatashaInitializer.Preheating(excludeReferencesFunc, useRuntimeUsing, useRuntimeReference, useFileCache);
    }
    public static void Preheating(
        bool useRuntimeUsing = false,
        bool useRuntimeReference = false, 
        bool useFileCache = false)
    {
        NatashaInitializer.Preheating(null, useRuntimeUsing, useRuntimeReference, useFileCache);
    }

    public static void RegistDomainCreator<T>() where T : INatashaDynamicLoadContextCreator, new()
    {
        NatashaLoadContext<T>.Prepare();
    }

    /// <summary>
    /// 增加全局 Using 引用,其他编译将默认添加该 Using
    /// 例如: AddGlobalUsing("System.IO");
    /// </summary>
    /// <param name="namespaces"></param>
    public static void AddGlobalUsing(params string[] @namespaces)
    {
        NatashaLoadContext.DefaultContext.UsingRecorder.Using(@namespaces);
    }

    /// <summary>
    /// 增加全局 Using 引用,其他编译将默认添加该 Using
    /// 例如: AddGlobalUsing("System.IO");
    /// </summary>
    /// <param name="namespaces"></param>
    public static void AddGlobalUsing(params Assembly[] @namespaces)
    {
        foreach (var item in @namespaces)
        {
            NatashaLoadContext.DefaultContext.UsingRecorder.Using(item);


        }
    }

    /// <summary>
    /// 移除全局 Using 引用
    /// 例如: RemoveGlobalUsing("System.IO");
    /// </summary>
    /// <param name="namespaces"></param>
    public static void RemoveGlobalUsing(params string[] @namespaces)
    {
        NatashaLoadContext.DefaultContext.UsingRecorder.Remove(@namespaces);
    }

    /// <summary>
    /// 获取系统域
    /// </summary>
    /// <returns></returns>
    public static NatashaLoadContext GetDefaultDomain()
    {
        return NatashaLoadContext.DefaultContext;
    }
    /// <summary>
    /// 新建一个域
    /// </summary>
    /// <param name="domainName"></param>
    /// <returns></returns>
    public static NatashaLoadContext CreateDomain(string domainName)
    {
        return DomainManagement.Create(domainName);
    }
    /// <summary>
    /// 新建一个随机域
    /// </summary>
    /// <returns></returns>
    public static NatashaLoadContext CreateRandomDomain()
    {
        return DomainManagement.Random();
    }

    /// <summary>
    /// 增加元数据引用,从内存中提取实现程序集并加载到共享域中.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="loadBehavior">加载行为,如果有相同类型的引用, 那么此枚举会比较新旧程序集版本</param>
    /// <returns></returns>
    public static bool AddGlobalReferenceAndUsing(Type type, AssemblyCompareInfomation loadBehavior = AssemblyCompareInfomation.None)
    {
        return AddGlobalReferenceAndUsing(type.Assembly, loadBehavior);
    }
    /// <summary>
    /// 增加元数据引用,从内存中提取实现程序集并加载到共享域中.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="loadBehavior"></param>
    /// <returns></returns>
    public static bool AddGlobalReferenceAndUsing(Assembly assembly, AssemblyCompareInfomation loadBehavior = AssemblyCompareInfomation.None)
    {
        var result = MetadataHelper.GetMetadataAndNamespaceFromMemory(assembly);
        if (result != null)
        {
            NatashaLoadContext.DefaultContext.AddReferenceAndUsing(result.Value.asmName, result.Value.metadata, result.Value.namespaces, loadBehavior);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 增加元数据以及UsingCode, 从文件中提取元数据并加载到共享域中。
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool AddGlobalReferenceAndUsing(string filePath, AssemblyCompareInfomation loadBehavior = AssemblyCompareInfomation.None)
    {
        var result = MetadataHelper.GetMetadataAndNamespaceFromFile(filePath);
        if (result != null)
        {
            NatashaLoadContext.DefaultContext.AddReferenceAndUsing(result.Value.asmName, result.Value.metadata, result.Value.namespaces, loadBehavior);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 移除元数据引用,编译需要元数据支持.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="loadBehavior">加载行为,如果有相同类型的引用, 那么此枚举会比较新旧程序集版本</param>
    /// <returns></returns>
    public static bool RemoveGlobalReference(Type type, AssemblyCompareInfomation loadBehavior = AssemblyCompareInfomation.None)
    {
        if (type.Assembly.IsDynamic || type.Assembly.GetName() == null)
        {
            return false;
        }
        NatashaLoadContext.DefaultContext.ReferenceRecorder.RemoveReference(type.Assembly.GetName());
        return true;
    }
}

