using Microsoft.CodeAnalysis;
using Natasha.CSharp.Component.Domain;
using Natasha.CSharp.Component.Domain.Core;
using Natasha.CSharp.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;


public class NatashaReferenceDomain : NatashaDomain
{
    public static readonly NatashaReferenceDomain DefaultDomain;
    static NatashaReferenceDomain()
    {
        DefaultDomain = new NatashaReferenceDomain();
        DomainManagement.Add("Default", DefaultDomain);
        DefaultDomainIncrementAssembly += NatashaReferenceDomain_DefaultDomainIncrementAssembly;

    }


    public static void AddDefaultReferenceAndUsing(AssemblyName assemblyName, string path)
    {
        DefaultDomain.References.AddReference(assemblyName, path);
        DefaultUsing.AddUsing(assemblyName);
    }



    private static void NatashaReferenceDomain_DefaultDomainIncrementAssembly(Assembly arg1, string arg2)
    {
        DefaultDomain.NatashaReferenceDomain_LoadAssemblyReferencsWithPath(arg1, arg2);
    }

    public IEnumerable<PortableExecutableReference> GetReferences(LoadBehaviorEnum loadBehavior = LoadBehaviorEnum.None, Func<AssemblyName, AssemblyName, LoadVersionResultEnum>? useAssemblyNameFunc = null)
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
    private readonly UsingRecoder _usingRecoder;
    private NatashaReferenceDomain() : base()
    {
        References = new();
        _usingRecoder = new();
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
        else
        {
            _usingRecoder.Using(assembly);
        }

    }

    private void NatashaReferenceDomain_LoadAssemblyReferencsWithPath(Assembly assembly, string path)
    {
        References.AddReference(assembly.GetName(), path);
        if (Name == "Default")
        {
            DefaultUsing.AddUsing(assembly);
        }
        else
        {
            _usingRecoder.Using(assembly);
        }
    }

    public NatashaReferenceDomain(string key) : base(key)
    {
        References = new();
        _usingRecoder = new();
        LoadAssemblyReferencsWithPath += NatashaReferenceDomain_LoadAssemblyReferencsWithPath;
        LoadAssemblyReferenceWithStream += NatashaReferenceDomain_LoadAssemblyReferenceWithStream;

    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            LoadAssemblyReferencsWithPath -= NatashaReferenceDomain_LoadAssemblyReferencsWithPath;
            LoadAssemblyReferenceWithStream -= NatashaReferenceDomain_LoadAssemblyReferenceWithStream;
            _usingRecoder._usingTypes.Clear();
            References.Clear();
        }
    }

}
