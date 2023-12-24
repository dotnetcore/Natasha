#if NETCOREAPP3_0_OR_GREATER
using Microsoft.CodeAnalysis;
using Natasha.CSharp.Compiler.Public.Component.Metadata;
using Natasha.CSharp.Component;
using Natasha.CSharp.Using;
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


    public IEnumerable<MetadataReference> GetReferences(ReferenceConfiguration configuration)
    {
        if (Name == DefaultDomain.Name)
        {
            return References.GetReferences();
        }
        else
        {
            return References.CombineWithDefaultReferences(DefaultDomain.References, configuration._compileReferenceBehavior, configuration._referenceSameNamePickFunc);
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
        References.AddReference(assembly.GetName(), MetadataHelper.CreateMetadataReference(stream), AssemblyCompareInfomation.None);
        UsingRecorder.Using(assembly);
    }


    public void AddReferenceAndUsing(AssemblyName name, MetadataReference metadataReference, HashSet<string> usings, AssemblyCompareInfomation compareInfomation = AssemblyCompareInfomation.None)
    {
        References.AddReference(name, metadataReference, compareInfomation);
        UsingRecorder.Using(usings);
    }


    private void NatashaReferenceDomain_LoadAssemblyReferencsWithPath(Assembly assembly, string path)
    {
        References.AddReference(assembly.GetName(), MetadataHelper.CreateMetadataReference(path), AssemblyCompareInfomation.None);
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