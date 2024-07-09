using Natasha.CSharp.Compiler.Utils;
using Natasha.DynamicLoad.Base;
using System;
using System.Diagnostics;
using System.Reflection;

public static partial class NatashaManagement
{

    public static NatashaInitializeHelper GetInitializer()
    {
        return new NatashaInitializeHelper();
    }

    /// <summary>
    ///  是 <see cref="Preheating{TCreator}(Func{AssemblyName?, string?, bool}?, bool?, bool?, bool)"/> 的封装方法.
    /// </summary>
    /// <remarks>
    /// 该方法传递的参数为 null ,false, false, false. 
    /// </remarks>
    public static void Preheating<TCreator>() where TCreator : INatashaDynamicLoadContextCreator, new()
    {
        Preheating<TCreator>(null, false, false, false);
    }

    /// <summary>
    ///  是 <see cref="Preheating{TCreator}(Func{AssemblyName?, string?, bool}?, bool?, bool?, bool)"/> 的封装方法.
    /// </summary>
    /// <remarks>
    /// 该方法传递的参数为 null ,[该方法参数1], false, false. 
    /// </remarks>
    public static void Preheating<TCreator>(bool? useRuntimeUsing) where TCreator : INatashaDynamicLoadContextCreator, new()
    {
        Preheating<TCreator>(null, useRuntimeUsing, false, false);
    }

    /// <summary>
    ///  是 <see cref="Preheating{TCreator}(Func{AssemblyName?, string?, bool}?, bool?, bool?, bool)"/> 的封装方法.
    /// </summary>
    /// <remarks>
    /// 该方法传递的参数为 null ,[该方法参数1], [该方法参数2], false. 
    /// </remarks>
    public static void Preheating<TCreator>(bool? useRuntimeUsing, bool? useRuntimeReference) where TCreator : INatashaDynamicLoadContextCreator, new()
    {
        Preheating<TCreator>(null, useRuntimeUsing, useRuntimeReference, false);
    }

    /// <summary>
    ///  是 <see cref="Preheating{TCreator}(Func{AssemblyName?, string?, bool}?, bool?, bool?, bool)"/> 的封装方法.
    /// </summary>
    /// <remarks>
    /// 该方法传递的参数为 null ,[该方法参数1], [该方法参数2], [该方法参数3]. 
    /// </remarks>
    public static void Preheating<TCreator>(bool? useRuntimeUsing, bool? useRuntimeReference, bool useFileCache = false) where TCreator : INatashaDynamicLoadContextCreator, new()
    {
        Preheating<TCreator>(null, useRuntimeUsing, useRuntimeReference, useFileCache);
    }

    /// <summary>
    /// 预热方法
    /// </summary>
    /// <typeparam name="TCreator">实现 INatashaDynamicLoadContextCreator 的类</typeparam>
    /// <param name="excludeReferencesFunc"></param>
    /// <param name="useRuntimeUsing">是否使用实现程序集的 using</param>
    /// <param name="useRuntimeReference">是否使用实现程序集的元数据</param>
    /// <param name="useFileCache">是否使用 using 缓存</param>
    public static void Preheating<TCreator>(
        Func<AssemblyName?, string?, bool>? excludeReferencesFunc,
        bool? useRuntimeUsing,
        bool? useRuntimeReference,
        bool useFileCache = false) where TCreator : INatashaDynamicLoadContextCreator, new ()
    {
        RegistDomainCreator<TCreator>();
        NatashaInitializer.Preheating(excludeReferencesFunc, useRuntimeUsing, useRuntimeReference, useFileCache);
    }

    /// <summary>
    ///  是 <see cref="Preheating(Func{AssemblyName?, string?, bool}?, bool?, bool?, bool)"/> 的封装方法.
    /// </summary>
    /// <remarks>
    /// 该方法传递的参数为 null ,false, false, false. 
    /// </remarks>
    public static void Preheating()
    {
        Preheating(null, false, false, false);
    }

    /// <summary>
    ///  是 <see cref="Preheating(Func{AssemblyName?, string?, bool}?, bool?, bool?, bool)"/> 的封装方法.
    /// </summary>
    /// <remarks>
    /// 该方法传递的参数为 null ,[该方法参数1], false, false. 
    /// </remarks>
    public static void Preheating(bool? useRuntimeUsing) { 
        Preheating(null, useRuntimeUsing, false, false);
    }

    /// <summary>
    ///  是 <see cref="Preheating(Func{AssemblyName?, string?, bool}?, bool?, bool?, bool)"/> 的封装方法.
    /// </summary>
    /// <remarks>
    /// 该方法传递的参数为 null ,[该方法参数1], [该方法参数2], false. 
    /// </remarks>
    public static void Preheating(bool? useRuntimeUsing, bool? useRuntimeReference)
{ 
        Preheating(null, useRuntimeUsing, useRuntimeReference, false);
    }

    /// <summary>
    ///  是 <see cref="Preheating(Func{AssemblyName?, string?, bool}?, bool?, bool?, bool)"/> 的封装方法.
    /// </summary>
    /// <remarks>
    /// 该方法传递的参数为 null ,[该方法参数1], [该方法参数2],  [该方法参数3]. 
    /// </remarks>
    public static void Preheating(bool? useRuntimeUsing, bool? useRuntimeReference, bool useFileCache = false)
    {
        Preheating(null, useRuntimeUsing, useRuntimeReference, useFileCache);
    }

    /// <summary>
    /// 预热方法,调用此方法之前需要调用 RegistDomainCreator<TCreator> 确保域的创建
    /// </summary>
    /// <param name="excludeReferencesFunc"></param>
    /// <param name="useRuntimeUsing">是否使用实现程序集的 using</param>
    /// <param name="useRuntimeReference">是否使用实现程序集的元数据</param>
    /// <param name="useFileCache">是否使用 using 缓存</param>
    public static void Preheating(
        Func<AssemblyName?, string?, bool>? excludeReferencesFunc,
        bool? useRuntimeUsing = false, 
        bool? useRuntimeReference = false,
        bool useFileCache = false)
    {
        NatashaInitializer.Preheating(excludeReferencesFunc, useRuntimeUsing, useRuntimeReference, useFileCache);
    }


    /// <summary>
    /// 注册域的实现
    /// </summary>
    /// <typeparam name="TCreator">INatashaDynamicLoadContextCreator 的实现类</typeparam>
    public static void RegistDomainCreator<TCreator>() where TCreator : INatashaDynamicLoadContextCreator, new()
    {
#if DEBUG
        //StopwatchExtension.EnableMemoryMonitor();
        Stopwatch stopwatch = new();
        stopwatch.Start();
#endif
        NatashaLoadContext<TCreator>.Prepare();
#if DEBUG
        stopwatch.StopAndShowCategoreInfo("[  Regist Domain  ]", "注册域实现", 1);
#endif
    }


    /// <summary>
    /// 获取系统域
    /// </summary>
    /// <returns></returns>
    public static NatashaLoadContext GetDefaultDomain()
    {
        return NatashaLoadContext.DefaultContext;
    }
    /// <summary>
    /// 新建一个域
    /// </summary>
    /// <param name="domainName"></param>
    /// <returns></returns>
    public static NatashaLoadContext CreateDomain(string domainName)
    {
        return DomainManagement.Create(domainName);
    }
    /// <summary>
    /// 新建一个随机域
    /// </summary>
    /// <returns></returns>
    public static NatashaLoadContext CreateRandomDomain()
    {
        return DomainManagement.Random();
    }
}

