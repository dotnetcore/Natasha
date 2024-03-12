using System;
using System.Reflection;
using Natasha.CSharp.Component.Load;

public class ReferenceConfiguration
{
    internal AssemblyCompareInformation _compileReferenceBehavior = AssemblyCompareInformation.None;
    internal Func<AssemblyName, AssemblyName, AssemblyLoadVersionResult>? _referenceSameNamePickFunc=null;

    /// <summary>
    /// 手动设置引用比较时，适用的程序集比较行为
    /// </summary>
    /// <param name="AssemblyCompareInformation"></param>
    /// <returns></returns>
    public ReferenceConfiguration SetCompareInfomation(AssemblyCompareInformation AssemblyCompareInformation)
    {
        _compileReferenceBehavior = AssemblyCompareInformation;
        return this;
    }
    /// <summary>
    /// 配置主域及当前域的引用加载行为，当主域和当前域存在相同引用时，选择高版本引用加载
    /// </summary>
    /// <returns></returns>
    public ReferenceConfiguration UseHighVersionReferences()
    {
        _compileReferenceBehavior = AssemblyCompareInformation.UseHighVersion;
        return this;
    }
    /// <summary>
    /// 配置主域及当前域的引用加载行为，当主域和当前域存在相同引用时，选择低版本引用加载
    /// </summary>
    /// <returns></returns>
    public ReferenceConfiguration UseLowVersionReferences()
    {
        _compileReferenceBehavior = AssemblyCompareInformation.UseLowVersion;
        return this;
    }
    /// <summary>
    /// 配置主域及当前域的引用加载行为，当主域和当前域存在相同引用时，选择全部加载
    /// </summary>
    /// <returns></returns>
    public ReferenceConfiguration UseAllReferences()
    {
        _compileReferenceBehavior = AssemblyCompareInformation.None;
        return this;
    }
    /// <summary>
    /// 配置主域及当前域的引用加载行为，当主域和当前域存在相同引用时，选择默认域引用加载
    /// </summary>
    /// <returns></returns>
    public ReferenceConfiguration UseDefaultReferences()
    {
        _compileReferenceBehavior = AssemblyCompareInformation.UseDefault;
        return this;
    }
    /// <summary>
    /// 配置主域及当前域的引用加载行为，当主域和当前域存在相同引用时，选择使用当前域引用加载
    /// </summary>
    /// <returns></returns>
    public ReferenceConfiguration UseCustomReferences()
    {
        _compileReferenceBehavior = AssemblyCompareInformation.UseCustom;
        return this;
    }

    /// <summary>
    /// 配置引用同名过滤策略
    /// </summary>
    /// <param name="useAssemblyNameFunc"></param>
    /// <returns></returns>
    public ReferenceConfiguration ConfigSameNameReferencesFilter(Func<AssemblyName, AssemblyName, AssemblyLoadVersionResult>? useAssemblyNameFunc = null)
    {
        _referenceSameNamePickFunc = useAssemblyNameFunc;
        return this;
    }
}