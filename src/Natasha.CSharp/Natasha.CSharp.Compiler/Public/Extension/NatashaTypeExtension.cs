using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Reflection;


public static class NatashaTypeExtension
{
    /// <summary>
    /// 获取类型所在程序集的元数据引用
    /// </summary>
    /// <param name="type"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    public unsafe static IEnumerable<MetadataReference> GetDependencyReferences(this Type type, Func<AssemblyName?, string?, bool>? filter = null)
    {
        return type.Assembly.GetDependencyReferences(filter);
    }

    public static Delegate GetDelegateFromType(this Type type, string methodName, Type delegateType, object? target = null)
    {
        var info = type.GetMethod(methodName);
        try
        {
            if (info == null)
            {
                throw new Exception($"未从{type.FullName}中反射出{methodName}方法!");
            }
            return info.CreateDelegate(delegateType, target);

        }
        catch (Exception ex)
        {

            NatashaException exception = new($"类型为 {type.FullName} 的 {methodName} 方法无法转成委托 {delegateType.Name}！错误信息:{ex.Message}")
            {
                ErrorKind = NatashaExceptionKind.Delegate
            };
            throw exception;

        }

    }
    public static T GetDelegateFromType<T>(this Type type, string methodName, object? target = null) where T : Delegate
    {
        return (T)GetDelegateFromType(type, methodName, typeof(T), target);
    }

}

