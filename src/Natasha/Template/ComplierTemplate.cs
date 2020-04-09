using Natasha.CSharpEngine;
using Natasha.Framework;
using System;

namespace Natasha.Template
{
    public class CompilerTemplate<T> : ALinkTemplate<T> where T : CompilerTemplate<T>, new()
    {

        //使用默认编译器
        public readonly AssemblyBuilder AssemblyBuilder;
        public Action<AssemblyBuilder> OptionAction;
        public CompilerTemplate()
        {
            AssemblyBuilder = new AssemblyBuilder();
        }




        public T AutoUsing()
        {

            AssemblyBuilder.CustomerUsingShut = false;
            return Link;

        }
        public T CurstomeUsing()
        {
            AssemblyBuilder.CustomerUsingShut = true;
            return Link;
        }




        public T AssemblyName(string name)
        {

            if (name != default)
            {
                AssemblyBuilder.Compiler.AssemblyName = name;
            }
            return Link;

        }



        #region 指定编译器的域进行创建
        public static T Create(AssemblyBuilder builder, Action<AssemblyBuilder> option = default)
        {

            return Use(builder.Compiler.Domain, option);

        }
        #endregion
        #region 指定字符串域创建以及参数
        public static T Create(string domainName, Action<AssemblyBuilder> option = default)
        {

            if (domainName == default || domainName.ToLower() == "default")
            {
                return Use(DomainManagment.Default, option);
            }
            else
            {
                return Use(DomainManagment.Create(domainName), option);
            }

        }
        #endregion
        #region 指定域创建以及参数
        public static T Use(DomainBase domain, Action<AssemblyBuilder> option = default)
        {

            T instance = new T();
            instance.AssemblyBuilder.Compiler.Domain = domain;
            instance.OptionAction = option;
            option?.Invoke(instance.AssemblyBuilder);
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static T Default(Action<AssemblyBuilder> option = default)
        {

            return Use(DomainManagment.Default, option);

        }


        #endregion
        #region 随机域创建以及参数
        public static T Random(Action<AssemblyBuilder> option = default)
        {

            return Use(DomainManagment.Random, option);

        }
        #endregion

    }
}
