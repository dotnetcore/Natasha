using System;


public static class NatashaManagement
{
    /// <summary>
    /// 和 NatashaInitializer.Preheating(); 一样
    /// </summary>
    public static void Preheating()
    {
        NatashaInitializer.Preheating();
    }
    /// <summary>
    /// 获取系统域
    /// </summary>
    /// <returns></returns>
    public static NatashaReferenceDomain GetDefaultDomain()
    {
        return NatashaReferenceDomain.DefaultDomain;
    }
    /// <summary>
    /// 新建一个域
    /// </summary>
    /// <param name="domainName"></param>
    /// <returns></returns>
    public static NatashaReferenceDomain CreateDomain(string domainName)
    {
        return DomainManagement.Create(domainName);
    }
    /// <summary>
    /// 新建一个随机域
    /// </summary>
    /// <returns></returns>
    public static NatashaReferenceDomain CreateRandomDomain()
    {
        return DomainManagement.Random();
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
    /// 移除全局 Using 引用
    /// 例如: RemoveGlobalUsing("System.IO");
    /// </summary>
    /// <param name="namespaces"></param>
    public static void RemoveGlobalUsing(params string[] @namespaces)
    {
        DefaultUsing.Remove(@namespaces);
    }

    /// <summary>
    /// 增加元数据引用,编译需要元数据支持.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="loadBehavior">加载行为,如果有相同类型的引用, 那么此枚举会比较新旧程序集版本</param>
    /// <returns></returns>
    public static bool AddGlobalReference(Type type, LoadBehaviorEnum loadBehavior = LoadBehaviorEnum.None)
    {
        if (type.Assembly.IsDynamic || type.Assembly.Location == null)
        {
            return false;
        }
        NatashaReferenceDomain.DefaultDomain.References.AddReference(type.Assembly, loadBehavior);
        return true;
    }

    /// <summary>
    /// 移除元数据引用,编译需要元数据支持.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="loadBehavior">加载行为,如果有相同类型的引用, 那么此枚举会比较新旧程序集版本</param>
    /// <returns></returns>
    public static bool RemoveGlobalReference(Type type, LoadBehaviorEnum loadBehavior = LoadBehaviorEnum.None)
    {
        if (type.Assembly.IsDynamic || type.Assembly.GetName() == null)
        {
            return false;
        }
        NatashaReferenceDomain.DefaultDomain.References.RemoveReference(type.Assembly.GetName());
        return true;
    }
}

