#if MULTI
using Microsoft.CodeAnalysis;
using Natasha.CSharp;
using Natasha.CSharp.Component;
using Natasha.CSharp.Component.Domain;
using Natasha.CSharp.Using;
using System;
using System.Collections.Generic;
using System.Reflection;

public sealed class NatashaReferenceDomain : NatashaDomain
{
    public static new readonly NatashaReferenceDomain DefaultDomain;
    static NatashaReferenceDomain()
    {

        DefaultDomain = new NatashaReferenceDomain();
        DomainManagement.Add("Default", DefaultDomain);

    }


    public static void AddDefaultReferenceAndUsing(AssemblyName assemblyName, string path)
    {
        DefaultDomain.References.AddReference(assemblyName, path);
        DefaultUsing.AddUsing(assemblyName);
    }


    public IEnumerable<MetadataReference> GetReferences(PluginLoadBehavior loadBehavior = PluginLoadBehavior.None, Func<AssemblyName, AssemblyName, AssemblyLoadVersionResult>? useAssemblyNameFunc = null)
    {
        if (Name == DefaultDomain.Name)
        {
            return References.GetReferences();
        }
        else
        {
            return References.CombineWithDefaultReferences(DefaultDomain.References, loadBehavior, useAssemblyNameFunc);
        }
    }

    /// <summary>
    /// 引用 记录
    /// </summary>
    public readonly NatashaReferenceCache References;
    /// <summary>
    /// Using 记录
    /// </summary>
    public readonly NatashaUsingCache UsingRecorder;
    private NatashaReferenceDomain() : base()
    {
        References = new();
        UsingRecorder = new();
        LoadAssemblyReferencsWithPath += NatashaReferenceDomain_LoadAssemblyReferencsWithPath;
        LoadAssemblyReferenceWithStream += NatashaReferenceDomain_LoadAssemblyReferenceWithStream;
    }


    public NatashaReferenceDomain(string key) : base(key)
    {
        References = new();
        UsingRecorder = new();
        LoadAssemblyReferencsWithPath += NatashaReferenceDomain_LoadAssemblyReferencsWithPath;
        LoadAssemblyReferenceWithStream += NatashaReferenceDomain_LoadAssemblyReferenceWithStream;
    }


    private void NatashaReferenceDomain_LoadAssemblyReferenceWithStream(Assembly assembly, System.IO.Stream stream)
    {

        References.AddReference(assembly.GetName(), stream);
        if (Name == "Default")
        {
            DefaultUsing.AddUsing(assembly);
        }
        UsingRecorder.Using(assembly);

    }


    private void NatashaReferenceDomain_LoadAssemblyReferencsWithPath(Assembly assembly, string path)
    {
        References.AddReference(assembly.GetName(), path);
        if (Name == "Default")
        {
            DefaultUsing.AddUsing(assembly);
        }
        UsingRecorder.Using(assembly);
    }


    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            LoadAssemblyReferencsWithPath -= NatashaReferenceDomain_LoadAssemblyReferencsWithPath;
            LoadAssemblyReferenceWithStream -= NatashaReferenceDomain_LoadAssemblyReferenceWithStream;
            //References.Clear();
        }
    }

}
#endif