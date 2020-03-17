using Natasha.Template;
using System;

namespace Natasha.Builder
{
    /// <summary>
    /// 一次性方法构建器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OnceMethodBuilder<T> : OnceMethodDelegateTemplate<T> where T : OnceMethodBuilder<T>,new()
    {


        public OnceMethodBuilder(){}



        /// <summary>
        /// 编译返回委托
        /// </summary>
        /// <returns></returns>
        public virtual Delegate Complie(object binder = null)
        {

            Complier.Add(this);
            return Complier.GetDelegate(
                OopNameScript,
                MethodNameScript,
                DelegateType,
                binder);

        }




        /// <summary>
        /// 将结果编译进文件
        /// </summary>
        /// <returns></returns>
        public T UseFileComplie()
        {
            Complier.EnumCRTarget = ComplierResultTarget.File ;
            return Link;
        }




        /// <summary>
        /// 返回强类型委托
        /// </summary>
        /// <typeparam name="S">委托的强类型</typeparam>
        /// <returns></returns>
        public virtual S Complie<S>(object binder = null) where S : Delegate
        {

            Complier.Add(this);
            return Complier.GetDelegate<S>(
                OopNameScript,
                MethodNameScript,
                binder);

        }

    }

}
