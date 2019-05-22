using Natasha.Engine.Builder.Reverser;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Natasha.Remote
{
    public class ParametersMaker
    {
        public RemoteParameters Parameters;
        private string _current_name;
        public ParametersMaker()
        {
            Parameters = new RemoteParameters();
        }

        public ParametersMaker SetClass(string @class)
        {
            Parameters.TypeName = @class;
            return this;
        }

        public ParametersMaker SetMethod(string method)
        {
            Parameters.MethodName = method;
            return this;
        }

        public ParametersMaker this[string key]
        {
            get
            {
                _current_name = key;
                return this;
            }
        }

        public ParametersMaker Params<T>(T instance)
        {
            Parameters[_current_name] = JsonConvert.SerializeObject(instance);
            return this;
        }
    }


    public class ParametersMaker<T>
    {
        public RemoteParameters Parameters;
        private MethodInfo _info;
        private Type _type;
        public ParametersMaker()
        {
            
            Parameters = new RemoteParameters();
            _type = typeof(T);
            Parameters.TypeName = TypeReverser.Get(_type);
        }

        public ParametersMaker<T> this[string key]
        {
            get
            {
                Parameters.MethodName = key;
                _info = _type.GetMethod(key);
                return this;
            }
        }

        public RemoteParameters Params<P1>(P1 param1)
        {
            var parameters = _info.GetParameters();
            if (parameters.Length > 0)
            {
                Parameters[parameters[0].Name] = GetString(param1);
            }
            return Parameters;
        }
        public RemoteParameters Params<P1, P2>(P1 param1, P2 param2)
        {
            var parameters = _info.GetParameters();
            if (parameters.Length > 0)
            {
                Parameters[parameters[0].Name] = GetString(param1);
                Parameters[parameters[1].Name] = GetString(param2);
            }
            return Parameters;
        }
        public RemoteParameters Params<P1, P2, P3>(P1 param1, P2 param2, P3 param3)
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
        public RemoteParameters Params<P1, P2, P3, P4>(P1 param1, P2 param2, P3 param3, P4 param4)
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
        public RemoteParameters Params<P1, P2, P3, P4, P5>(P1 param1, P2 param2, P3 param3, P4 param4, P5 param5)
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
        public RemoteParameters Params<P1, P2, P3, P4, P5, P6>(P1 param1, P2 param2, P3 param3, P4 param4, P5 param5, P6 param6)
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
