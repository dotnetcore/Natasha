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


        //使用默认编译器
        public readonly AssemblyComplier Complier;

        public OnceMethodBuilder()
        {

            Complier = new AssemblyComplier();
        }



        #region 指定字符串域创建以及参数
        public static T Create(string domainName, ComplierResultTarget target = ComplierResultTarget.Stream, ComplierResultError error = ComplierResultError.None)
        {

            return Create(domainName, error, target);

        }

        public static T Create(string domainName, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            if (domainName == default || domainName.ToLower() == "default")
            {
                return Create(DomainManagment.Default, target, error);
            }
            else
            {
                return Create(DomainManagment.Create(domainName), target, error);
            }

        }
        #endregion
        #region 指定域创建以及参数
        public static T Create(AssemblyDomain domain, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(domain, target, error);

        }

        public static T Create(AssemblyDomain domain, ComplierResultTarget target = ComplierResultTarget.Stream, ComplierResultError error = ComplierResultError.None)
        {

            T instance = new T();
            instance.Complier.EnumCRError = error;
            instance.Complier.EnumCRTarget = target;
            instance.Complier.Domain = domain;
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static T Default()
        {

            return Create(DomainManagment.Default, ComplierResultTarget.Stream);

        }

        public static T Default(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Default, target, error);

        }

        public static T Default(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Default, target, error);

        }
        #endregion
        #region 随机域创建以及参数
        public static T Random()
        {

            return Create(DomainManagment.Random, ComplierResultTarget.Stream);

        }


        public static T Random(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Random, target, error);

        }


        public static T Random(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Random, target, error);

        }
        #endregion




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
