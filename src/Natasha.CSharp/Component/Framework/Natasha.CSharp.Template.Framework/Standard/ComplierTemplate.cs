using System;

namespace Natasha.CSharp.Template
{

    public partial class CompilerTemplate<T> : ALinkTemplate<T> where T : CompilerTemplate<T>, new()
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

        /// <summary>
        /// 为该模板所在的 编译单元 设置一个程序集名称[默认随机生成]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T SetAssemblyName(string name)
        {

            AssemblyBuilder.AssemblyName = name;
            return Link;

        }


        #region  Default 默认域创建以及参数

        /// <summary>
        /// 使用默认域来初始化编译单元. 
        /// </summary>
        /// <inheritdoc cref="CreateDomain" path="//*[not(self::summary)]"/>
        public static T DefaultDomain(Action<AssemblyCSharpBuilder>? option = default)
        {

            T instance = new();
            instance.OptionAction = option;
            option?.Invoke(instance.AssemblyBuilder);
            return instance;

        }
        #endregion


    }
}
