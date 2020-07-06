using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using Natasha.Core;
using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;


public class NatashaAssemblyDomain : DomainBase
{

    public readonly ConcurrentDictionary<string, Assembly> DllAssemblies;



    #region 加载程序集
    /// <summary>
    /// 从外部文件获取程序集，并添加引用信息
    /// </summary>
    /// <param name="path">文件路径</param>
    public override Assembly LoadAssemblyFromFile(string path)
    {
        var assembly = base.LoadAssemblyFromFile(path);
        if (assembly != null)
        {
            AddReferencesFromDllFile(path);
        }
        return assembly;
    }


    /// <summary>
    /// 从外部文件以流的方式获取程序集，并添加引用信息
    /// [自动释放]
    /// </summary>
    /// <param name="path">外部文件</param>
    public override Assembly LoadAssemblyFromStream(string path)
    {
        return LoadAssemblyFromStream(new FileStream(path, FileMode.Open, FileAccess.Read));
    }


    /// <summary>
    /// 以流的方式获取程序集，并添加引用信息
    /// [自动释放]
    /// </summary>
    /// <param name="stream">流</param>
    /// <returns></returns>
    public override Assembly LoadAssemblyFromStream(Stream stream)
    {
        using (stream)
        {
            var assembly = base.LoadAssemblyFromStream(stream);
            if (assembly != null)
            {
                stream.Seek(0, SeekOrigin.Begin);
                AddReferencesFromAssemblyStream(assembly, stream);
            }
            return assembly;
        }
    }
    #endregion


    #region 加载插件
    public override Assembly LoadPluginFromFile(string path, params string[] excludePaths)
    {

        AddDeps(path, excludePaths);
        var assembly = base.LoadAssemblyFromFile(path);
        DllAssemblies[path] = assembly;
        return assembly;

    }

    public override Assembly LoadPluginFromStream(string path, params string[] excludePaths)
    {

        AddDeps(path, excludePaths);
        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            var assembly = base.LoadAssemblyFromStream(stream);
            DllAssemblies[path] = assembly;
            return assembly;
        }

    }
    #endregion


    public bool UseNewVersionAssmebly;
    /// <summary>
    /// 获取编译所需的引用库
    /// </summary>
    /// <returns></returns>
    public override HashSet<PortableExecutableReference> GetCompileReferences()
    {

        var defaultNode = (NatashaAssemblyDomain)DefaultDomain;
        if (Name != "default")
        {

            //去除剩余的部分
            var sets = new HashSet<PortableExecutableReference>(defaultNode.ReferencesFromStream.Values);
            foreach (var item in defaultNode.ReferencesFromStream.Keys)
            {

                foreach (var current in ReferencesFromStream.Keys)
                {

                    //如果程序集名相同
                    if (item.GetName().Name == current.GetName().Name)
                    {

                        //是否选用最新程序集
                        if (UseNewVersionAssmebly)
                        {

                            //如果默认域版本小
                            if (item.GetName().Version < current.GetName().Version)
                            {

                                //使用现有域的程序集版本
                                sets.Remove(defaultNode.ReferencesFromStream[item]);
                                break;

                            }
                        }
                        else
                        {
                            sets.Remove(defaultNode.ReferencesFromStream[item]);
                            break;
                        }
                    }
                }
            }

            //先添加流编译的引用
            sets.UnionWith(ReferencesFromStream.Values);
            sets.UnionWith(defaultNode.ReferencesFromFile.Values);
            foreach (var item in defaultNode.ReferencesFromFile.Keys)
            {
                foreach (var current in ReferencesFromFile.Keys)
                {
                    if (item == current) 
                    {
                        sets.Remove(defaultNode.ReferencesFromFile[item]);
                    }
                }
            }
            sets.UnionWith(ReferencesFromFile.Values);
            return sets;

        }
        else
        {
            //如果是系统域则直接拼接自己的引用库
            var sets = new HashSet<PortableExecutableReference>(defaultNode.ReferencesFromStream.Values);
            sets.UnionWith(defaultNode.ReferencesFromFile.Values);
            return sets;
        }

    }

    #region 编译成功事件
    /// <summary>
    /// 当拿到动态编译生成的文件时调用
    /// </summary>
    /// <param name="dllFile">dll文件位置</param>
    /// <param name="pdbFile">pdb文件位置</param>
    /// <param name="AssemblyName">程序集名</param>
    /// <returns></returns>
    public override Assembly CompileFileHandler(string dllFile, string pdbFile, string AssemblyName)
    {
        return LoadAssemblyFromFile(dllFile);
    }


    /// <summary>
    /// 当拿到动态编译生成的流时调用
    /// </summary>
    /// <param name="stream">流</param>
    /// <param name="AssemblyName">程序集名</param>
    /// <returns></returns>
    public override Assembly CompileStreamHandler(Stream stream, string AssemblyName)
    {
        return LoadAssemblyFromStream(stream);
    }
    #endregion


    #region 移除引用
    /// <summary>
    /// 移除对应的短名程序集引用，移除插件方式加载的引用才会用到该方法
    /// </summary>
    /// <param name="path">文件路径</param>
    public override void Remove(string path)
    {
        if (path != null)
        {

            if (DllAssemblies.ContainsKey(path))
            {
                var shortName = Path.GetFileNameWithoutExtension(path);
                while (!ReferencesFromFile.TryRemove(shortName, out _)) { }
                while (!DllAssemblies.TryRemove(path, out _)) { }
            }
        }
    }

    /// <summary>
    /// 移除程序集对应的引用
    /// 优先：移除动态编译的引用
    /// 其次：移除插件加载方式的引用
    /// </summary>
    /// <param name="assembly"></param>
    public override void Remove(Assembly assembly)
    {
        if (assembly != null)
        {
            if (ReferencesFromStream.ContainsKey(assembly))
            {
                while (!ReferencesFromStream.TryRemove(assembly, out _)) { }
            }
            else if (assembly.Location != "" && assembly.Location != default)
            {
                Remove(assembly.Location);
            }
        }
    }
    #endregion


    public NatashaAssemblyDomain(string key) : base(key)
    {
#if !NETSTANDARD2_0
        DependencyResolver = new AssemblyDependencyResolver(AppDomain.CurrentDomain.BaseDirectory);
#endif
        DllAssemblies = new ConcurrentDictionary<string, Assembly>();

    }

    public override void Dispose()
    {
        DllAssemblies.Clear();
        base.Dispose();
    }



    protected override Assembly Load(AssemblyName assemblyName)
    {
#if !NETSTANDARD2_0
        string assemblyPath = DependencyResolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return LoadAssemblyFromStream(assemblyPath);
        }
#endif
        return null;
    }

    public override Assembly Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
    {
        return Load(arg2);
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
#if !NETSTANDARD2_0
        string libraryPath = DependencyResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null)
        {
            return LoadUnmanagedDllFromPath(libraryPath);
        }
#endif
        return IntPtr.Zero;
    }

    public override IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2)
    {
        return LoadUnmanagedDll(arg2);
    }

    public override IEnumerable<Assembly> GetPluginAssemblies()
    {
        return DllAssemblies.Values;
    }
}
