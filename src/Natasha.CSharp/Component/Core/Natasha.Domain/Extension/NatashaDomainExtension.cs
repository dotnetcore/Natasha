using System;
using System.Reflection;
using static System.Runtime.Loader.AssemblyLoadContext;

/// <summary>
/// 关于域使用的扩展
/// </summary>
public static class NatashaDomainExtension
{

    /// <summary>
    /// 锁定域的上下文
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static ContextualReflectionScope CreateScope(this NatashaDomain domain)
    {
        return domain.EnterContextualReflection();
    }


    /// <summary>
    /// 如果该插件某个依赖已经在主域中,则使用版本较高的那个.
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="path">插件路径</param>
    /// <param name="excludeAssembliesFunc">排除对应程序集名的依赖项</param>
    /// <returns></returns>
    public static Assembly LoadPluginWithHighDependency(this NatashaDomain domain,string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {
        domain.SetAssemblyLoadBehavior(AssemblyCompareInfomation.UseHighVersion);
        return domain.LoadPlugin(path, excludeAssembliesFunc);
    }


    /// <summary>
    /// 如果该插件某个依赖已经在主域中,则使用版本较低的那个.
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="path">插件路径</param>
    /// <param name="excludeAssembliesFunc">排除对应程序集名的依赖项</param>
    /// <returns></returns>
    public static Assembly LoadPluginWithLowDependency(this NatashaDomain domain, string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {
        domain.SetAssemblyLoadBehavior(AssemblyCompareInfomation.UseLowVersion);
        return domain.LoadPlugin(path, excludeAssembliesFunc);
    }


    /// <summary>
    /// 如果该插件某个依赖已经在主域中,则跳过该依赖项的加载.
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="path">插件路径</param>
    /// <param name="excludeAssembliesFunc">排除对应程序集名的依赖项</param>
    /// <returns></returns>
    public static Assembly LoadPluginUseDefaultDependency(this NatashaDomain domain, string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {
        domain.SetAssemblyLoadBehavior(AssemblyCompareInfomation.UseDefault);
        return domain.LoadPlugin(path, excludeAssembliesFunc);
    }


    /// <summary>
    /// 加载插件和其所有插件
    /// </summary>
    /// <param name="domain"></param>
    /// <param name="path">插件路径</param>
    /// <param name="excludeAssembliesFunc">排除对应程序集名的依赖项</param>
    /// <returns></returns>
    public static Assembly LoadPluginWithAllDependency(this NatashaDomain domain, string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {
        domain.SetAssemblyLoadBehavior(AssemblyCompareInfomation.None);
        return domain.LoadPlugin(path, excludeAssembliesFunc);
    }
}

