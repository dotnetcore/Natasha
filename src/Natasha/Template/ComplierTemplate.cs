using System;

namespace Natasha.Template
{
    public class CompilerTemplate<T> : ALinkTemplate<T> where T : CompilerTemplate<T>, new()
    {

        //使用默认编译器
        public readonly AssemblyCompiler Compiler;
        public Action<AssemblyCompiler> OptionAction;
        public CompilerTemplate()
        {
            Compiler = new AssemblyCompiler();
        }




        public T AutoUsing()
        {

            Compiler.CustomerUsingShut = false;
            return Link;

        }
        public T CurstomeUsing()
        {
            Compiler.CustomerUsingShut = true;
            return Link;
        }




        public T AssemblyName(string name)
        {

            if (name != default)
            {
                Compiler.AssemblyName = name;
            }
            return Link;

        }



        #region 指定编译器的域进行创建
        public static T Create(AssemblyCompiler Compiler, Action<AssemblyCompiler> option = default)
        {

            return Use(Compiler.Domain, option);

        }
        #endregion
        #region 指定字符串域创建以及参数
        public static T Create(string domainName, Action<AssemblyCompiler> option = default)
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
        public static T Use(AssemblyDomain domain, Action<AssemblyCompiler> option = default)
        {

            T instance = new T();
            instance.Compiler.Domain = domain;
            instance.OptionAction = option;
            option?.Invoke(instance.Compiler);
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static T Default(Action<AssemblyCompiler> option = default)
        {

            return Use(DomainManagment.Default, option);

        }


        #endregion
        #region 随机域创建以及参数
        public static T Random(Action<AssemblyCompiler> option = default)
        {

            return Use(DomainManagment.Random, option);

        }
        #endregion

    }
}
