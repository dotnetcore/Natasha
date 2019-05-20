using System;
using System.Collections.Concurrent;

namespace Natasha.Remote
{
    public class RemoteReader
    {

        internal static ConcurrentDictionary<string, ConcurrentDictionary<string, Func<RemoteParameters, string>>> _func_mapping;
        static RemoteReader()
        {
            _func_mapping = new ConcurrentDictionary<string, ConcurrentDictionary<string, Func<RemoteParameters, string>>>();
        }
        /// <summary>
        /// 获取动态调用委托
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Func<RemoteParameters, string> GetFunc(RemoteParameters parameters)
        {
            return _func_mapping[parameters.TypeName][parameters.MethodName];
        }
        /// <summary>
        /// 动态调用
        /// </summary>
        /// <param name="remote">调用信息</param>
        /// <returns>返回序列化的结果</returns>
        public static string Invoke(string remote)
        {
            RemoteParameters parameters = null;
            return _func_mapping[parameters.TypeName][parameters.MethodName](parameters);
        }
        /// <summary>
        /// 动态调用
        /// </summary>
        /// <param name="parameters">调用参数</param>
        /// <returns>返回序列化的结果</returns>
        public static string Invoke(RemoteParameters parameters)
        {
            return _func_mapping[parameters.TypeName][parameters.MethodName](parameters);
        }
    }
}
