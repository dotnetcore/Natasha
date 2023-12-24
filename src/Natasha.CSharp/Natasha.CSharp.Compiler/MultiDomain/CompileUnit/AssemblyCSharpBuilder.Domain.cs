#if NETCOREAPP3_0_OR_GREATER
using System;
using System.Runtime.Loader;

/// <summary>
/// 程序集编译构建器 - 域
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{
    private NatashaReferenceDomain? _domain;
    private DomainConfiguration _domainConfiguration;

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
                    if (_domain.AssemblyLoadBehavior != _domainConfiguration._dependencyLoadBehavior)
                    {
                        _domain.SetAssemblyLoadBehavior(_domainConfiguration._dependencyLoadBehavior);
                    }
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
                value.SetAssemblyLoadBehavior(_domainConfiguration._dependencyLoadBehavior);
            }
            _domain = value;
        }
    }

    public AssemblyCSharpBuilder ConfigDomain(Action<DomainConfiguration> action)
    {
        action(_domainConfiguration);
        if (Domain.Name != "Default")
        {
            Domain.SetAssemblyLoadBehavior(_domainConfiguration._dependencyLoadBehavior);
        }
        return this;
    }
}
#endif
