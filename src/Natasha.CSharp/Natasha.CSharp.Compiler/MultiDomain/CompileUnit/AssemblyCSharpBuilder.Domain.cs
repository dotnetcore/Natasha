#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.Loader;

/// <summary>
/// 程序集编译构建器 - 域
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{
    private NatashaReferenceDomain? _domain;

    /// <summary>
    /// 编译单元所在域.
    /// </summary>
    /// <remarks>
    /// <example>
    /// <code>
    /// 
    ///     //程序集依赖的加载行为将和当前域同步
    ///     domain.SetAssemblyLoadBehavior();
    ///     //编译单元支持的方法:
    ///     WithHighVersionDependency
    ///     WithLowVersionDependency
    ///     WithDefaultVersionDependency
    ///     WithCustomVersionDependency
    /// 
    /// </code>
    /// </example>
    /// </remarks>
    public NatashaReferenceDomain Domain
    {
        get
        {
            if (_domain == null)
            {

                if (AssemblyLoadContext.CurrentContextualReflectionContext != default)
                {
                    _domain = (NatashaReferenceDomain)(AssemblyLoadContext.CurrentContextualReflectionContext);
                }
                else
                {
                    _domain = NatashaReferenceDomain.DefaultDomain;
                }
               
            }
            return _domain;
        }
        set
        {
            if (value == default)
            {
                value = NatashaReferenceDomain.DefaultDomain;
            }
            else
            {
                value.SetAssemblyLoadBehavior(_dependencyLoadBehavior);
            }
            _domain = value;
        }
    }

    private AssemblyCompareInfomation _dependencyLoadBehavior;
    /// <summary>
    /// 配置当前域程序集的加载行为
    /// </summary>
    /// <param name="loadBehavior"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigAssemblyLoadBehavior(AssemblyCompareInfomation loadBehavior)
    {
        _dependencyLoadBehavior = loadBehavior;
        return this;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择较高版本的依赖
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithHighVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInfomation.UseHighVersion;
        if (Domain.Name != "Default")
        {
            Domain.SetAssemblyLoadBehavior(_dependencyLoadBehavior);
        }
        return this;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择较低版本的依赖
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithLowVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInfomation.UseLowVersion;
        if (Domain.Name != "Default")
        {
            Domain.SetAssemblyLoadBehavior(_dependencyLoadBehavior);
        }
        return this;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择默认域的依赖程序集
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithDefaultVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInfomation.UseDefault;
        if (Domain.Name != "Default")
        {
            Domain.SetAssemblyLoadBehavior(_dependencyLoadBehavior);
        }
        return this;
    }
    /// <summary>
    /// 域加载动态程序集或者插件时，若遇到依赖程序集与主域程序集同名，则选择当前域的依赖程序集
    /// </summary>
    /// <returns></returns>
    public AssemblyCSharpBuilder WithCustomVersionDependency()
    {
        _dependencyLoadBehavior = AssemblyCompareInfomation.UseCustom;
        if (Domain.Name != "Default")
        {
            Domain.SetAssemblyLoadBehavior(_dependencyLoadBehavior);
        }
        return this;
    }
}
#endif
