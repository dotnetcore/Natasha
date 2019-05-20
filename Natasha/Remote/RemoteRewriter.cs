using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.Remote
{
    public class RemoteWritter
    {
        public static string Serialization;
        public static string Deserialization;

        public static void ComplieToRemote<T>()
        {
            ComplieToRemote(typeof(T));
        }
        public static void ComplieToRemote(Type type)
        {
            string className = TypeReverser.Get(type);

            if (!RemoteReader._func_mapping.ContainsKey(className))
            {
                RemoteReader._func_mapping[className] = new ConcurrentDictionary<string, Func<RemoteParameters, string>>();

                MethodInfo[] infos = type.GetMethods();

                foreach (var item in infos)
                {

                    var parameters = item.GetParameters();
                    StringBuilder sb = new StringBuilder();
                    StringBuilder call = new StringBuilder($"{item.Name}(");
                    if (parameters != null)
                    {
                        if (parameters.Length > 0)
                        {
                            call.Append(parameters[0].Name);
                            sb.Append($"{TypeReverser.Get(parameters[0].ParameterType)} {parameters[0].Name} = {Serialization}<{TypeReverser.Get(parameters[0].ParameterType)}>(parameters[\"{parameters[0].Name}\"]);");
                            for (int i = 1; i < parameters.Length; i++)
                            {
                                call.Append($",{parameters[i].Name}");
                                sb.Append($"{TypeReverser.Get(parameters[i].ParameterType)} {parameters[i].Name} = {Serialization}<{TypeReverser.Get(parameters[i].ParameterType)}>(parameters[\"{parameters[i].Name}\"]);");
                            }
                        }
                    }
                    RemoteReader._func_mapping[className][item.Name] = MethodBuilder
                        .NewMethod
                        .Using(type)
                        .Param<RemoteParameters>("parameters")
                        .Body(
                        $@"{sb}{className} instance = new {className}();return {Deserialization}<{TypeReverser.Get(item.ReturnType)}> (instance.{call}));"
                        ).Return<string>().Create<Func<RemoteParameters, string>>();

                }
            }
        }

    }
}
