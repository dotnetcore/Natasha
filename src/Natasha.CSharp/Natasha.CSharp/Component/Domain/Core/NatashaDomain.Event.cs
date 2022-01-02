using System;
using System.Reflection;
using System.Runtime.Loader;

/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>
public partial class NatashaDomain
{
   
    #region 默认域解析程序集依赖事件
    internal Assembly? Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
    {
        return Load(arg2);
    }



    internal IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2)
    {
        return LoadUnmanagedDll(arg2);
    }
    #endregion


}
