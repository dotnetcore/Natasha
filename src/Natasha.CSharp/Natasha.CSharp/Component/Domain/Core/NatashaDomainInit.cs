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

    public readonly NatashaReferenceCache ReferenceCache;
    private NatashaDomain() : base("Default")
    {

        Default.Resolving += Default_Resolving;
        Default.ResolvingUnmanagedDll += Default_ResolvingUnmanagedDll;
        _pluginAssemblies = new();
        ReferenceCache = new();
        _usingRecoder = new();
        LoadPluginBehavior = LoadBehaviorEnum.UseHighVersion;
        _dependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory!);
    }
    public NatashaDomain(string key) : base(key, true)
    {

        if (key == "Default")
        {
            throw new Exception("不能重复创建主域!");
        }
        LoadPluginBehavior = LoadBehaviorEnum.UseHighVersion;
        _pluginAssemblies = new();
        ReferenceCache = new();
        _usingRecoder = new();
        _dependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory!);

    }


    /// <summary>
    /// 销毁函数
    /// </summary>
    public void Dispose()
    {
        ReferenceCache.Clear();
        _pluginAssemblies.Clear();
    }
}
