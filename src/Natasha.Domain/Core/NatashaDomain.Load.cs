using Natasha.CSharp.Component.Domain;
using Natasha.Domain.Extension;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>
public partial class NatashaDomain
{

    private LoadBehaviorEnum _assemblyLoadBehavior;

    public void SetAssemblyLoadBehavior(LoadBehaviorEnum loadBehavior)
    {
        _assemblyLoadBehavior = loadBehavior;
    }

    /// <summary>
    /// 依赖解析库
    /// </summary>
    private AssemblyDependencyResolver _dependencyResolver;
    /// <summary>
    /// 排除插件引用
    /// </summary>
    private Func<AssemblyName, bool> _excludePluginReferencesFunc;


    /// <summary>
    /// 将文件转换为程序集，并加载到域
    /// </summary>
    /// <param name="path">外部文件</param>
    /// <returns></returns>
    public virtual Assembly LoadAssemblyFromFile(string path)
    {

#if DEBUG
        Debug.WriteLine($"[加载]路径:{path}.");
#endif
        Assembly assembly;
        if (Name == "Default")
        {
            assembly = Default.LoadFromAssemblyPath(path);
        }
        else
        {
            assembly = LoadFromAssemblyPath(path);
        }
        LoadAssemblyReferencsWithPath?.Invoke(assembly, path);
        return assembly;

    }


    /// <summary>
    /// 将流转换为程序集，并加载到域
    /// [手动释放]
    /// </summary>
    /// <param name="stream">外部流</param>
    /// <returns></returns>
    public virtual Assembly LoadAssemblyFromStream(Stream stream,Stream? pdbStream)
    {
        using (stream)
        {

            Assembly assembly;
            if (Name == "Default")
            {
                assembly = Default.LoadFromStream(stream, pdbStream);
            }
            else
            {
                assembly = LoadFromStream(stream, pdbStream);
            }

            stream.Seek(0, SeekOrigin.Begin);
            LoadAssemblyReferenceWithStream?.Invoke(assembly, stream);
            return assembly;

        }
    }

    /// <summary>
    /// 对程序集上下文的重载函数，注：系统规定需要重载
    /// </summary>
    /// <param name="assemblyName">程序集名</param>
    /// <returns></returns>
    protected override Assembly? Load(AssemblyName assemblyName)
    {
#if DEBUG
        Debug.WriteLine($"[解析]程序集:{assemblyName.Name},全名:{assemblyName.FullName}");
#endif
        if (_assemblyLoadBehavior != LoadBehaviorEnum.None && Name != "Default")
        {
            var name = assemblyName.GetUniqueName();
            if (_defaultAssemblyNameCache.TryGetValue(name!, out var defaultCacheName))
            {
                if (assemblyName.CompareWithDefault(defaultCacheName, _assemblyLoadBehavior) == LoadVersionResultEnum.UseDefault)
                {
                    return null;
                }
            }
            //var asm = this.LoadFromAssemblyName(assemblyName);//死循环代码
        }
        var result = _excludePluginReferencesFunc(assemblyName);
        if (!result)
        {
            string? assemblyPath = _dependencyResolver!.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadAssemblyFromFile(assemblyPath);
            }
        }
        return null;

    }


    /// <summary>
    /// 对程序集上下文非托管插件的函数重载，注：系统规定需要重载
    /// </summary>
    /// <param name="unmanagedDllName">路径</param>
    /// <returns></returns>
    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        //var result = _excludeAssembliesFunc == null ? false : _excludeAssembliesFunc(unmanagedDllName);
        //if (!result)
        //{
            string? libraryPath = _dependencyResolver!.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
       //}
        return IntPtr.Zero;

    }

}
