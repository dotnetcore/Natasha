using System.Runtime.Loader;

/// <summary>
/// 程序集编译构建器 - 域
/// </summary>
public partial class AssemblyCSharpBuilder 
{
    private NatashaDomain? _domain;
    public NatashaDomain Domain
    {
        get
        {
            if (_domain == null)
            {

                if (AssemblyLoadContext.CurrentContextualReflectionContext != default)
                {
                    _domain = (NatashaDomain)(AssemblyLoadContext.CurrentContextualReflectionContext);
                }
                else
                {
                    _domain = NatashaDomain.DefaultDomain;
                }

            }
            return _domain;
        }
        set
        {
            if (value == default)
            {
                value = NatashaDomain.DefaultDomain;
            }
            _domain = value;
        }
    }
}



