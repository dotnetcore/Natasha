#if !NETCOREAPP3_0_OR_GREATER
using Natasha.CSharp.Component;
using System;
using System.Reflection;

public static partial class NatashaManagement
{


    /// <summary>
    /// 增加元数据引用,编译需要元数据支持.
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <returns></returns>
    public static bool AddGlobalReference(Assembly assembly)
    {
        NatashaReferenceCache.AddReference(assembly);
        return true;
    }
    /// <summary>
    /// 增加元数据引用,编译需要元数据支持.
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns></returns>
    public static bool AddGlobalReference(Type type)
    {
        if (type.Assembly.IsDynamic || type.Assembly.Location == null)
        {
            return false;
        }
        NatashaReferenceCache.AddReference(type.Assembly);
        return true;
    }
    /// <summary>
    /// 增加元数据引用,编译需要元数据支持.
    /// </summary>
    /// <param name="path">程序集路径</param>
    /// <returns></returns>
    public static bool AddGlobalReference(string path)
    {
        NatashaReferenceCache.AddReference(path);
        return true;
    }

    /// <summary>
    /// 移除元数据引用,编译需要元数据支持.
    /// </summary>
    /// <param name="assembly">程序集</param>
    /// <returns></returns>
    public static bool RemoveGlobalReference(Assembly assembly)
    {
        NatashaReferenceCache.AddReference(assembly);
        return true;
    }

    /// <summary>
    /// 移除元数据引用,编译需要元数据支持.
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns></returns>
    public static bool RemoveGlobalReference(Type type)
    {
        NatashaReferenceCache.AddReference(type.Assembly);
        return true;
    }

    /// <summary>
    /// 移除元数据引用,编译需要元数据支持.
    /// </summary>
    /// <param name="path">程序集路径</param>
    /// <returns></returns>
    public static bool RemoveGlobalReference(string path)
    {
        NatashaReferenceCache.AddReference(path);
        return true;
    }
}
#endif