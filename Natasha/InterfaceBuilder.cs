using Natasha.Standard;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class InterfaceBuilder<T> : InterfaceBuilder
    {
        internal static ConcurrentDictionary<string, Func<T>> _ctor_mapping;

        static InterfaceBuilder()
        {
            _ctor_mapping = new ConcurrentDictionary<string, Func<T>>();
        }

        public InterfaceBuilder() : base(typeof(T))
        {

        }

        public static T New(string @class) {
            return _ctor_mapping[@class]();
        }


        public override Delegate Compile()
        {
            var result = base.Compile();
            _ctor_mapping[_class_name] = (Func<T>)result;
            return result;
        }

        public T Create(string @class)
        {
            return _ctor_mapping[@class]();
        }
        
    }
    public class InterfaceBuilder : ScriptStandard<InterfaceBuilder>
    {
        internal static ConcurrentDictionary<string, Delegate> _delegate_mapping;
        private Type _interface_type;
        private HashSet<string> _interface_methods;
        private Dictionary<string, string> _interface_methods_mapping;
        private string _current_key;
        public string Result;

        static InterfaceBuilder()
        {
            _delegate_mapping = new ConcurrentDictionary<string, Delegate>();
        }
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
                _current_key = key;
                _interface_methods_mapping[key] = value;
            }
        }


        public string PackageMethod(string key, string content)
        {
            StringBuilder sb = new StringBuilder();
            MethodInfo method = _interface_type.GetMethod(key);
            var parameters = method.GetParameters();
            Using(method.ReturnType);
            sb.Append($"public {GetRealMethod(method.ReturnType.Name)} {method.Name}");
            sb.Append("(");
            if (parameters.Length>0)
            {
                Using(parameters[0].ParameterType);
                sb.Append($"{GetRealMethod(parameters[0].ParameterType.Name)} {parameters[0].Name}");
                for (int i = 1; i < parameters.Length; i++)
                {
                    Using(parameters[i].ParameterType);
                    sb.Append($",{GetRealMethod(parameters[i].ParameterType.Name)} {parameters[i].Name}");
                }
            }
            sb.Append($"){{{content}}}");
            return sb.ToString();
        }


        /// <summary>
        /// 组装编译
        /// </summary>
        /// <returns></returns>
        public virtual Delegate Compile()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _interface_methods_mapping)
            {
                sb.Append(PackageMethod(item.Key,item.Value));
            }

            Result = Using(_interface_type)
                .Namespace("NatashaInterface")
                .Inheritance(_interface_type)
                .MakerHeader()
                .MakerContent(sb.ToString()).Script;

            Type type = ClassBuilder.GetType(Result);
            var tempDelegate = MethodBuilder.NewMethod
                .Using(_interface_type)
                .Using(type)
                .Body($"return new {_class_name}();")
                .Return(_interface_type)
                .Create();

            _delegate_mapping[_class_name] = tempDelegate;
            return tempDelegate;
        }

        private string GetRealMethod(string type)
        {
            if (type=="Void")
            {
                return "void";
            }
            else if (type.EndsWith("&"))
            {
                return $"ref {type.Substring(0,type.Length-1)}";
            }
            return type;
        }
    }
}
