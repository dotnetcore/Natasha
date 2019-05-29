using System;
using System.Reflection;

namespace Natasha
{
    public static class ModifierReverser
    {
        public static string GetModifier(Modifiers modifier)
        {
            switch (modifier)
            {
                case Modifiers.Static:
                    return "static ";
                case Modifiers.Virtual:
                    return "virtual ";
                case Modifiers.New:
                    return "new ";
                case Modifiers.Abstract:
                    return "abstract ";
                case Modifiers.Override:
                    return "override ";
                default:
                    return "";
            }
        }
        public static string GetModifier(MethodInfo info)
        {
            if (info.IsStatic)
            {
                return "static ";
            }
            if (info.IsVirtual)
            {
                return "virtual ";
            }
            return "";
        }
        public static string GetModifier(Type info)
        {
            if (info.IsAbstract)
            {
                return "abstract ";
            }
            if (info.IsSealed)
            {
                return "sealed ";
            }
            return "";
        }
    }
}
