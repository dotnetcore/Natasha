using System;
using System.Reflection;
using System.Runtime.Loader;


public partial class NatashaDomain
{

    /// <summary>
    /// 加载插件
    /// </summary>
    /// <param name="path">插件路径</param>
    /// <param name="excludeAssembliesFunc">不需要加载的依赖项</param>
    /// <returns></returns>
    internal Assembly LoadPlugin(string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null)
    {

        if (excludeAssembliesFunc!=null)
        {
            _excludePluginReferencesFunc = excludeAssembliesFunc;
        }
        CheckAndIncrmentAssemblies();
        _dependencyResolver = new AssemblyDependencyResolver(path);
        var assembly = LoadAssemblyFromFile(path);
        return assembly!;

    }

}

