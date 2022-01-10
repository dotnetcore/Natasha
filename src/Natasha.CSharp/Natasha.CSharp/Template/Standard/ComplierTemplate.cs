using System;

namespace Natasha.CSharp.Template
{
    public class CompilerTemplate<T> : ALinkTemplate<T> where T : CompilerTemplate<T>, new()
    {
        //使用默认编译器
        public AssemblyCSharpBuilder AssemblyBuilder;
        public Action<AssemblyCSharpBuilder>? OptionAction;

        public CompilerTemplate()
        {

            AssemblyBuilder = new();

        }


        public T ConfigBuilder(Func<AssemblyCSharpBuilder, AssemblyCSharpBuilder> builderFunc)
        {

            builderFunc(AssemblyBuilder);
            return Link;

        }



        public T AssemblyName(string name)
        {

            AssemblyBuilder.AssemblyName = name;
            return Link;

        }



        #region 指定编译器的域进行创建
        public static T UseCompiler(AssemblyCSharpBuilder builder, Action<AssemblyCSharpBuilder>? option = default)
        {

            return UseDomain(builder.Domain, option);

        }
        #endregion
        #region 指定字符串域创建以及参数
        public static T CreateDomain(string domainName, Action<AssemblyCSharpBuilder>? option = default)
        {

            if (domainName.ToLower() == "default")
            {
                return UseDomain(NatashaReferenceDomain.DefaultDomain, option);
            }
            else
            {
                return UseDomain(DomainManagement.Create(domainName), option);
            }

        }
        #endregion
        #region 指定域创建以及参数
        public static T UseDomain(NatashaReferenceDomain domain, Action<AssemblyCSharpBuilder>? option = default)
        {

            T instance = new();
            instance.AssemblyBuilder.Domain = domain;
            instance.OptionAction = option;
            option?.Invoke(instance.AssemblyBuilder);
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static T DefaultDomain(Action<AssemblyCSharpBuilder>? option = default)
        {

            return UseDomain(NatashaReferenceDomain.DefaultDomain, option);

        }


        #endregion
        #region 随机域创建以及参数
        public static T RandomDomain(Action<AssemblyCSharpBuilder>? option = default)
        {

            return UseDomain(DomainManagement.Random(), option);

        }
        #endregion

    }
}
