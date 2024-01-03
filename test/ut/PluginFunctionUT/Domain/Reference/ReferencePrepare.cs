using Microsoft.CodeAnalysis;
using PluginBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Natasha.CSharp.Compiler.Component;
using System.Text;
using System.Diagnostics;

public class ReferencePrepare : DomainPrepare
{
    internal protected static readonly NatashaMetadataCache DefaultReferences;

    static ReferencePrepare()
    {
        NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
        DefaultReferences = NatashaLoadContext.DefaultContext.ReferenceRecorder;
    }
    internal static HashSet<MetadataReference> GetPortableExecutableReferences(AssemblyCompareInfomation loadBehavior)
    {
        var loadContext = DomainManagement.Random();
        var domain = (NatashaDomain)(loadContext.Domain);
        //12 1.6 3.1
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Domain", "Reference", "Libraries", "DNDV1.dll");
        var assembly = domain.LoadPluginWithAllDependency(path,item=>item.Name!.Contains("PluginBase"));
        loadContext.AddReferenceAndUsingCode(assembly, item => item.Name!.Contains("PluginBase"));
        var type1 = assembly.GetTypes().Where(item => item.Name == "P1").First();
        IPluginBase plugin1 = (IPluginBase)(Activator.CreateInstance(type1)!);
        //强制加载所有引用
        var result = plugin1!.PluginMethod1();
        var references = loadContext.ReferenceRecorder.CombineWithDefaultReferences(DefaultReferences, loadBehavior);
        var sets = new HashSet<MetadataReference>(references);
        //在合法的引用中排除默认引用
        sets.ExceptWith(DefaultReferences.GetReferences());
        foreach (var item in sets)
        {
            var asm = loadContext.ReferenceRecorder.GetAssmeblyNameByMetadata(item);
            asm ??= NatashaLoadContext.DefaultContext.ReferenceRecorder.GetAssmeblyNameByMetadata(item);
            Debug.WriteLine("[[[=================="+asm.FullName);
        }
        
        return sets;
    }
}

