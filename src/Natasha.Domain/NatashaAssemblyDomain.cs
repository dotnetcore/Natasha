using Microsoft.CodeAnalysis;
using Natasha.Domain.Template;
using Natasha.Domain.Utils;
using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>
public class NatashaAssemblyDomain : DomainBase
{
    private readonly UsingRecoder _usingsRecoder;
    /// <summary>
    /// 从插件加载来的程序集
    /// </summary>
    private readonly ConcurrentDictionary<string, Assembly> _pluginAssemblies;
    public override HashSet<string> GetReferenceElements()
    {
        return _usingsRecoder._usings;
    }


    /// <summary>
    /// 获取编译所需的引用库
    /// </summary>
    /// <returns></returns>
    public override HashSet<PortableExecutableReference> GetCompileReferences()
    {
        return ReferenceHandler.GetCompileReferences(DefaultDomain, this);
    }


    #region 编译成功事件



    /// <summary>
    /// 当拿到动态编译生成的流时调用
    /// </summary>
    /// <param name="stream">流</param>
    /// <param name="AssemblyName">程序集名</param>
    /// <returns></returns>
    public override Assembly CompileStreamCallback(string dllFile, string pdbFile, Stream stream, string AssemblyName)
    {
        return LoadAssemblyFromStream(stream);
    }
    #endregion



    public NatashaAssemblyDomain(string key) : base(key)
    {
        _pluginAssemblies = new ConcurrentDictionary<string, Assembly>();
        UseNewVersionAssmebly = true;
#if NETCOREAPP3_0_OR_GREATER
        DependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory);
#endif
        _usingsRecoder = new UsingRecoder();
        AddAssemblyEvent += NatashaAssemblyDomain_AddAssemblyEvent;
        RemoveAssemblyEvent += NatashaAssemblyDomain_RemoveAssemblyEvent;
    }

    private void NatashaAssemblyDomain_RemoveAssemblyEvent(Assembly obj)
    {
        foreach (var item in AssemblyReferencesCache)
        {
            _usingsRecoder.Using(item.Key);
        }
        foreach (var item in _pluginAssemblies)
        {
            _usingsRecoder.Using(item.Value);
        }
    }


    private void NatashaAssemblyDomain_AddAssemblyEvent(Assembly obj)
    {
        _usingsRecoder.Using(obj);
    }


    #region 插件
    /// <summary>
    /// 获取当前域的程序集
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<Assembly> GetPluginAssemblies()
    {
        return _pluginAssemblies.Values;
    }


    /// <summary>
    /// 加载插件
    /// </summary>
    /// <param name="path">插件路径</param>
    /// <param name="excludePaths">不需要加载的依赖项</param>
    /// <returns></returns>
    public override Assembly LoadPlugin(string path, params string[] excludePaths)
    {

#if NETCOREAPP3_0_OR_GREATER
        DependencyResolver = new AssemblyDependencyResolver(path);
#endif
        var assembly = LoadAssemblyFromStream(path);
        if (assembly != default)
        {
            _pluginAssemblies[path] = assembly;
        }
        return assembly;

    }


    /// <summary>
    /// 删除插件
    /// </summary>
    /// <param name="path"></param>
    public override void RemovePlugin(string path)
    {
        if (_pluginAssemblies.TryGetValue(path, out var assembly))
        {
            RemoveReference(assembly);
        }
    }
    #endregion



    /// <summary>
    /// 销毁函数
    /// </summary>
    public override void Dispose()
    {
        _usingsRecoder._usingTypes.Clear();
        _pluginAssemblies.Clear();
        base.Dispose();
    }



    #region 默认域解析程序集依赖事件
    public override Assembly Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
    {
        return Load(arg2);
    }


    
    public override IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2)
    {
        return LoadUnmanagedDll(arg2);
    }
    #endregion

    #region 扩展API
    /// <summary>
    /// 添加该类型所在程序集的引用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void AddReferencesFromType<T>()
    {
        AddReferencesFromType(typeof(T));
    }


    /// <summary>
    /// 添加该类型所在程序集的引用
    /// </summary>
    /// <param name="type"></param>
    public void AddReferencesFromType(Type type)
    {
        AddReferencesFromAssembly(type.Assembly);
    }


    /// <summary>
    /// 移除类型所在的程序集引用
    /// </summary>
    /// <param name="type"></param>
    public void RemoveReferencesFromType(Type type)
    {
        RemoveReference(type.Assembly);
    }
    public void RemoveReferencesFromType<T>()
    {
        RemoveReference(typeof(T).Assembly);
    }

    #endregion
}
