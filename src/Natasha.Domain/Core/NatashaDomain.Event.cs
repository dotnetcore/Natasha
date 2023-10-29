using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

/// <summary>
/// Natasha域实现
/// C# 的引用代码是通过 Using 来完成的,该域实现增加了 Using 记录
/// </summary>
public partial class NatashaDomain
{
   

    private Assembly? Default_Resolving(AssemblyLoadContext arg1, AssemblyName arg2)
    {
        return Load(arg2);
    }



    private IntPtr Default_ResolvingUnmanagedDll(Assembly arg1, string arg2)
    {
        return LoadUnmanagedDll(arg2);
    }

    /// <summary>
    /// 文件加载引用事件
    /// </summary>
    protected event Action<Assembly, string>? LoadAssemblyReferencsWithPath;
    /// <summary>
    /// 流加载引用事件
    /// </summary>

    protected event Action<Assembly, Stream>? LoadAssemblyReferenceWithStream;



}
