using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha.Template
{
    public class OnceMethodParametersTemplate<T>: OnceMethodNameTemplate<T>
    {
        public readonly List<KeyValuePair<Type, string>> ParametersMappings;
        private MethodInfo _methodInfo;
        public readonly List<Type> ParametersTypes;
        public string ParametersScript;

        public OnceMethodParametersTemplate()
        {
            ParametersTypes = new List<Type>();
            ParametersMappings = new List<KeyValuePair<Type, string>>();
        }




        public T Param(MethodInfo info)
        {
            _methodInfo = info;
            var parameters = info.GetParameters();
            for (int i = 0; i < parameters.Length; i+=1)
            {
                Param(parameters[i].ParameterType, parameters[i].Name);
            }
            return Link;

        }




        public T Param(string parameters)
        {
            ParametersScript = parameters;
            return Link;
        }




        internal void GetParams(IEnumerable<KeyValuePair<Type, string>> parameters)
        {
            if (_methodInfo==null)
            {
                ParametersScript = DeclarationReverser.GetParameters(parameters).ToString();
            }
            else
            {
                ParametersScript = DeclarationReverser.GetParameters(_methodInfo).ToString();
            }
            
        }




        /// <summary>
        /// 添加参数
        /// </summary>
        /// <typeparam name="S">参数类型</typeparam>
        /// <param name="key">参数名字</param>
        /// <returns></returns>
        public T Param<S>(string key)
        {
            return Param(typeof(S), key);
        }




        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="type">参数类型</param>
        /// <param name="key">参数名字</param>
        /// <returns></returns>
        public T Param(Type type, string key)
        {
            ParametersTypes.Add(type);
            if (type!=null && type.IsGenericType)
            {
                UsingRecoder.Add(type.GetAllGenericTypes());
            }
            UsingRecoder.Add(type);
            ParametersMappings.Add(new KeyValuePair<Type, string>(type, key));
            return Link;
        }

        public override T Builder()
        {
            if (ParametersScript == null)
            {
                GetParams(ParametersMappings);
            }
            StringBuilder temp = new StringBuilder();
            temp.Append($@"{ParametersScript}
{{
{OnceBuilder}
}}");
            OnceBuilder = temp;
            return base.Builder();
        }

    }
}
