using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;


public partial class NatashaDomain
{
    /// <summary>
    /// 从插件加载来的程序集
    /// </summary>
    private readonly ConcurrentDictionary<string, Assembly> _pluginAssemblies;
    private Func<AssemblyName, bool>? _excludeAssembliesFunc;




    #region 插件
    /// <summary>
    /// 获取当前域的程序集
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Assembly> GetPluginAssemblies()
    {
        return _pluginAssemblies.Values;
    }



    /// <summary>
    /// 加载插件
    /// </summary>
    /// <param name="path">插件路径</param>
    /// <param name="excludeAssembliesFunc">不需要加载的依赖项</param>
    /// <returns></returns>
    public Assembly LoadPlugin(string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {

        _excludeAssembliesFunc = excludeAssembliesFunc;
         _dependencyResolver = new AssemblyDependencyResolver(path);
        var assembly = LoadAssemblyFromFile(path);
        if (assembly != default)
        {
            var name = assembly.GetName().Name;
            if (!string.IsNullOrEmpty(name))
            {
                _pluginAssemblies[name] = assembly;
            }
            else
            {
                _pluginAssemblies[path] = assembly;
            }
            
        }
        return assembly!;

    }
    #endregion



}

