using Natasha.Framework;
using System;

namespace Natasha.CSharp.Template
{
    public class CompilerTemplate<T> : ALinkTemplate<T> where T : CompilerTemplate<T>, new()
    {

        //使用默认编译器
        public readonly AssemblyCSharpBuilder AssemblyBuilder;
        public Action<AssemblyCSharpBuilder> OptionAction;
        public CompilerTemplate()
        {
            AssemblyBuilder = new AssemblyCSharpBuilder();
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
        public static T Create(AssemblyCSharpBuilder builder, Action<AssemblyCSharpBuilder> option = default)
        {

            return Use(builder.Compiler.Domain, option);

        }
        #endregion
        #region 指定字符串域创建以及参数
        public static T Create(string domainName, Action<AssemblyCSharpBuilder> option = default)
        {

            if (domainName == default || domainName.ToLower() == "default")
            {
                return Use(DomainManagement.Default, option);
            }
            else
            {
                return Use(DomainManagement.Create(domainName), option);
            }

        }
        #endregion
        #region 指定域创建以及参数
        public static T Use(DomainBase domain, Action<AssemblyCSharpBuilder> option = default)
        {

            T instance = new T();
            instance.AssemblyBuilder.Compiler.Domain = domain;
            instance.OptionAction = option;
            option?.Invoke(instance.AssemblyBuilder);
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static T Default(Action<AssemblyCSharpBuilder> option = default)
        {

            return Use(DomainManagement.Default, option);

        }


        #endregion
        #region 随机域创建以及参数
        public static T Random(Action<AssemblyCSharpBuilder> option = default)
        {

            return Use(DomainManagement.Random, option);

        }
        #endregion

    }
}
