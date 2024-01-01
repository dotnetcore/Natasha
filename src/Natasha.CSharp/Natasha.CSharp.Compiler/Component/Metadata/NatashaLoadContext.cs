using Microsoft.CodeAnalysis;
using Natasha.CSharp.Compiler.Component;
using Natasha.DynamicLoad.Base;
using System;
using System.Collections.Generic;
using System.Reflection;

public static class NatashaLoadContext<TCreator> where TCreator : INatashaDynamicLoadContextCreator, new()
{
    static NatashaLoadContext() 
    {
        NatashaLoadContext.SetLoadContextCreator<TCreator>();
    }
    public static void Prepare() { }
}
public sealed class NatashaLoadContext : IDisposable
{
    public INatashaDynamicLoadContextBase Domain;

    internal NatashaLoadContext()
    {
        Domain = Creator.CreateDefaultContext();
        Domain.SetCallerReference(this);
    }
    internal NatashaLoadContext(string key)
    {
        Domain = Creator.CreateContext(key);
        Domain.SetCallerReference(this);
    }

    public static INatashaDynamicLoadContextCreator Creator = default!;
    public const string DefaultName = "Default";

    public static void SetLoadContextCreator<TCreator>() where TCreator : INatashaDynamicLoadContextCreator, new()
    {
        Creator = new TCreator();
        DefaultContext = new NatashaLoadContext();
        DomainManagement.Add(DefaultName, DefaultContext);
    }

    public static NatashaLoadContext DefaultContext = default!;

    public IEnumerable<MetadataReference> GetReferences(ReferenceConfiguration configuration)
    {
        if (Domain.Name == DefaultContext.Domain.Name)
        {
            return References.GetReferences();
        }
        else
        {
            return References.CombineWithDefaultReferences(DefaultContext.References, configuration._compileReferenceBehavior, configuration._referenceSameNamePickFunc);
        }
    }

    /// <summary>
    /// 引用 记录
    /// </summary>
    public readonly NatashaMetadataCache References = new();
    /// <summary>
    /// Using 记录
    /// </summary>
    public readonly NatashaUsingCache UsingRecorder = new();

    internal void LoadMetadataWithAssembly(Assembly assembly)
    {
        var result = MetadataHelper.GetMetadataAndNamespaceFromMemory(assembly);
        if (result.HasValue)
        {
            AddReferenceAndUsing(result.Value.asmName, result.Value.metadata, result.Value.namespaces);
        }
    }


    public void AddReferenceAndUsing(AssemblyName name, MetadataReference metadataReference, HashSet<string> usings, AssemblyCompareInfomation compareInfomation = AssemblyCompareInfomation.None)
    {
        References.AddReference(name, metadataReference, compareInfomation);
        UsingRecorder.Using(usings);
    }

    public void Dispose()
    {
        References.Dispose();
        UsingRecorder.Dispose();
        Domain.Dispose();
    }
}