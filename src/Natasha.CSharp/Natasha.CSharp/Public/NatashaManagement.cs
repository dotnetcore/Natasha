using System;
using System.Reflection;

public static partial class NatashaManagement
{
    /// <summary>
    /// 和 NatashaInitializer.Preheating(); 一样
    /// </summary>
    public static void Preheating(Func<AssemblyName, string?, bool>? excludeReferencesFunc = null)
    {
        NatashaInitializer.Preheating(excludeReferencesFunc);
    }
    

    /// <summary>
    /// 增加全局 Using 引用,其他编译将默认添加该 Using
    /// 例如: AddGlobalUsing("System.IO");
    /// </summary>
    /// <param name="namespaces"></param>
    public static void AddGlobalUsing(params string[] @namespaces)
    {
        DefaultUsing.AddUsing(@namespaces);
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
            DefaultUsing.AddUsing(item);
        }
    }

    /// <summary>
    /// 移除全局 Using 引用
    /// 例如: RemoveGlobalUsing("System.IO");
    /// </summary>
    /// <param name="namespaces"></param>
    public static void RemoveGlobalUsing(params string[] @namespaces)
    {
        DefaultUsing.Remove(@namespaces);
    }
}

