using Microsoft.CodeAnalysis;
using PluginBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Loader;


public class DomainPrepare
{
    protected static readonly int AssembliesCount;
    static DomainPrepare()
    {
        AssembliesCount = AssemblyLoadContext.Default.Assemblies.Count();
        DomainComponent.Init();
    }

    internal static HashSet<PortableExecutableReference> GetPortableExecutableReferences(LoadBehaviorEnum loadBehavior)
    {
        var domain = DomainManagement.Random();
        domain.LoadPluginBehavior = LoadBehaviorEnum.UseHighVersion;

        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Reference", "Libraries", "DNDV1.dll");
        var assembly = domain.LoadPlugin(path);

        var type1 = assembly.GetTypes().Where(item => item.Name == "P1").First();
        IPluginBase plugin1 = (IPluginBase)(Activator.CreateInstance(type1)!);
        //强制加载所有引用
        var result = plugin1!.PluginMethod1();

        var references = domain.ReferenceCache.CombineReferences(NatashaDomain.DefaultDomain.ReferenceCache, loadBehavior);
        var sets = new HashSet<PortableExecutableReference>(references);
        sets.ExceptWith(NatashaDomain.DefaultDomain.ReferenceCache.GetReferences());
        return sets;
    }
}

