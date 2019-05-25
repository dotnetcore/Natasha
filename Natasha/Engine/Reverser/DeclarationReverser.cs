using System.Collections.Concurrent;
using System.Reflection;

namespace Natasha.Engine.Builder.Reverser
{
    public static class DeclarationReverser
    {
        //public readonly static ConcurrentDictionary<ParameterInfo, string> _type_mapping;
        //static DeclarationReverser()
        //{
        //    _type_mapping = new ConcurrentDictionary<ParameterInfo, string>();
        //}

        public static string GetDeclarationType(ParameterInfo info)
        {
            string Prefix = string.Empty;
            string result = NameReverser.GetName(info.ParameterType);
            if (info.ParameterType.Name.EndsWith("&"))
            {
                if (info.IsIn)
                {
                    Prefix = "in ";
                }
                else if (info.IsOut)
                {
                    Prefix = "out ";
                }
                else
                {
                    Prefix = "ref ";
                }
                return $"{Prefix}{result}";
            }
            else
            {
                return $"{result}";
            }
        }
        public static string GetDeclaration(ParameterInfo info)
        {
            string Prefix = string.Empty;
            string result = NameReverser.GetName(info.ParameterType);
            if (info.ParameterType.Name.EndsWith("&"))
            {
                if (info.IsIn)
                {
                    Prefix = "in ";
                }
                else if (info.IsOut)
                {
                    Prefix = "out ";
                }
                else
                {
                    Prefix = "ref ";
                }
                return $"{Prefix}{result} {info.Name}";
            }
            else
            {
                return $"{result} {info.Name}";
            }
        }

        public static string GetDeclaration(FieldInfo info)
        {
            return NameReverser.GetName(info.FieldType) + " "+info.Name;
        }
    }
}
