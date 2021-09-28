using Natasha.CSharp;
using Natasha.CSharp.Reverser;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.Text;


public static class TypeExtension
{

    public static DomainBase GetDomain(this Type type)
    {

        return type.Assembly.GetDomain();

    }


    public static void RemoveReferences(this Type type)
    {

        type.Assembly.RemoveReferences();

    }



    public static void DisposeDomain(this Type type)
    {

        type.Assembly.DisposeDomain();

    }
    /// <summary>
    /// 判断是否为值类型,字符串类型,委托类型,Type类型,及委托的子类型其中之一
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsSimpleType(this Type type)
    {
        return type.IsValueType ||
            type == typeof(string) ||
            type.IsSubclassOf(typeof(Delegate)) ||
            type == typeof(Delegate) ||
            type.IsSubclassOf(typeof(MulticastDelegate)) ||
            type == typeof(MulticastDelegate) ||
            type == typeof(Type);
    }


    /// <summary>
    /// 当前类是否实现了某接口
    /// </summary>
    /// <param name="type">要判断的类型</param>
    /// <param name="iType">接口类型</param>
    /// <returns></returns>
    public static bool IsImplementFrom(this Type type, Type iType)
    {
        return new HashSet<Type>(type.GetInterfaces()).Contains(iType);
    }


    /// <summary>
    /// 当前类是否实现了某接口
    /// </summary>
    /// <param name="T">要判断的类型</param>
    /// <param name="iType">接口类型</param>
    /// <returns></returns>
    public static bool IsImplementFrom<T>(this Type type)
    {
        return new HashSet<Type>(type.GetInterfaces()).Contains(typeof(T));
    }


    /// <summary>
    /// 获取与该类型相关的所有类型,例如 List<int> => List<> / int32
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static HashSet<Type> GetAllTypes(this Type type)
    {
        HashSet<Type> result = new HashSet<Type> { type };


        var temp = type;
        while (temp.HasElementType)
        {
            temp = temp.GetElementType();
        }


        result.Add(temp);
        if (temp.IsGenericType && temp.FullName != null)
        {
            foreach (var item in temp.GetGenericArguments())
            {
                result.UnionWith(item.GetAllTypes());
            }
        }
        return result;

    }


    /// <summary>
    /// 获取所有该类所含的类 List<int> 则返回 List<> , Int32
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static HashSet<Type> GetAllTypes<T>()
    {
        return typeof(T).GetAllTypes();
    }



    /// <summary>
    /// 将类名替换成 文件名可使用的名字
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetAvailableName(this Type type)
    {
        return AvailableNameReverser.GetName(type);
    }


    /// <summary>
    /// 获取运行时完整类名
    /// 例如: System.Collections.Generic<T>
    /// 例如: System.Collections.Generic<System.Int32>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetDevelopName(this Type type)
    {
        return TypeNameReverser.ReverseFullName(type);
    }


    /// <summary>
    /// 获取运行时类名, 例如: List<int>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetRuntimeName(this Type type)
    {
        return TypeNameReverser.ReverseTypeName(type);
    }


    /// <summary>
    /// 获取运行时类名,无标识
    /// System.Collections.Generic.List<>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetDevelopNameWithoutFlag(this Type type)
    {
        return TypeNameReverser.ReverseFullName(type, true);
    }


    // <summary>
    /// 制作范型类型
    /// </summary>
    /// <param name="type">IType<T,S,D></param>
    /// <param name="types">T,S,D</param>
    /// <returns></returns>
    public static Type With(this Type type, params Type[] types)
    {
        return type.MakeGenericType(types);
    }

}

