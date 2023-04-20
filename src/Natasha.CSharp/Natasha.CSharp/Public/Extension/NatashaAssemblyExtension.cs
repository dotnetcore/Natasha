using System;
using System.Linq;
using System.Reflection;


public static class NatashaAssemblyExtension
{
    public static Type GetTypeFromShortName(this Assembly assembly, string typeName)
    {
        try
        {
            return assembly.GetTypes().First(item => item.Name == typeName);
        }
        catch (Exception ex)
        {
            NatashaException exception = new($"无法在程序集 {assembly.FullName} 中找到该类型 {typeName}！错误信息:{ex.Message}");
            exception.ErrorKind = NatashaExceptionKind.Type;
            throw exception;
        }

    }
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

            NatashaException exception = new($"无法在程序集 {assembly.FullName} 中找到类型 {typeName} 对应的 {methodName} 方法！错误信息:{ex.Message}");
            exception.ErrorKind = NatashaExceptionKind.Method;
            throw exception;

        }

    }
    public static Delegate GetDelegateFromShortName(this Assembly assembly, string typeName, string methodName, Type delegateType, object? target = null)
    {

        var info = GetMethodFromShortName(assembly, typeName, methodName);

        try
        {

            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            NatashaException exception = new($"无法将程序集 {assembly.FullName} 类型为 {typeName} 的 {methodName} 方法转成委托 {delegateType.Name}！错误信息:{ex.Message}");
            exception.ErrorKind = NatashaExceptionKind.Delegate;
            throw exception;

        }

    }
    public static T GetDelegateFromShortName<T>(this Assembly assembly, string typeName, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromShortName(assembly, typeName, methodName, typeof(T), target);
    }


    public static Type GetTypeFromFullName(this Assembly assembly, string typeName)
    {

        try
        {
            return assembly.GetTypes().First(item => item.GetDevelopName() == typeName);
        }
        catch (Exception ex)
        {
            NatashaException exception = new($"无法在程序集 {assembly.FullName} 中找到该类型 {typeName}！错误信息:{ex.Message}");
            exception.ErrorKind = NatashaExceptionKind.Type;
            throw exception;
        }

    }


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

            NatashaException exception = new($"无法在程序集 {assembly.FullName} 中找到类型 {typeName} 对应的 {methodName} 方法！错误信息:{ex.Message}");
            exception.ErrorKind = NatashaExceptionKind.Method;
            throw exception;

        }

    }



    public static Delegate GetDelegateFromFullName(this Assembly assembly, string typeName, string methodName, Type delegateType, object? target = null)
    {

        var info = GetMethodFromFullName(assembly, typeName, methodName);

        try
        {

            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            NatashaException exception = new($"无法将程序集 {assembly.FullName} 类型为 {typeName} 的 {methodName} 方法转成委托 {delegateType.Name}！错误信息:{ex.Message}");
            exception.ErrorKind = NatashaExceptionKind.Delegate;
            throw exception;

        }

    }
    public static T GetDelegateFromFullName<T>(this Assembly assembly, string typeName, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromFullName(assembly, typeName, methodName, typeof(T), target);
    }

}

