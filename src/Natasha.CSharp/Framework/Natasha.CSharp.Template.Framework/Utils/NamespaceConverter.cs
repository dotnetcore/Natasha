using System;
using System.Reflection;

namespace Natasha.CSharp.Template
{

    /// <summary>
    /// 为快速委托提供显式转换类，可以对：
    /// Assembly/Assembly[]/Type/Type[]/string/string[]
    /// 等类型进行显式的赋值，将自动解析 using
    /// 例如 NDelegate.xxx.Action(code,"System",typeof(int),type.Assembly.....)
    /// </summary>
    public class NamespaceConverter
    {

        public Assembly? NamespaceAssembly;
        public Type? NamespaceType;
        public string? NamespaceString;
        public Type[]? NamespaceTypes;
        public string[]? NamespaceStrings;
        public Assembly[]? NamespaceAssemblys;



        public static implicit operator NamespaceConverter(Assembly assembly)
        {
            return new NamespaceConverter { NamespaceAssembly = assembly };
        }
        public static implicit operator NamespaceConverter(Type type)
        {
            return new NamespaceConverter { NamespaceType = type };
        }
        public static implicit operator NamespaceConverter(string @namespace)
        {
            return new NamespaceConverter { NamespaceString = @namespace };
        }
        public static implicit operator NamespaceConverter(string[] @namespace)
        {
            return new NamespaceConverter { NamespaceStrings = @namespace };
        }
        public static implicit operator NamespaceConverter(Assembly[] @namespace)
        {
            return new NamespaceConverter { NamespaceAssemblys = @namespace };
        }
        public static implicit operator NamespaceConverter(Type[] @namespace)
        {
            return new NamespaceConverter { NamespaceTypes = @namespace };
        }

    }

}
