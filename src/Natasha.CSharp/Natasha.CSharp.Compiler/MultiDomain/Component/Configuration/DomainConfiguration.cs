
#if NETCOREAPP3_0_OR_GREATER
public class DomainConfiguration
{
    internal AssemblyCompareInfomation _dependencyLoadBehavior = AssemblyCompareInfomation.None;

    /// <summary>
    /// 配置当前域程序集的加载行为
    /// </summary>
    /// <param name="loadBehavior"></param>
    /// <returns></returns>
    public void SetAssemblyLoadBehavior(AssemblyCompareInfomation loadBehavior)
    {
        _dependencyLoadBehavior = loadBehavior;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择较高版本的依赖
    /// </summary>
    /// <returns></returns>
    public void WithHighVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInfomation.UseHighVersion;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择较低版本的依赖
    /// </summary>
    /// <returns></returns>
    public void WithLowVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInfomation.UseLowVersion;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择默认域的依赖程序集
    /// </summary>
    /// <returns></returns>
    public void WithDefaultVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInfomation.UseDefault;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择当前域的依赖程序集
    /// </summary>
    /// <returns></returns>
    public void WithCustomVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInfomation.UseCustom;
    }

}
#endif


