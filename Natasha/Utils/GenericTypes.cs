using System;

namespace Natasha.Utils
{
    public class GenericTypes
    {
        public Type[] GetGenericTypes()
        {
            return new Type[0];
        }
        public Type[] GetGenericTypes<T1>()
        {
            Type[] methodParameterTypes = new Type[1];
            methodParameterTypes[0] = typeof(T1);
            return methodParameterTypes;
        }
        public Type[] GetGenericTypes<T1, T2>()
        {
            Type[] methodParameterTypes = new Type[2];
            methodParameterTypes[0] = typeof(T1);
            methodParameterTypes[1] = typeof(T2);
            return methodParameterTypes;
        }
        public Type[] GetGenericTypes<T1, T2, T3>()
        {
            Type[] methodParameterTypes = new Type[3];
            methodParameterTypes[0] = typeof(T1);
            methodParameterTypes[1] = typeof(T2);
            methodParameterTypes[2] = typeof(T3);
            return methodParameterTypes;
        }
        public Type[] GetGenericTypes<T1, T2, T3, T4>()
        {
            Type[] methodParameterTypes = new Type[4];
            methodParameterTypes[0] = typeof(T1);
            methodParameterTypes[1] = typeof(T2);
            methodParameterTypes[2] = typeof(T3);
            methodParameterTypes[3] = typeof(T4);
            return methodParameterTypes;
        }
        public Type[] GetGenericTypes<T1, T2, T3, T4, T5>()
        {
            Type[] methodParameterTypes = new Type[5];
            methodParameterTypes[0] = typeof(T1);
            methodParameterTypes[1] = typeof(T2);
            methodParameterTypes[2] = typeof(T3);
            methodParameterTypes[3] = typeof(T4);
            methodParameterTypes[4] = typeof(T5);
            return methodParameterTypes;
        }
        public Type[] GetGenericTypes<T1, T2, T3, T4, T5, T6>()
        {
            Type[] methodParameterTypes = new Type[6];
            methodParameterTypes[0] = typeof(T1);
            methodParameterTypes[1] = typeof(T2);
            methodParameterTypes[2] = typeof(T3);
            methodParameterTypes[3] = typeof(T4);
            methodParameterTypes[4] = typeof(T5);
            methodParameterTypes[5] = typeof(T6);
            return methodParameterTypes;
        }
        public Type[] GetGenericTypes<T1, T2, T3, T4, T5, T6, T7>()
        {
            Type[] methodParameterTypes = new Type[7];
            methodParameterTypes[0] = typeof(T1);
            methodParameterTypes[1] = typeof(T2);
            methodParameterTypes[2] = typeof(T3);
            methodParameterTypes[3] = typeof(T4);
            methodParameterTypes[4] = typeof(T5);
            methodParameterTypes[5] = typeof(T6);
            methodParameterTypes[6] = typeof(T7);
            return methodParameterTypes;
        }
        public Type[] GetGenericTypes<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            Type[] methodParameterTypes = new Type[8];
            methodParameterTypes[0] = typeof(T1);
            methodParameterTypes[1] = typeof(T2);
            methodParameterTypes[2] = typeof(T3);
            methodParameterTypes[3] = typeof(T4);
            methodParameterTypes[4] = typeof(T5);
            methodParameterTypes[5] = typeof(T6);
            methodParameterTypes[6] = typeof(T7);
            methodParameterTypes[7] = typeof(T8);
            return methodParameterTypes;
        }
    }
}
