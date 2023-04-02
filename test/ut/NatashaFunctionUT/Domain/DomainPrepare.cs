using Microsoft.CodeAnalysis;
using NatashaFunctionUT.Domain.Load;
using System.Linq;
using System.Runtime.Loader;


public class DomainPrepare
{

    protected static readonly int DefaultAssembliesCount;
    protected static readonly int DefaultUsingCount;
    protected static string _runtimeVersion;
    static DomainPrepare()
    {

        var domain = new TestDomain();
#if NETCOREAPP3_1
        _runtimeVersion = "netcoreapp3.1";
#elif NET5_0
             _runtimeVersion = "net5.0";
#elif NET6_0_OR_GREATER
        _runtimeVersion = "net6.0";
#endif
        DefaultAssembliesCount = AssemblyLoadContext.Default.Assemblies.Count();
        NatashaInitializer.Preheating((item, name) => name!.Contains("IO"));
        DefaultUsingCount = DefaultUsing.Count;
    }

}

