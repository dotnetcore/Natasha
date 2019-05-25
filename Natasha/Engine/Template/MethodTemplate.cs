using Natasha.Engine.Builder.Reverser;
using Natasha.Engine.Reverser;
using Natasha.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.Engine.Template
{
    public class MethodTemplate : ParametersTemplate<MethodTemplate>, IMethodPackage
    {
        private string _method_name;
        public MethodTemplate()
        {
            Link = this;
            _method_name = "NatashaDynamicMethod";
        }
        public bool HashMethodName()
        {
            return _method_name != "NatashaDynamicMethod";
        }
        public MethodTemplate MethodName(string name)
        {
            _method_name = name;
            return this;
        }
        public MethodTemplate MethodName(MethodInfo info)
        {
            _method_name = info.Name;
            return this;
        }

        private string _modifier;
        public MethodTemplate Modifier(MethodInfo modifier)
        {
            _modifier = ModifierReverser.GetModifier(modifier);
            return this;
        }
        public MethodTemplate Modifier(Modifiers modifier)
        {
            _modifier = ModifierReverser.GetModifier(modifier);
            return this;
        }
        public MethodTemplate Modifier(string modifier)
        {
            _modifier = modifier;
            return this;
        }
       



        private string _return_string;
        private Type _return_type;
        /// <summary>
        /// 设置返回类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <returns></returns>

        public MethodTemplate Return<T>()
        {
            return Return(typeof(T));
        }
        public MethodTemplate Return(MethodInfo info)
        {
            return Return(info.ReturnType);
        }
        public MethodTemplate Return(Type type = null)
        {
            if (type==null)
            {
                type = typeof(void);
            }
            _return_type = type;
            UsingRecoder.Add(type);
            _return_string = NameReverser.GetName(type);
            return this;
        }



        private Type _delegate_type;
        /// <summary>
        /// 打包成脚本
        /// </summary>
        /// <returns>(函数名，涉及到的类，脚本体, 委托类型)</returns>
        public (string Flag, IEnumerable<Type> Types, string Script, Type Delegate) Package()
        {
            _delegate_type = DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), _return_type);
            StringBuilder builder = new StringBuilder();
            if (Parameters == null)
            {
                Parameter(ParametersMappings);
            }
            builder.Append($"{AccessLevel}{_modifier}{_return_string} {_method_name}{Parameters}{{{Content}}}");
            return (_method_name, UsingRecoder.Types, builder.ToString(), _delegate_type);
        }
    }
}
