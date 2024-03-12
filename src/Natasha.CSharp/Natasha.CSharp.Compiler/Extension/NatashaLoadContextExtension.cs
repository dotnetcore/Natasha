using System;
using System.Reflection;

public static class NatashaLoadContextExtension
{
    /// <summary>
    /// 根据类型所在的 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <typeparam name="T">要添加引用的类型</typeparam>
    /// <param name="context">Natasha 加载上下文</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    /// <returns></returns>
    public static NatashaLoadContext AddReferenceAndUsingCode<T>(this NatashaLoadContext context, AssemblyCompareInformation loadReferenceBehavior = AssemblyCompareInformation.None)
    {
        return context.AddReferenceAndUsingCode(typeof(T).Assembly, null, loadReferenceBehavior);
    }
    /// <summary>
    /// 根据类型所在的 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <param name="context">Natasha 加载上下文</param>
    /// <param name="type">要添加引用的类型</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static NatashaLoadContext AddReferenceAndUsingCode(this NatashaLoadContext context, Type type, AssemblyCompareInformation loadReferenceBehavior = AssemblyCompareInformation.None)
    {
        return context.AddReferenceAndUsingCode(type.Assembly, null, loadReferenceBehavior);
    }
    /// <summary>
    /// 根据类型所在的 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <param name="context">Natasha 加载上下文</param>
    /// <param name="type">类型</param>
    /// <param name="excludeAssembliesFunc">过滤委托</param>
    public static NatashaLoadContext AddReferenceAndUsingCode(this NatashaLoadContext context, Type type, Func<AssemblyName, bool> excludeAssembliesFunc)
    {
        return context.AddReferenceAndUsingCode(type.Assembly, excludeAssembliesFunc, AssemblyCompareInformation.None);
    }

    /// <summary>
    /// 根据 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <param name="context">Natasha 加载上下文</param>
    /// <param name="assembly">程序集</param>
    /// <param name="excludeAssembliesFunc">过滤委托</param>
    public static NatashaLoadContext AddReferenceAndUsingCode(this NatashaLoadContext context, Assembly assembly, Func<AssemblyName, bool> excludeAssembliesFunc)
    {
        return context.AddReferenceAndUsingCode(assembly, excludeAssembliesFunc, AssemblyCompareInformation.None);
    }

    /// <summary>
    /// 根据 Assmely及其引用的程序集 增加元数据 和 using
    /// </summary>
    /// <param name="context">Natasha 加载上下文</param>
    /// <param name="assembly">程序集</param>
    /// <param name="loadReferenceBehavior">加载行为</param>
    public static NatashaLoadContext AddReferenceAndUsingCode(this NatashaLoadContext context, Assembly assembly, AssemblyCompareInformation loadReferenceBehavior)
    {
        return context.AddReferenceAndUsingCode(assembly, null, loadReferenceBehavior);
    }

}

