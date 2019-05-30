using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.Complier
{
    /// <summary>
    /// 默认编译器
    /// </summary>
    public class DefaultComplier :IComplier
    {

        //构建的脚本
        public string Script;




        /// <summary>
        /// 构建脚本
        /// </summary>
        /// <returns></returns>
        public override string Builder()
        {
            return Script;
        }




        /// <summary>
        /// 编译脚本，生成委托
        /// </summary>
        /// <param name="info">方法委托元祖</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public Delegate Complie((string Flag, IEnumerable<Type> Types, string Script, Type Delegate) info,string className=null)
        {
            Script = info.Script;
            return Complie(className, info.Flag, info.Delegate);
        }




        /// <summary>
        /// 编译脚本，生成委托
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public Delegate Complie(string className,string methodName,Type delegateType)
        {
            //获取程序集
            Assembly assembly = GetAssemblyByScript(className);


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
    }
}
