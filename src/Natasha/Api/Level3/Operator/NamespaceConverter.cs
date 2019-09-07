using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{

    public class NamespaceConverter
    {

        public Assembly NamespaceAssembly;
        public Type NamespaceType;
        public string NamespaceString;

        public static implicit operator NamespaceConverter(Assembly assembly)
        {
            return new NamespaceConverter { NamespaceAssembly = assembly };
        }

        public static implicit operator NamespaceConverter(Type type)
        {
            return new NamespaceConverter{ NamespaceType = type};
        }
        public static implicit operator NamespaceConverter(string @namespace)
        {
            return new NamespaceConverter { NamespaceString = @namespace };
        }

    }

}
