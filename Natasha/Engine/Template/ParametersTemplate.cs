using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Natasha.Engine.Template
{
    public class ParametersTemplate<T>: ContentTemplate<T>
    {
        public readonly List<KeyValuePair<Type, string>> ParametersMappings;
        public readonly List<Type> ParametersTypes;
        public string Parameters;

        public ParametersTemplate()
        {
            ParametersTypes = new List<Type>();
            ParametersMappings = new List<KeyValuePair<Type, string>>();
        }

        public T Parameter(MethodInfo info)
        {
            UsingRecoder.Add(info);
            Parameters = DeclarationReverser.GetParameters(info).ToString();
            return Link;
        }
        public T Parameter(string parameters)
        {
            Parameters = parameters;
            return Link;
        }
        public T Parameter(IEnumerable<KeyValuePair<Type, string>> parameters)
        {
            UsingRecoder.Add(parameters.Select(item => item.Key));
            Parameters = DeclarationReverser.GetParameters(parameters).ToString(); ;
            return Link;
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
            UsingRecoder.Add(type);
            ParametersMappings.Add(new KeyValuePair<Type, string>(type, key));
            return Link;
        }

    }
}
