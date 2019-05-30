using Natasha.Complier;
using System;

namespace Natasha.Builder
{
    /// <summary>
    /// 一次性方法构建器
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    public class OnceMethodBuilder<TBuilder> : OnceMethodTemplate<TBuilder>
    {

        //使用默认编译器
        public DefaultComplier ComplierInstance;

        public OnceMethodBuilder() => ComplierInstance = new DefaultComplier();




        /// <summary>
        /// 编译返回委托
        /// </summary>
        /// <returns></returns>
        public Delegate Complie()
        {
            return ComplierInstance.Complie(Package(), NameScript);
        }




        /// <summary>
        /// 返回强类型委托
        /// </summary>
        /// <typeparam name="T">委托的强类型</typeparam>
        /// <returns></returns>
        public T Complie<T>() where T:Delegate
        {
            return (T)Complie();
        }
    }
}
