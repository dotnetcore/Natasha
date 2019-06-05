using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;

namespace Natasha.Remote
{
    /// <summary>
    /// 动态方法写入
    /// </summary>
    public class RemoteWritter
    {
        public static string Serialization;
        public static string Deserialization;

        static RemoteWritter()
        {
            Serialization = "JsonConvert.SerializeObject";
            Deserialization = "JsonConvert.DeserializeObject";
        }

        /// <summary>
        /// 将类型中的方法写入缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void ComplieToRemote<T>()
        {
            ComplieToRemote(typeof(T));
        }
        public static void ComplieToRemote(Type type)
        {
            string className = NameReverser.GetName(type);

            if (!RemoteReader._func_mapping.ContainsKey(className))
            {
                RemoteReader._func_mapping[className] = new ConcurrentDictionary<string, Func<RemoteParameters, string>>();
                MethodInfo[] infos = type.GetMethods();

                foreach (var item in infos)
                {
                    if (item.ReturnType == typeof(void))
                    {
                        continue;
                    }
                    var parameters = item.GetParameters();
                    StringBuilder sb = new StringBuilder();
                    StringBuilder call = new StringBuilder($"{item.Name}(");
                    if (parameters != null)
                    {
                        if (parameters.Length > 0)
                        {
                            call.Append(parameters[0].Name);
                            sb.Append($"{NameReverser.GetName(parameters[0].ParameterType)} {parameters[0].Name} = {Deserialization}<{NameReverser.GetName(parameters[0].ParameterType)}>(parameters[\"{parameters[0].Name}\"]);");

                            for (int i = 1; i < parameters.Length; i++)
                            {
                                call.Append($",{parameters[i].Name}");
                                sb.Append($"{NameReverser.GetName(parameters[i].ParameterType)} {parameters[i].Name} = {Deserialization}<{NameReverser.GetName(parameters[i].ParameterType)}>(parameters[\"{parameters[i].Name}\"]);");
                            }
                        }
                    }

                    RemoteReader._func_mapping[className][item.Name] = FastMethodOperator.New
                            .Using(type)
                            .Using(typeof(JsonConvert))
                            .Param<RemoteParameters>("parameters")
                            .MethodBody(
                            $@"{sb}{className} instance = new {className}();return {Serialization}(instance.{call}));")
                            .Return<string>()
                        .Complie<Func<RemoteParameters, string>>();

                }
            }
        }

    }
}
