using Natasha.CSharp.Component.Load;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Xml.Linq;

/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>
public partial class NatashaDomain
{

    public AssemblyCompareInfomation AssemblyLoadBehavior;

    /// <summary>
    /// 设置加载行为
    /// </summary>
    /// <param name="loadBehavior">加载行为枚举</param>
    public void SetAssemblyLoadBehavior(AssemblyCompareInfomation loadBehavior)
    {
        AssemblyLoadBehavior = loadBehavior;
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
        var pdbPath = Path.ChangeExtension(path, ".pdb");
        if (Name == "Default")
        {
            if (File.Exists(pdbPath))
            {
                using var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var pdbFile = File.Open(pdbPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                assembly = Default.LoadFromStream(file, pdbFile);
            }
            else
            {
                assembly = Default.LoadFromAssemblyPath(path);
            }
        }
        else
        {
            if (File.Exists(pdbPath))
            {
                using var file = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var pdbFile = File.Open(pdbPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                assembly = LoadFromStream(file, pdbFile);
            }
            else
            {
                assembly = LoadFromAssemblyPath(path);
            }
        }
        return assembly;

    }


    /// <summary>
    /// 将流转换为程序集，并加载到域
    /// [手动释放]
    /// </summary>
    /// <param name="dllStream">库文件流</param>
    /// <param name="pdbStream">符号流</param>
    /// <returns></returns>
    public virtual Assembly LoadAssemblyFromStream(Stream dllStream, Stream? pdbStream)
    {
        using (dllStream)
        {

            Assembly assembly;
            if (Name == "Default")
            {
                assembly = Default.LoadFromStream(dllStream, pdbStream);
            }
            else
            {
                assembly = LoadFromStream(dllStream, pdbStream);
            }
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
        var result = _excludePluginReferencesFunc(assemblyName);
        if (AssemblyLoadBehavior != AssemblyCompareInfomation.None && Name != "Default")
        {
            var name = assemblyName.GetUniqueName();
#if DEBUG
            Debug.WriteLine($"\t[当前域解析]程序集唯一名称:{assemblyName.Name}！");
#endif

            //当前域检测
            var preAssembly = this.Assemblies.FirstOrDefault(asm => asm.GetName().GetUniqueName() == name);
            if (preAssembly != null)
            {
#if DEBUG
                Debug.WriteLine($"\t\t使用之前的依赖:{preAssembly.FullName}！");
#endif
                return preAssembly;
            }

            //默认域覆盖检测
            if (_defaultAssemblyNameCache.TryGetValue(name!, out var defaultAsmName))
            {
#if DEBUG
                Debug.WriteLine($"\t\t[匹配成功]源/共享域版本:{assemblyName.Version}/{defaultAsmName.Version}！");
#endif
                if (result || assemblyName.CompareWithDefault(defaultAsmName, AssemblyLoadBehavior) == AssemblyLoadVersionResult.UseDefault)
                {
                    var defaultAsm = Default.Assemblies.FirstOrDefault(asm => asm.GetName().GetUniqueName() == name);
                    if (defaultAsm != null)
                    {
#if DEBUG
                        if (result)
                        {
                            Debug.WriteLine($"\t由于被排除，使用共享域版本:{defaultAsmName.Version}！");
                        }
                        else
                        {
                            Debug.WriteLine($"\t使用共享域版本:{defaultAsmName.Version}！");
                        }
                        
#endif
                        return defaultAsm;
                    }
                }
            }
            //var asm = this.LoadFromAssemblyName(assemblyName);//死循环代码
        }
        if (!result)
        {
            string? assemblyPath = _dependencyResolver!.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null && File.Exists(assemblyPath))
            {
#if DEBUG
                Debug.WriteLine($"\t[文件依赖解析]依赖路径:{assemblyPath}！");
#endif
                return LoadAssemblyFromFile(assemblyPath);
            }
        }
#if DEBUG
        Debug.WriteLine($"\t使用当前域版本:{assemblyName.FullName}！");
#endif
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
        if (libraryPath != null && File.Exists(libraryPath))
        {
            return LoadUnmanagedDllFromPath(libraryPath);
        }
        //}
        return IntPtr.Zero;

    }

}
