using Microsoft.CodeAnalysis;
using Natasha.CSharp.Component.Domain.Core;
using Natasha.Domain.Utils;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Loader;

/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>

public partial class NatashaDomain : AssemblyLoadContext, IDisposable
{

    internal readonly NatashaReferenceCache _referenceCache;
    private NatashaDomain() : base("Default")
    {

        Default.Resolving += Default_Resolving;
        Default.ResolvingUnmanagedDll += Default_ResolvingUnmanagedDll;
        _pluginAssemblies = new();
        _referenceCache = new();
        _usingRecoder = new();
        _loadPluginBehavior = LoadBehaviorEnum.None;
        _dependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory!);
    }
    public NatashaDomain(string key) : base(key, true)
    {

        if (key == "Default")
        {
            throw new Exception("不能重复创建主域!");
        }
        _loadPluginBehavior = LoadBehaviorEnum.None;
        _pluginAssemblies = new();
        _referenceCache = new();
        _usingRecoder = new();
        _dependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory!);

    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _usingRecoder._usingTypes.Clear();
            _referenceCache.Clear();
            _pluginAssemblies.Clear();
        }
    }

}
