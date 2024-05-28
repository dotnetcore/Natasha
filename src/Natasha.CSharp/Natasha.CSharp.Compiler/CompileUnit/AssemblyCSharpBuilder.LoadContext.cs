using Natasha.DynamicLoad.Base;
using System;

/// <summary>
/// 程序集编译构建器 - 域
/// </summary>
public sealed partial class AssemblyCSharpBuilder 
{
    private NatashaLoadContext _loadContext;
    private readonly DomainConfiguration _domainConfiguration;

    /// <summary>
    /// 配置加载上下文，并立即生效.
    /// <list type="table">
    /// <item>复用 Builder 场景:
    /// <list type="bullet">
    /// <item>
    ///     没有需要处理的，除非想重新创建一个上下文.
    /// </item>
    /// <item>
    ///     别在同一个域创建同名程序集.
    /// </item>
    /// </list>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// 注：[加载上下文] 的内容包括该域成功编译后产生的 [元数据引用]、[UsingCode]. 
    /// </remarks>
    /// <param name="handle"></param>
    /// <returns></returns>
    public AssemblyCSharpBuilder ConfigLoadContext(Func<NatashaLoadContext, NatashaLoadContext> handle)
    {
        _loadContext = handle(_loadContext);
        return this;
    }

    /// <summary>
    /// 编译单元所在的上下文.
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
    /// 编译单元所在域.该域实现了 INatashaDynamicLoadContextBase 接口，并由 INatashaDynamicLoadContextCreator 接口实现创建而来.
    /// </summary>
    public INatashaDynamicLoadContextBase Domain
    {
        get { return _loadContext.Domain;  }
    }
    /// <summary>
    /// 配置域，并立即生效.
    /// </summary>
    /// <remarks>
    /// 注：选项状态会被缓存，复用时无需重复调用.
    /// </remarks>
    /// <param name="action">配置逻辑</param>
    /// <returns>链式对象(调用方法的实例本身).</returns>
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

