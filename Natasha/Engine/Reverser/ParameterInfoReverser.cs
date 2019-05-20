using System.Collections.Concurrent;
using System.Reflection;

namespace Natasha.Engine.Builder.Reverser
{
    public static class ParameterInfoReverser
    {
        public static ConcurrentDictionary<ParameterInfo, string> _type_mapping;
        static ParameterInfoReverser()
        {
            _type_mapping = new ConcurrentDictionary<ParameterInfo, string>();
        }

        public static string Get(ParameterInfo info)
        {
            if (!_type_mapping.ContainsKey(info))
            {
                _type_mapping[info] = Reverser(info);
            }
            return _type_mapping[info];
        }
        public static string Reverser(ParameterInfo info)
        {

            string Prefix = string.Empty;
            string result = TypeReverser.Get(info.ParameterType);
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
                return Prefix + result;
            }
            else
            {
                return result;
            }

        }
    }
}
