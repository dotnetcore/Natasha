#if MULTI
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
    ///     //程序集默认加载行为, 遇到同名不同版本的依赖也加载.
    ///     domain.SetAssemblyLoadBehavior();
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
            _domain = value;
        }
    }
}
#endif
