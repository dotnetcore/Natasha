using System;
using System.Linq;
using System.Reflection;


public static class NatashaAssemblyExtension
{

    /// <summary>
    /// 为统一 Exception 报错, 为 Assembly 封装扩展方法, 反射出类型.
    /// </summary>
    /// <param name="assembly">要反射的程序集</param>
    /// <param name="typeName">反射的短类名</param>
    /// <returns></returns>
    public static Type GetTypeFromShortName(this Assembly assembly, string typeName)
    {
        try
        {
            return assembly.GetTypes().First(item => item.Name == typeName);
        }
        catch (Exception ex)
        {
            throw new NatashaException($"无法在程序集 {assembly.FullName} 中找到该类型 {typeName}！错误信息:{ex.Message}")
            {
                ErrorKind = NatashaExceptionKind.Type
            };
        }

    }
    /// <summary>
    /// 为统一 Exception 报错, 为 Assembly 封装扩展方法, 反射出类中的方法.
    /// </summary>
    /// <param name="assembly">要反射的程序集</param>
    /// <param name="typeName">反射的短类名</param>
    /// <param name="methodName">类中的方法名</param>
    /// <returns></returns>
    public static MethodInfo GetMethodFromShortName(this Assembly assembly, string typeName, string methodName)
    {

        var type = GetTypeFromShortName(assembly, typeName);
        try
        {
            var info = type.GetMethod(methodName);
            if (info == null)
            {
                throw new Exception("获取方法返回空!");
            }
            return info!;
        }
        catch (Exception ex)
        {

            throw new NatashaException($"无法在程序集 {assembly.FullName} 中找到类型 {typeName} 对应的 {methodName} 方法！错误信息:{ex.Message}")
            {
                ErrorKind = NatashaExceptionKind.Method
            };
        }

    }
    /// <summary>
    ///  为统一 Exception 报错, 为 Assembly 封装扩展方法, 反射出类中的方法委托.
    /// </summary>
    /// <param name="assembly">要反射的程序集</param>
    /// <param name="typeName">反射的短类名</param>
    /// <param name="methodName">类中的方法名</param>
    /// <param name="delegateType">委托类型</param>
    /// <param name="target">绑定信息</param>
    /// <returns></returns>
    public static Delegate GetDelegateFromShortName(this Assembly assembly, string typeName, string methodName, Type delegateType, object? target = null)
    {

        var info = GetMethodFromShortName(assembly, typeName, methodName);

        try
        {

            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            throw new NatashaException($"无法将程序集 {assembly.FullName} 类型为 {typeName} 的 {methodName} 方法转成委托 {delegateType.Name}！错误信息:{ex.Message}")
            {
                ErrorKind = NatashaExceptionKind.Delegate
            };

        }

    }
    public static T GetDelegateFromShortName<T>(this Assembly assembly, string typeName, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromShortName(assembly, typeName, methodName, typeof(T), target);
    }

    /// <summary>
    /// 为统一 Exception 报错, 为 Assembly 封装扩展方法, 反射出类型.
    /// </summary>
    /// <param name="assembly">要反射的程序集</param>
    /// <param name="typeName">反射的完整类名</param>
    /// <returns></returns>
    public static Type GetTypeFromFullName(this Assembly assembly, string typeName)
    {

        try
        {
            return assembly.GetTypes().First(item => item.GetDevelopName() == typeName);
        }
        catch (Exception ex)
        {
            throw new NatashaException($"无法在程序集 {assembly.FullName} 中找到该类型 {typeName}！错误信息:{ex.Message}")
            {
                ErrorKind = NatashaExceptionKind.Type
            };
        }

    }

    /// <summary>
    /// 为统一 Exception 报错, 为 Assembly 封装扩展方法, 反射出类中的方法.
    /// </summary>
    /// <param name="assembly">要反射的程序集</param>
    /// <param name="typeName">反射的完整类名</param>
    /// <param name="methodName">类中的方法名</param>
    /// <returns></returns>
    public static MethodInfo GetMethodFromFullName(this Assembly assembly, string typeName, string methodName)
    {

        var type = GetTypeFromFullName(assembly, typeName);
        try
        {
            var info = type.GetMethod(methodName);
            if (info == null)
            {
                throw new Exception("获取方法返回空!");
            }
            return info!;
        }
        catch (Exception ex)
        {

            throw new NatashaException($"无法在程序集 {assembly.FullName} 中找到类型 {typeName} 对应的 {methodName} 方法！错误信息:{ex.Message}")
            {
                ErrorKind = NatashaExceptionKind.Method
            };

        }

    }

    /// <summary>
    ///  为统一 Exception 报错, 为 Assembly 封装扩展方法, 反射出类中的方法委托.
    /// </summary>
    /// <param name="assembly">要反射的程序集</param>
    /// <param name="typeName">反射的完整类名</param>
    /// <param name="methodName">类中的方法名</param>
    /// <param name="delegateType">委托类型</param>
    /// <param name="target">绑定信息</param>
    /// <returns></returns>
    public static Delegate GetDelegateFromFullName(this Assembly assembly, string typeName, string methodName, Type delegateType, object? target = null)
    {

        var info = GetMethodFromFullName(assembly, typeName, methodName);

        try
        {

            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            throw new NatashaException($"无法将程序集 {assembly.FullName} 类型为 {typeName} 的 {methodName} 方法转成委托 {delegateType.Name}！错误信息:{ex.Message}")
            {
                ErrorKind = NatashaExceptionKind.Delegate
            };

        }

    }
    public static T GetDelegateFromFullName<T>(this Assembly assembly, string typeName, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromFullName(assembly, typeName, methodName, typeof(T), target);
    }

}

