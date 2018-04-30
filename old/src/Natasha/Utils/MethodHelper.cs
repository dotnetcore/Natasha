using Natasha;
using Natasha.Cache;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Reflection.Emit
{
    public static class MethodHelper
    {
        
        public static Type[] GetGenericTypes()
        {
            return new Type[0];
        }
        public static Type[] GetGenericTypes<T1>()
        {
            Type[] methodParameterTypes = new Type[1];
            methodParameterTypes[0] = GetRuntimeType(typeof(T1));
            return methodParameterTypes;
        }
        public static Type[] GetGenericTypes<T1, T2>()
        {
            Type[] methodParameterTypes = new Type[2];
            methodParameterTypes[0] = GetRuntimeType(typeof(T1));
            methodParameterTypes[1] = GetRuntimeType(typeof(T2));
            return methodParameterTypes;
        }
        public static Type[] GetGenericTypes<T1, T2, T3>()
        {
            Type[] methodParameterTypes = new Type[3];
            methodParameterTypes[0] = GetRuntimeType(typeof(T1));
            methodParameterTypes[1] = GetRuntimeType(typeof(T2));
            methodParameterTypes[2] = GetRuntimeType(typeof(T3));
            return methodParameterTypes;
        }
        public static Type[] GetGenericTypes<T1, T2, T3, T4>()
        {
            Type[] methodParameterTypes = new Type[4];
            methodParameterTypes[0] = GetRuntimeType(typeof(T1));
            methodParameterTypes[1] = GetRuntimeType(typeof(T2));
            methodParameterTypes[2] = GetRuntimeType(typeof(T3));
            methodParameterTypes[3] = GetRuntimeType(typeof(T4));
            return methodParameterTypes;
        }
        public static Type[] GetGenericTypes<T1, T2, T3, T4, T5>()
        {
            Type[] methodParameterTypes = new Type[5];
            methodParameterTypes[0] = GetRuntimeType(typeof(T1));
            methodParameterTypes[1] = GetRuntimeType(typeof(T2));
            methodParameterTypes[2] = GetRuntimeType(typeof(T3));
            methodParameterTypes[3] = GetRuntimeType(typeof(T4));
            methodParameterTypes[4] = GetRuntimeType(typeof(T5));
            return methodParameterTypes;
        }
        public static Type[] GetGenericTypes<T1, T2, T3, T4, T5, T6>()
        {
            Type[] methodParameterTypes = new Type[6];
            methodParameterTypes[0] = GetRuntimeType(typeof(T1));
            methodParameterTypes[1] = GetRuntimeType(typeof(T2));
            methodParameterTypes[2] = GetRuntimeType(typeof(T3));
            methodParameterTypes[3] = GetRuntimeType(typeof(T4));
            methodParameterTypes[4] = GetRuntimeType(typeof(T5));
            methodParameterTypes[5] = GetRuntimeType(typeof(T6));
            return methodParameterTypes;
        }
        public static Type[] GetGenericTypes<T1, T2, T3, T4, T5, T6, T7>()
        {
            Type[] methodParameterTypes = new Type[7];
            methodParameterTypes[0] = GetRuntimeType(typeof(T1));
            methodParameterTypes[1] = GetRuntimeType(typeof(T2));
            methodParameterTypes[2] = GetRuntimeType(typeof(T3));
            methodParameterTypes[3] = GetRuntimeType(typeof(T4));
            methodParameterTypes[4] = GetRuntimeType(typeof(T5));
            methodParameterTypes[5] = GetRuntimeType(typeof(T6));
            methodParameterTypes[6] = GetRuntimeType(typeof(T7));
            return methodParameterTypes;
        }
        public static Type[] GetGenericTypes<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            Type[] methodParameterTypes = new Type[8];
            methodParameterTypes[0] = GetRuntimeType(typeof(T1));
            methodParameterTypes[1] = GetRuntimeType(typeof(T2));
            methodParameterTypes[2] = GetRuntimeType(typeof(T3));
            methodParameterTypes[3] = GetRuntimeType(typeof(T4));
            methodParameterTypes[4] = GetRuntimeType(typeof(T5));
            methodParameterTypes[5] = GetRuntimeType(typeof(T6));
            methodParameterTypes[6] = GetRuntimeType(typeof(T7));
            methodParameterTypes[7] = GetRuntimeType(typeof(T8));
            return methodParameterTypes;
        }
        public static Type[] GetGenericTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        {
            Type[] methodParameterTypes = new Type[9];
            methodParameterTypes[0] = GetRuntimeType(typeof(T1));
            methodParameterTypes[1] = GetRuntimeType(typeof(T2));
            methodParameterTypes[2] = GetRuntimeType(typeof(T3));
            methodParameterTypes[3] = GetRuntimeType(typeof(T4));
            methodParameterTypes[4] = GetRuntimeType(typeof(T5));
            methodParameterTypes[5] = GetRuntimeType(typeof(T6));
            methodParameterTypes[6] = GetRuntimeType(typeof(T7));
            methodParameterTypes[7] = GetRuntimeType(typeof(T8));
            methodParameterTypes[8] = GetRuntimeType(typeof(T9));
            return methodParameterTypes;
        }
        public static Type GetRuntimeType(Type type)
        {
            if (type.IsGenericType)
            {
                Type tempType = type.GetGenericTypeDefinition();
                if (tempType==typeof(ERef<>))
                {
                    tempType = type.GetGenericArguments()[0];
                    return tempType.MakeByRefType();
                }
            }
            return type;
        }
        public static void CallMethod(MethodInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (info.DeclaringType.IsValueType || info.IsStatic)
            {
                il.REmit(OpCodes.Call, info);
            }
            else
            {
                il.REmit(OpCodes.Callvirt, info);
            }
        }

        //返回
        public static void Return()
        {
            ILGenerator il = ThreadCache.GetIL();
            il.REmit(OpCodes.Ret);
        }

        //压栈并返回
        public static void ReturnValue(object value)
        {
            ThreadCache.GetIL().NoErrorLoad(value);
            Return();
        }
    }
}
