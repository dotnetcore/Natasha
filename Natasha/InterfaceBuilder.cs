using Natasha.Standard;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class InterfaceBuilder : ScriptStandard<InterfaceBuilder>
    {
        private Type _interface_type;
        private HashSet<string> _interface_methods;
        private Dictionary<string, string> _interface_methods_mapping;
        public string Result;
        public InterfaceBuilder(Type interfaceType) : base()
        {
            _link = this;
            _interface_type = interfaceType;
            _interface_methods_mapping = new Dictionary<string, string>();
        }

        public string this[string key]
        {
            get
            {
                if (!_interface_methods.Contains(key))
                {
                    throw new Exception($"{key}函数不是{_interface_type.Name}的成员函数！");
                }
                if (!_interface_methods_mapping.ContainsKey(key))
                {
                    return null;
                }
                return _interface_methods_mapping[key];
            }
            set
            {
                _interface_methods_mapping[key] = value;
            }
        }

        /// <summary>
        /// 组装编译
        /// </summary>
        /// <returns></returns>
        public Type Compile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _interface_methods_mapping)
            {
                sb.Append(item.Value);
            }
            Result = MakerHeader().MakerContent(sb.ToString()).Script;

            return ClassBuilder.GetType(Result);
        }
    }
}
