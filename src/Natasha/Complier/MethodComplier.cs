using System;
using System.Reflection;

namespace Natasha.Complier
{
    /// <summary>
    /// 默认编译器
    /// </summary>
    public class MethodComplier :IComplier
    {

        /// <summary>
        /// 编译脚本，生成委托
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public Delegate Complie(string className,string content,string methodName,Type delegateType)
        {
            //获取程序集
            Assembly assembly = GetAssemblyByScript(content);


            //判空
            if (assembly == null)
            {
                return null;
            }


            //获取方法委托
            return AssemblyOperator
                .Loader(assembly)[className]
                .GetMethod(methodName)
                .CreateDelegate(delegateType);
        }


        /// <summary>
        /// 编译脚本，生成委托
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public Delegate Complie<T>(string className, string content, string methodName)
        {
            //获取程序集
            Assembly assembly = GetAssemblyByScript(content);


            //判空
            if (assembly == null)
            {
                return null;
            }


            //获取方法委托
            return AssemblyOperator
                .Loader(assembly)[className]
                .GetMethod(methodName)
                .CreateDelegate(typeof(T));
        }
    }
}
