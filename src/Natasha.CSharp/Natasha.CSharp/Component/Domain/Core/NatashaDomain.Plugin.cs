using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

[assembly: InternalsVisibleTo("NatashaFunctionUT, PublicKey=002400000480000094000000060200000024000052534131000400000100010069acb31dd0d9918441d6ed2b49cd67ae17d15fd6ded4ccd2f99b4a88df8cddacbf72d5897bb54f406b037688d99f482ff1c3088638b95364ef614f01c3f3f2a2a75889aa53286865463fb1803876056c8b98ec57f0b3cf2b1185de63d37041ba08f81ddba0dccf81efcdbdc912032e8d2b0efa21accc96206c386b574b9d9cb8")]
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
    internal Assembly LoadPlugin(string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {
        CheckAndIncrmentAssemblies();
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

