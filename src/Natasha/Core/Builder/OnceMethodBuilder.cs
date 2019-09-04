using Natasha.Template;
using System;

namespace Natasha.Builder
{
    /// <summary>
    /// 一次性方法构建器
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    public class OnceMethodBuilder<TBuilder> : OnceMethodDelegateTemplate<TBuilder>
    {


        //使用默认编译器
        public readonly AssemblyComplier Complier;
        public OnceMethodBuilder() => Complier = new AssemblyComplier();


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
        /// 返回强类型委托
        /// </summary>
        /// <typeparam name="T">委托的强类型</typeparam>
        /// <returns></returns>
        public virtual T Complie<T>(object binder=null) where T : Delegate
        {

            Complier.Add(this);
            return Complier.GetDelegate<T>(
                OopNameScript,
                MethodNameScript,
                binder);

        }

    }

}
