using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Template
{
    public class ComplierTemplate<T> : ALinkTemplate<T> where T: ComplierTemplate<T>,new()
    {

        //使用默认编译器
        public readonly AssemblyComplier Complier;
        public Action<AssemblyComplier> OptionAction;
        public ComplierTemplate()
        {
            Complier = new AssemblyComplier();
        }



        public T AssemblyName(string name)
        {

            if (name!=default)
            {
                Complier.AssemblyName = name;
            }
            return Link;
           
        }



        #region 指定编译器的域进行创建
        public static T Create(AssemblyComplier complier, Action<AssemblyComplier> option = default)
        {

            return Create(complier.Domain,option);

        }
        #endregion

        #region 指定字符串域创建以及参数
        public static T Create(string domainName, Action<AssemblyComplier> option = default)
        {

            if (domainName == default || domainName.ToLower() == "default")
            {
                return Create(DomainManagment.Default, option);
            }
            else
            {
                return Create(DomainManagment.Create(domainName), option);
            }

        }
        #endregion
        #region 指定域创建以及参数
        public static T Create(AssemblyDomain domain, Action<AssemblyComplier> option = default)
        {

            T instance = new T();
            instance.Complier.Domain = domain;
            instance.OptionAction = option;
            option?.Invoke(instance.Complier);
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static T Default(Action<AssemblyComplier> option = default)
        {

            return Create(DomainManagment.Default, option);

        }


        #endregion
        #region 随机域创建以及参数
        public static T Random(Action<AssemblyComplier> option = default)
        {

            return Create(DomainManagment.Random, option);

        }
        #endregion

    }
}
