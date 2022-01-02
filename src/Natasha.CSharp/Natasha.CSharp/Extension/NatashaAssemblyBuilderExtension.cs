using Natasha.CSharp.Error.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


public static class NatashaAssemblyBuilderExtension
{
    public static Type GetTypeFromShortName(this AssemblyCSharpBuilder builder, string typeName)
    {

        Assembly assembly = builder.GetAssembly();
        try
        {
            return assembly.GetTypes().First(item => item.Name == typeName);
        }
        catch (Exception ex)
        {
            NatashaException exception = new($"无法在程序集 {builder.AssemblyName} 中找到该类型 {typeName}！错误信息:{ex.Message}");
            exception.ErrorKind = ExceptionKind.Type;
            throw exception;
        }

    }
    public static MethodInfo GetMethodFromShortName(this AssemblyCSharpBuilder builder, string typeName, string methodName)
    {

        var type = GetTypeFromShortName(builder,typeName);
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

            NatashaException exception = new($"无法在程序集 {builder.AssemblyName} 中找到类型 {typeName} 对应的 {methodName} 方法！错误信息:{ex.Message}");
            exception.ErrorKind = ExceptionKind.Method;
            throw exception;

        }

    }
    public static Delegate GetDelegateFromShortName(this AssemblyCSharpBuilder builder, string typeName, string methodName, Type delegateType, object? target = null)
    {

        var info = GetMethodFromShortName(builder, typeName, methodName);

        try
        {

            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            NatashaException exception = new($"无法将程序集 {builder.AssemblyName} 类型为 {typeName} 的 {methodName} 方法转成委托 {delegateType.Name}！错误信息:{ex.Message}");
            exception.ErrorKind = ExceptionKind.Delegate;
            throw exception;

        }

    }
    public static T GetDelegateFromShortName<T>(this AssemblyCSharpBuilder builder, string typeName, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromShortName(builder, typeName, methodName, typeof(T), target);
    }
}

