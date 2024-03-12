public class DomainConfiguration
{
    internal AssemblyCompareInformation _dependencyLoadBehavior = AssemblyCompareInformation.None;

    /// <summary>
    /// 配置当前域程序集的加载行为
    /// </summary>
    /// <param name="loadBehavior"></param>
    /// <returns></returns>
    public void SetAssemblyLoadBehavior(AssemblyCompareInformation loadBehavior)
    {
        _dependencyLoadBehavior = loadBehavior;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择较高版本的依赖
    /// </summary>
    /// <returns></returns>
    public void WithHighVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInformation.UseHighVersion;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择较低版本的依赖
    /// </summary>
    /// <returns></returns>
    public void WithLowVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInformation.UseLowVersion;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择默认域的依赖程序集
    /// </summary>
    /// <returns></returns>
    public void WithDefaultVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInformation.UseDefault;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择当前域的依赖程序集
    /// </summary>
    /// <returns></returns>
    public void WithCustomVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInformation.UseCustom;
    }

}


