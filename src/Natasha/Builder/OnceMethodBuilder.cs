using Natasha.Template;
using System;

namespace Natasha.Builder
{
    /// <summary>
    /// 一次性方法构建器
    /// </summary>
    /// <typeparam name="TBuilder"></typeparam>
    public class OnceMethodBuilder<TBuilder> : OnceMethodDelegateTemplate<TBuilder> where TBuilder : OnceMethodBuilder<TBuilder>,new()
    {


        //使用默认编译器
        public readonly AssemblyComplier Complier;

        public OnceMethodBuilder()
        {

            Complier = new AssemblyComplier();
        }



        public static TBuilder Default
        {

            get { return Create(); }

        }


        /// <summary>
        /// 如果参数为空，则使用默认域
        /// 如果参数不为空，则创建以参数为名字的独立域
        /// </summary>
        /// <param name="domainName">域名</param>
        /// <returns></returns>
        public static TBuilder Create(string domainName = default, bool complieInFile = default)
        {

            TBuilder instance = new TBuilder();
            instance.Complier.ComplieInFile = complieInFile;
            if (domainName == default)
            {
                instance.Complier.Domain = DomainManagment.Default;
            }
            else
            {
                instance.Complier.Domain = DomainManagment.Create(domainName);
            }

            return instance;

        }
        /// <summary>
        /// 使用一个现成的域
        /// </summary>
        /// <param name="domain">域</param>
        /// <returns></returns>
        public static TBuilder Create(AssemblyDomain domain, bool complieInFile = default)
        {

            TBuilder instance = new TBuilder();
            instance.Complier.ComplieInFile = complieInFile;
            instance.Complier.Domain = domain;
            return instance;

        }
        /// <summary>
        /// 创建一个随机的域
        /// </summary>
        /// <returns></returns>
        public static TBuilder Random(bool complieInFile = default)
        {

            TBuilder instance = new TBuilder() { };
            instance.Complier.ComplieInFile = complieInFile;
            instance.Complier.Domain = DomainManagment.Random;
            return instance;

        }





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
        public TBuilder UseFileComplie()
        {
            Complier.ComplieInFile = true;
            return Link;
        }




        /// <summary>
        /// 返回强类型委托
        /// </summary>
        /// <typeparam name="T">委托的强类型</typeparam>
        /// <returns></returns>
        public virtual T Complie<T>(object binder = null) where T : Delegate
        {

            Complier.Add(this);
            return Complier.GetDelegate<T>(
                OopNameScript,
                MethodNameScript,
                binder);

        }

    }

}
