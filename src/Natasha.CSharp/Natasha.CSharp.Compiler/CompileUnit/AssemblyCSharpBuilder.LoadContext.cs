using Natasha.DynamicLoad.Base;
using System;
using System.Runtime.CompilerServices;

/// <summary>
/// 程序集编译构建器 - 域
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{
    private NatashaLoadContext _loadContext;
    private readonly DomainConfiguration _domainConfiguration;

    /// <summary>
    /// 配置上下文
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigLoadContext(Func<NatashaLoadContext, NatashaLoadContext> handle)
    {
        _loadContext = handle(_loadContext);
        return this;
    }

    /// <summary>
    /// 编译单元所在的上下文
    /// </summary>
    public NatashaLoadContext LoadContext
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
    /// <summary>
    /// 编译单元所在域.该域实现了 INatashaDynamicLoadContextBase 接口，并由 INatashaDynamicLoadContextCreator 接口实现创建而来
    /// </summary>
    public INatashaDynamicLoadContextBase Domain
    {
        get { return _loadContext.Domain;  }
    }

    /// <summary>
    /// 配置域
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
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

