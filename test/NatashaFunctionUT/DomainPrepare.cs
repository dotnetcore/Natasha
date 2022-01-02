using Microsoft.CodeAnalysis;
using PluginBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;


public class DomainPrepare
{
    protected static readonly int DefaultAssembliesCount;
    internal static string RuntimeVersion;
    static DomainPrepare()
    {
#if NETCOREAPP3_1
        RuntimeVersion = "netcoreapp3.1";
#elif NET5_0
             RuntimeVersion = "net5.0";
#elif NET6_0_OR_GREATER
            RuntimeVersion = "net6.0";
#endif
        DefaultAssembliesCount = AssemblyLoadContext.Default.Assemblies.Count();
        DomainComponent.Init();
    }

    internal static HashSet<PortableExecutableReference> GetPortableExecutableReferences(LoadBehaviorEnum loadBehavior)
    {
        var domain = DomainManagement.Random();

        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Reference", "Libraries", "DNDV1.dll");
        var assembly = domain.LoadPluginWithHighDependency(path);

        var type1 = assembly.GetTypes().Where(item => item.Name == "P1").First();
        IPluginBase plugin1 = (IPluginBase)(Activator.CreateInstance(type1)!);
        //强制加载所有引用
        var result = plugin1!.PluginMethod1();

        var references = domain._referenceCache.CombineReferences(NatashaDomain.DefaultDomain._referenceCache, loadBehavior);
        var sets = new HashSet<PortableExecutableReference>(references);
        sets.ExceptWith(NatashaDomain.DefaultDomain._referenceCache.GetReferences());
        return sets;
    }
}

