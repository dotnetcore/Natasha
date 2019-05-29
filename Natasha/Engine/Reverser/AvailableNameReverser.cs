using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class AvailableNameReverser
    {
        public readonly static ConcurrentDictionary<Type, string> _type_mapping;
        static AvailableNameReverser()
        {
            _type_mapping = new ConcurrentDictionary<Type, string>();
        }

        public static string GetName(Type type)
        {
            if (type == null)
            {
                return "";
            }
            if (!_type_mapping.ContainsKey(type))
            {
                _type_mapping[type] = GetAvailableName(type);
            }
            return _type_mapping[type];
        }

        public static string GetAvailableName(Type type)
        {
            return NameReverser.GetName(type).Replace('<', '_').Replace('>', '_').Replace(',', '_').Replace("[]", "__");
        }
    }
}
