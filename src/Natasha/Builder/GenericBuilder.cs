using System;

namespace Natasha
{
    /// <summary>
    /// 泛型类型构建器
    /// </summary>
    public class GenericBuilder
    {
        /// <summary>
        /// 生成对应的泛型类
        /// </summary>
        /// <param name="genericType">泛型类型</param>
        /// <param name="genericParametersTypes">泛型参数</param>
        /// <returns></returns>
        public static Type GetType(Type genericType, params Type[] genericParametersTypes)
        {
            return genericType.MakeGenericType(genericParametersTypes);
        }




        /// <summary>
        /// 生成对应的泛型类
        /// </summary>
        /// <typeparam name="G">泛型类型</typeparam>
        /// <param name="genericParametersTypes">泛型参数</param>
        /// <returns></returns>
        public static Type GetType<G>(params Type[] genericParametersTypes)
        {
            return typeof(G).MakeGenericType(genericParametersTypes);
        }
    }
}
