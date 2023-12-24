using System;
using System.Reflection;

public static partial class NatashaManagement
{
#if NETCOREAPP3_0_OR_GREATER
    /// <summary>
    /// 和 NatashaInitializer.Preheating(); 一样
    /// </summary>
    public static void Preheating(
        Func<AssemblyName?, string?, bool>? excludeReferencesFunc = null,
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
#else
        public static void Preheating(
        Func<AssemblyName, string?, bool>? excludeReferencesFunc = null)
    {
        NatashaInitializer.Preheating(excludeReferencesFunc);
    }
#endif


    /// <summary>
    /// 增加全局 Using 引用,其他编译将默认添加该 Using
    /// 例如: AddGlobalUsing("System.IO");
    /// </summary>
    /// <param name="namespaces"></param>
    public static void AddGlobalUsing(params string[] @namespaces)
    {
#if NETCOREAPP3_0_OR_GREATER
        NatashaReferenceDomain.DefaultDomain.UsingRecorder.Using(@namespaces);
#else
        DefaultUsing.AddUsing(@namespaces);
#endif
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
#if NETCOREAPP3_0_OR_GREATER
            NatashaReferenceDomain.DefaultDomain.UsingRecorder.Using(item);
#else
            DefaultUsing.AddUsing(item);
#endif

        }
    }

    /// <summary>
    /// 移除全局 Using 引用
    /// 例如: RemoveGlobalUsing("System.IO");
    /// </summary>
    /// <param name="namespaces"></param>
    public static void RemoveGlobalUsing(params string[] @namespaces)
    {
#if NETCOREAPP3_0_OR_GREATER
        NatashaReferenceDomain.DefaultDomain.UsingRecorder.Remove(@namespaces);
#else
        DefaultUsing.Remove(@namespaces);
#endif

    }
}

