using Microsoft.CodeAnalysis;
using System.Linq;
using System.Runtime.Loader;


public class DomainPrepare
{

    protected static readonly int DefaultAssembliesCount;
    protected static readonly int DefaultUsingCount;
    protected static string _runtimeVersion;
    static DomainPrepare()
    {

#if NETCOREAPP3_1
        _runtimeVersion = "netcoreapp3.1";
#elif NET5_0
             _runtimeVersion = "net5.0";
#elif NET6_0
        _runtimeVersion = "net6.0";
#elif NET7_0_OR_GREATER
        _runtimeVersion = "net7.0";
#endif
        DefaultAssembliesCount = AssemblyLoadContext.Default.Assemblies.Count();
        NatashaInitializer.Preheating((item, name) => name!.Contains("IO"), true, false);
        DefaultUsingCount = DefaultUsing.Count;
    }

}

