using System;
using System.Collections.Generic;
namespace Natasha
{
    /// <summary>
    /// 泛型参数类型获取
    /// </summary>
    public class GenericTypeOperator
    {


        /// <summary>
        /// 获取泛型类型的所有参数类型
        /// </summary>
        /// <param name="type">泛型类型</param>
        /// <returns></returns>
        public static List<Type> GetTypes(Type type)
        {
            List<Type> result = new List<Type>();
            result.Add(type);
            if (type.IsGenericType && type.FullName != null)
            {
                foreach (var item in type.GetGenericArguments())
                {
                    result.AddRange(GetTypes(item));
                }
            }
            return result;
        }




        public static List<Type> GetTypes<T>()
        {
            return GetTypes(typeof(T));
        }
    }
}
