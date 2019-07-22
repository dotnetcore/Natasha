using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Natasha.Remote
{

    /// <summary>
    /// 远程参数生成器
    /// </summary>
    public class RequestParameters
    {
        private string _current_name;
        private readonly TransportParameters Parameters;

        public RequestParameters() => Parameters = new TransportParameters();




        /// <summary>
        /// 设置类名
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public RequestParameters ClassName(string className)
        {
            Parameters.TypeName = className;
            return this;
        }




        /// <summary>
        /// 设置方法名字
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <returns></returns>
        public RequestParameters MethodName(string methodName)
        {
            Parameters.MethodName = methodName;
            return this;
        }




        /// <summary>
        /// 设置当前key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RequestParameters this[string key]
        {
            get
            {
                _current_name = key;
                return this;
            }
        }




        /// <summary>
        /// 添加参数
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="instance">参数值</param>
        /// <returns></returns>
        public RequestParameters Params<T>(T instance)
        {
            Parameters[_current_name] = JsonConvert.SerializeObject(instance);
            return this;
        }
    }


    /// <summary>
    /// 远程参数生成器
    /// </summary>
    /// <typeparam name="T">运行时的类型</typeparam>
    public class RequestParameters<T>
    {
        public TransportParameters Parameters;
        private MethodInfo _info;
        private readonly Type _type;
        public RequestParameters()
        {
            _type = typeof(T);
            Parameters = new TransportParameters
            {
                TypeName = _type.GetDevelopName()
            };
        }





        /// <summary>
        /// 设置当前方法名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RequestParameters<T> this[string key]
        {
            get
            {
                Parameters.MethodName = key;
                _info = _type.GetMethod(key);
                return this;
            }
        }




        public TransportParameters Params<P1>(P1 param1)
        {
            var parameters = _info.GetParameters();
            if (parameters.Length > 0)
            {
                Parameters[parameters[0].Name] = GetString(param1);
            }
            return Parameters;
        }
        public TransportParameters Params<P1, P2>(P1 param1, P2 param2)
        {
            var parameters = _info.GetParameters();
            if (parameters.Length > 0)
            {
                Parameters[parameters[0].Name] = GetString(param1);
                Parameters[parameters[1].Name] = GetString(param2);
            }
            return Parameters;
        }
        public TransportParameters Params<P1, P2, P3>(P1 param1, P2 param2, P3 param3)
        {
            var parameters = _info.GetParameters();
            if (parameters.Length > 0)
            {
                Parameters[parameters[0].Name] = GetString(param1);
                Parameters[parameters[1].Name] = GetString(param2);
                Parameters[parameters[2].Name] = GetString(param3);
            }
            return Parameters;
        }
        public TransportParameters Params<P1, P2, P3, P4>(P1 param1, P2 param2, P3 param3, P4 param4)
        {
            var parameters = _info.GetParameters();
            if (parameters.Length > 0)
            {
                Parameters[parameters[0].Name] = GetString(param1);
                Parameters[parameters[1].Name] = GetString(param2);
                Parameters[parameters[2].Name] = GetString(param3);
                Parameters[parameters[3].Name] = GetString(param4);
            }
            return Parameters;
        }
        public TransportParameters Params<P1, P2, P3, P4, P5>(P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
        {
            var parameters = _info.GetParameters();
            if (parameters.Length > 0)
            {
                Parameters[parameters[0].Name] = GetString(param1);
                Parameters[parameters[1].Name] = GetString(param2);
                Parameters[parameters[2].Name] = GetString(param3);
                Parameters[parameters[3].Name] = GetString(param4);
                Parameters[parameters[4].Name] = GetString(param5);
            }
            return Parameters;
        }
        public TransportParameters Params<P1, P2, P3, P4, P5, P6>(P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6)
        {
            var parameters = _info.GetParameters();
            if (parameters.Length > 0)
            {
                Parameters[parameters[0].Name] = GetString(param1);
                Parameters[parameters[1].Name] = GetString(param2);
                Parameters[parameters[2].Name] = GetString(param3);
                Parameters[parameters[3].Name] = GetString(param4);
                Parameters[parameters[4].Name] = GetString(param5);
                Parameters[parameters[5].Name] = GetString(param6);
            }
            return Parameters;
        }




        /// <summary>
        /// 序列化处理封装
        /// </summary>
        /// <typeparam name="S">参数类型</typeparam>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public string GetString<S>(S value)
        {
            //if (typeof(S)==typeof(string))
            //{
             //   return value.ToString();
           // }
            return JsonConvert.SerializeObject(value); 
        }
    }

}
