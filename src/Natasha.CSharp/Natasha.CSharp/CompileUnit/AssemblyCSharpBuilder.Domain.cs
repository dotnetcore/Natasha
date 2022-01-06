using Natasha.CSharp.Component.Domain;
using System.Runtime.Loader;

/// <summary>
/// 程序集编译构建器 - 域
/// </summary>
public partial class AssemblyCSharpBuilder 
{
    private NatashaReferenceDomain? _domain;
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



