#if !NETCOREAPP3_0_OR_GREATER
using System;

namespace Natasha.CSharp.Template
{

    public partial class CompilerTemplate<T> : ALinkTemplate<T> where T : CompilerTemplate<T>, new()
    {

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
#endif