using System;
using System.Reflection;

namespace Natasha.Engine.Reverser
{
    public static class AccessReverser
    {
        public static string GetAccess(AccessTypes access)
        {
            switch (access)
            {
                case AccessTypes.Public:
                    return "public ";
                case AccessTypes.Private:
                    return "private ";
                case AccessTypes.Protected:
                    return "protected ";
                case AccessTypes.Internal:
                    return "internal ";
                case AccessTypes.InternalAndProtected:
                    return "internal ";
                default:
                    return "internal ";
            }
        }
        public static string GetAccess(MethodInfo info)
        {
            if (info.IsPublic)
            {
                return "public ";
            }
            else if (info.IsPrivate)
            {
                return "private ";
            }
            else if (info.IsFamily)
            {
                return "protected ";
            }
            else if (info.IsAssembly)
            {
                return "internal ";
            }
            else if (info.IsFamilyOrAssembly)
            {
                return "protected internal ";
            }
            return "internal ";
        }
        public static string GetAccess(Type type)
        {
            if (type.IsPublic)
            {
                return "public ";
            }
            else if (type.IsNotPublic)
            {
                return "internal ";
            }
            else if (type.IsNestedFamily)
            {
                return "protected ";
            }
            else if (type.IsNestedAssembly)
            {
                return "internal protected";
            }
            return "internal ";
        }
    }
}
