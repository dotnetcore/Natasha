using Natasha.DynamicLoad.Base;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// 程序集编译构建器 - 域
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{
    private NatashaLoadContext? _loadContext;
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
    public NatashaLoadContext? LoadContext
    {
        get
        {
            return _loadContext;
        }
        set
        {
            _loadContext = value;
        }
    }

    public INatashaDynamicLoadContextBase? Domain
    {
        get { return _loadContext?.Domain;  }
    }

    public AssemblyCSharpBuilder ConfigDomain(Action<DomainConfiguration> action)
    {
        action(_domainConfiguration);
        this.CheckNullLoadContext();
        if (Domain!.Name != "Default")
        {
            Domain.SetAssemblyLoadBehavior(_domainConfiguration._dependencyLoadBehavior);
        }
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CheckNullLoadContext()
    {
        if (_loadContext == null)
        {
            throw new NullReferenceException("LoadContext 为空！请检查是否调用 NatashaManagement.Preheating 或 NatashaManagement.RegistDomainCreator, 若调用，请检查 Builder 是否创建了域！");
        }
    }
}

