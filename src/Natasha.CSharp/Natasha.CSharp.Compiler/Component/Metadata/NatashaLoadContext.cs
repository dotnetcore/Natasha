using Microsoft.CodeAnalysis;
using Natasha.CSharp.Compiler.Component;
using Natasha.DynamicLoad.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class NatashaLoadContext<TCreator> where TCreator : INatashaDynamicLoadContextCreator, new()
{
    static NatashaLoadContext() 
    {
        NatashaLoadContext.SetLoadContextCreator<TCreator>();
    }
    public static void Prepare() { }
}
 public sealed partial class NatashaLoadContext : IDisposable
{
    public INatashaDynamicLoadContextBase Domain;
    private static readonly object _initLock = new();
    private static bool _isInitialized;
    internal NatashaLoadContext(NatashaUsingCache? usingCache = null)
    {
        if (usingCache!=null)
        {
            UsingRecorder = usingCache;
        }
        else
        {
            UsingRecorder = new();
        }
        Domain = Creator.CreateDefaultContext();
        Domain.SetCallerReference(this);
    }
    internal NatashaLoadContext(string key, NatashaUsingCache? usingCache = null)
    {
        if (usingCache != null)
        {
            UsingRecorder = usingCache;
        }
        else
        {
            UsingRecorder = new();
        }
        Domain = Creator.CreateContext(key);
        Domain.SetCallerReference(this);
    }

    internal NatashaLoadContext(INatashaDynamicLoadContextBase domain, NatashaUsingCache? usingCache = null)
    {
        if (usingCache != null)
        {
            UsingRecorder = usingCache;
        }
        else
        {
            UsingRecorder = new();
        }
        Domain = domain;
        Domain.SetCallerReference(this);
    }

    public static INatashaDynamicLoadContextCreator Creator = default!;
    public const string DefaultName = "Default";

    internal static void SetLoadContextCreator<TCreator>() where TCreator : INatashaDynamicLoadContextCreator, new()
    {
        if (!_isInitialized)
        {
            lock (_initLock)
            {
                if (!_isInitialized)
                {
                    _isInitialized = true;
                    Creator = new TCreator();
                    DefaultContext = new NatashaLoadContext();
                    DomainManagement.Add(DefaultName, DefaultContext);
                }
            }
        }
    }

    public static NatashaLoadContext DefaultContext = default!;

    public IEnumerable<MetadataReference> GetReferences(ReferenceConfiguration configuration)
    {
        if (Domain.Name == DefaultContext.Domain.Name)
        {
            return ReferenceRecorder.GetReferences();
        }
        else
        {
            return ReferenceRecorder.CombineWithDefaultReferences(DefaultContext.ReferenceRecorder, configuration._compileReferenceBehavior, configuration._referenceSameNamePickFunc);
        }
    }

    /// <summary>
    /// 引用 记录
    /// </summary>
    public readonly NatashaMetadataCache ReferenceRecorder = new();
    /// <summary>
    /// Using 记录
    /// </summary>
    public readonly NatashaUsingCache UsingRecorder;

 
    public NatashaLoadContext AppendReference
        (AssemblyName name, MetadataReference metadataReference, AssemblyCompareInformation compareInfomation = AssemblyCompareInformation.None)
    {
        ReferenceRecorder.AddReference(name, metadataReference, compareInfomation);
        return this;
    }
    public NatashaLoadContext AppendUsings(HashSet<string> usings)
    {
        UsingRecorder.Using(usings);
        return this;
    }

    public void Dispose()
    {
        ReferenceRecorder.Dispose();
        UsingRecorder.Dispose();
        Domain.Dispose();
    }
}