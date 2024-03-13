using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

namespace Natasha.DynamicLoad.Base
{
    /// <summary>
    /// 编译时所需实现的域功能接口
    /// </summary>
    public interface INatashaDynamicLoadContextBase : IDisposable
    {
        //域名
        string? Name { get; }
        /// <summary>
        /// 获取 Natasha 动态加载域包装对象， NatashaLoadContext
        /// </summary>
        /// <returns></returns>
        object? GetCallerReference();
        /// <summary>
        /// 设置 Natasha 动态加载包装对象， NatashaLoadContext
        /// </summary>
        /// <param name="instance">包装对象</param>
        void SetCallerReference(object instance);
        /// <summary>
        /// 插件加载方法
        /// </summary>
        /// <param name="path"></param>
        /// <param name="excludeAssembliesFunc"></param>
        /// <returns></returns>
        Assembly LoadPlugin(string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null);
        /// <summary>
        /// 设置程序集比较行为
        /// </summary>
        /// <param name="loadBehavior"></param>
        void SetAssemblyLoadBehavior(AssemblyCompareInformation loadBehavior);
        /// <summary>
        /// 当前域的所有程序集
        /// </summary>
        IEnumerable<Assembly> Assemblies { get; }
        /// <summary>
        /// 通过文件加载到域
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns></returns>
        Assembly LoadAssemblyFromFile(string path);
        /// <summary>
        /// 通过流加载到域
        /// </summary>
        /// <param name="dllStream">dll文件流</param>
        /// <param name="pdbStream">pdb文件流</param>
        /// <returns></returns>
        Assembly LoadAssemblyFromStream(Stream dllStream,  Stream? pdbStream);
    }
}