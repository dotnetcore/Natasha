using System;
using System.Collections.Concurrent;

namespace Natasha.Remote
{

    /// <summary>
    /// 远程读取
    /// </summary>
    public class RemoteReader
    {

        public readonly static ConcurrentDictionary<string, ConcurrentDictionary<string, Func<TransportParameters, string>>> _func_mapping;
        static RemoteReader() => _func_mapping = new ConcurrentDictionary<string, ConcurrentDictionary<string, Func<TransportParameters, string>>>();




        /// <summary>
        /// 获取动态调用委托
        /// </summary>
        /// <param name="parameters">远程参数</param>
        /// <returns></returns>
        public static Func<TransportParameters, string> GetFunc(TransportParameters parameters)
        {
            return _func_mapping[parameters.TypeName][parameters.MethodName];
        }




        /// <summary>
        /// 动态调用
        /// </summary>
        /// <param name="remote">调用信息</param>
        /// <returns>返回序列化的结果</returns>
        public static string Execute(string remote)
        {
            TransportParameters parameters = null;
            return _func_mapping[parameters.TypeName][parameters.MethodName](parameters);
        }
        /// <summary>
        /// 动态调用
        /// </summary>
        /// <param name="parameters">调用参数</param>
        /// <returns>返回序列化的结果</returns>
        public static string Execute(TransportParameters parameters)
        {
            return _func_mapping[parameters.TypeName][parameters.MethodName](parameters);
        }
    }
}
