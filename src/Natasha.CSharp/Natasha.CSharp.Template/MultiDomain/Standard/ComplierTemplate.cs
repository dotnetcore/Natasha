#if NETCOREAPP3_0_OR_GREATER
using System;

namespace Natasha.CSharp.Template
{

    public partial class CompilerTemplate<T> : ALinkTemplate<T> where T : CompilerTemplate<T>, new()
    {

        #region 指定编译器的域进行创建

        /// <summary>
        /// 用传入的编译单元域来初始化编译单元.
        /// </summary>
        /// <inheritdoc cref="CreateDomain" path="//*[not(self::summary)]"/>
        public static T UseCompiler(AssemblyCSharpBuilder builder, Action<AssemblyCSharpBuilder>? option = default)
        {

            return UseDomain(builder.Domain, option);

        }
        #endregion
        #region 指定字符串域创建以及参数

        /// <summary>
        /// 创建一个域来初始化编译单元. 此方法创建的实例 instance. <br/>
        /// </summary>
        /// <remarks>
        /// <example>
        /// <code>
        /// 
        ///     //使用 NoGlobalUsing 来禁用全局 using 覆盖.(默认开启)
        ///     instance.NoGlobalUsing();
        ///     
        ///     //使用 NotLoadDomainUsing 来禁用域内 using 覆盖.(默认开启)
        ///     instance.NotLoadDomainUsing();
        ///     
        ///     //使用 ConfigBuilder 方法来配置编译单元.
        ///     instance.ConfigBuilder(bld=>bld);
        ///     
        ///     //使用 ConfigCompilerOption 方法来配置编译选项.
        ///     bld=>bld.ConfigCompilerOption(opt=>opt.xxx);
        ///     
        ///     //使用 ConfigSyntaxOptions 方法来配置语法选项
        ///     bld=>bld.ConfigSyntaxOptions(opt=>opt.xxx).
        /// 
        /// </code>
        /// </example>
        /// </remarks>
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


        /// <summary>
        /// 使用作用域中的 Domain 
        /// </summary>
        /// <inheritdoc cref="CreateDomain" path="//*[not(self::summary)]"/>
        public static T UseScope()
        {
            return new();
        }



        /// <summary>
        /// 使用一个域来初始化编译单元
        /// </summary>
        /// <inheritdoc cref="CreateDomain" path="//*[not(self::summary)]"/>
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


        /// <summary>
        /// 使用默认域来初始化编译单元. 
        /// </summary>
        /// <inheritdoc cref="CreateDomain" path="//*[not(self::summary)]"/>
        public static T DefaultDomain(Action<AssemblyCSharpBuilder>? option = default)
        {
            
            return UseDomain(NatashaReferenceDomain.DefaultDomain, option);

        }


        #endregion
        #region 随机域创建以及参数

        /// <summary>
        /// 使用随机域来初始化编译单元. 
        /// </summary>
        /// <inheritdoc cref="CreateDomain" path="//*[not(self::summary)]"/>
        public static T RandomDomain(Action<AssemblyCSharpBuilder>? option = default)
        {

            return UseDomain(DomainManagement.Random(), option);

        }
        #endregion

    }
}
#endif