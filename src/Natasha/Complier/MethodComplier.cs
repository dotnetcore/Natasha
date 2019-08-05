using System;

namespace Natasha.Complier
{
    /// <summary>
    /// 默认编译器
    /// </summary>
    public class MethodComplier : IComplier
    {

        public MethodComplier()
        {
            UseMemoryComplie();
        }



        /// <summary>
        /// 编译脚本，生成委托
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public virtual Delegate Complie(string className, string content, string methodName, Type delegateType, object binder = null)
        {

            return GetDelegateByScript(content, className, methodName, delegateType, binder);

        }




        /// <summary>
        /// 编译脚本，生成委托
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="delegateType">委托类型</param>
        /// <returns></returns>
        public virtual T Complie<T>(string className, string content, string methodName, object binder = null) where T : Delegate
        {

            return GetDelegateByScript<T>(content, className, methodName, binder);

        }

    }

}
