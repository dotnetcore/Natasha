using System;
using System.Collections.Generic;

namespace Natasha
{
    /// <summary>
    /// 委托构建器，动态构建Func和Action委托
    /// </summary>
    public class DelegateBuilder
    {


        public readonly static Type[] FuncMaker;
        public readonly static Type[] ActionMaker;


        static DelegateBuilder()
        {
            FuncMaker = new Type[9];
            FuncMaker[0] = typeof(Func<>);
            FuncMaker[1] = typeof(Func<,>);
            FuncMaker[2] = typeof(Func<,,>);
            FuncMaker[3] = typeof(Func<,,,>);
            FuncMaker[4] = typeof(Func<,,,,>);
            FuncMaker[5] = typeof(Func<,,,,,>);
            FuncMaker[6] = typeof(Func<,,,,,,>);
            FuncMaker[7] = typeof(Func<,,,,,,,>);
            FuncMaker[8] = typeof(Func<,,,,,,,,>);

            ActionMaker = new Type[9];
            ActionMaker[0] = typeof(Action);
            ActionMaker[1] = typeof(Action<>);
            ActionMaker[2] = typeof(Action<,>);
            ActionMaker[3] = typeof(Action<,,>);
            ActionMaker[4] = typeof(Action<,,,>);
            ActionMaker[5] = typeof(Action<,,,,>);
            ActionMaker[6] = typeof(Action<,,,,,>);
            ActionMaker[7] = typeof(Action<,,,,,,>);
            ActionMaker[8] = typeof(Action<,,,,,,,>);
        }




        /// <summary>
        /// 获取函数委托
        /// </summary>
        /// <param name="parametersTypes">泛型参数</param>
        /// <param name="returnType">返回类型</param>
        /// <returns>函数委托</returns>
        public static Type GetDelegate(Type[] parametersTypes = null, Type returnType = null)
        {
            if (returnType == null || returnType == typeof(void))
            {
                return GetAction(parametersTypes);
            }
            else
            {
                return GetFunc(returnType, parametersTypes);
            }
        }




        /// <summary>
        /// 根据类型动态生成Func委托
        /// </summary>
        /// <param name="returnType">返回类型</param>
        /// <param name="parametersTypes">泛型类型</param>
        /// <returns>Func委托类型</returns>
        public static Type GetFunc(Type returnType, Type[] parametersTypes = null)
        {
            if (parametersTypes == null)
            {
                return FuncMaker[0].MakeGenericType(returnType);
            }


            List<Type> list = new List<Type>();
            list.AddRange(parametersTypes);
            list.Add(returnType);


            return FuncMaker[parametersTypes.Length].MakeGenericType(list.ToArray());

        }




        /// <summary>
        /// 根据类型动态生成Action委托
        /// </summary>
        /// <param name="parametersTypes">泛型参数类型</param>
        /// <returns>Action委托类型</returns>
        public static Type GetAction(Type[] parametersTypes = null)
        {
            if (parametersTypes == null || parametersTypes.Length == 0)
            {
                return ActionMaker[0];
            }

            
            return ActionMaker[parametersTypes.Length].MakeGenericType(parametersTypes);
        }
    }
}
