using Natasha.Cache;
using Natasha.Core;
using Natasha.Utils;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha
{
    //函数操作类
    public class EMethod
    {
        public Type TypeHandler;

        public static implicit operator EMethod(Type value)
        {
            EMethod instance = new EMethod();
            instance.TypeHandler = value;
            return instance;
        }
        public MethodInfo GetMethodInfo(string methodName)
        {
            return TypeHandler.GetMethod(methodName, new Type[0]);
        }
        public MethodInfo GetMethodInfo<T1>(string methodName)
        {
            Type[] arrayType = new Type[1];
            arrayType[0] = typeof(T1);
            return TypeHandler.GetMethod(methodName, arrayType);
        }
        public MethodInfo GetMethodInfo<T1,T2>(string methodName)
        {
            Type[] arrayType = new Type[2];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            return TypeHandler.GetMethod(methodName, arrayType);
        }
        public MethodInfo GetMethodInfo<T1, T2, T3>(string methodName)
        {
            Type[] arrayType = new Type[3];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            return TypeHandler.GetMethod(methodName, arrayType);
        }
        public MethodInfo GetMethodInfo<T1, T2, T3, T4>(string methodName)
        {
            Type[] arrayType = new Type[4];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            return TypeHandler.GetMethod(methodName, arrayType);
        }
        public MethodInfo GetMethodInfo<T1, T2, T3, T4, T5>(string methodName)
        {
            Type[] arrayType = new Type[5];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            arrayType[4] = typeof(T5);
            return TypeHandler.GetMethod(methodName, arrayType);
        }
        public MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6>(string methodName)
        {
            Type[] arrayType = new Type[6];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            arrayType[4] = typeof(T5);
            arrayType[5] = typeof(T6);
            return TypeHandler.GetMethod(methodName, arrayType);
        }
        public MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(string methodName)
        {
            Type[] arrayType = new Type[7];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            arrayType[4] = typeof(T5);
            arrayType[5] = typeof(T6);
            arrayType[6] = typeof(T7);
            return TypeHandler.GetMethod(methodName, arrayType);
        }
        public MethodInfo GetMethodInfo<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName)
        {
            Type[] arrayType = new Type[8];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            arrayType[4] = typeof(T5);
            arrayType[5] = typeof(T6);
            arrayType[6] = typeof(T7);
            arrayType[7] = typeof(T8);
            return TypeHandler.GetMethod(methodName, arrayType);
        }
        public MethodInfo GetMethodInfo(string methodName, params Type[] types)
        {
            return TypeHandler.GetMethod(methodName, types);
        }

        public void ExecuteMethod(string methodName, Action action = null)
        {
            ExecuteMethod(GetMethodInfo(methodName), action);
        }
        public void ExecuteMethod<T1>(string methodName,Action action=null) {

            ExecuteMethod(GetMethodInfo<T1>(methodName), action);
        }
        public void ExecuteMethod<T1, T2>(string methodName, Action action = null)
        {
            ExecuteMethod(GetMethodInfo<T1, T2>(methodName), action);
        }
        public void ExecuteMethod<T1, T2, T3>(string methodName, Action action = null)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3>(methodName), action);
        }
        public void ExecuteMethod<T1, T2, T3, T4>(string methodName, Action action = null)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4>(methodName), action);
        }
        public void ExecuteMethod<T1, T2, T3, T4, T5>(string methodName, Action action = null)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4, T5>(methodName), action);
        }
        public void ExecuteMethod<T1, T2, T3, T4, T5, T6>(string methodName, Action action = null)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4, T5, T6>(methodName), action);
        }
        public void ExecuteMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Action action = null)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(methodName), action);
        }
        public void ExecuteMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Action action = null)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4, T5, T6, T7, T8>(methodName), action);
        }
        public void ExecuteMethod(string methodName, params object[] parameters)
        {
            ExecuteMethod(GetMethodInfo(methodName),new object[0]);
        }
        public void ExecuteMethod<T1>(string methodName, params object[] parameters)
        {
            ExecuteMethod(GetMethodInfo<T1>(methodName), parameters);
        }
        public void ExecuteMethod<T1, T2>(string methodName, params object[] parameters)
        {
            ExecuteMethod(GetMethodInfo<T1, T2>(methodName), parameters);
        }
        public void ExecuteMethod<T1, T2, T3>(string methodName, params object[] parameters)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3>(methodName), parameters);
        }
        public void ExecuteMethod<T1, T2, T3, T4>(string methodName, params object[] parameters)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4>(methodName), parameters);
        }
        public void ExecuteMethod<T1, T2, T3, T4, T5>(string methodName, params object[] parameters)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4, T5>(methodName), parameters);
        }
        public void ExecuteMethod<T1, T2, T3, T4, T5, T6>(string methodName, params object[] parameters)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4, T5, T6>(methodName), parameters);
        }
        public void ExecuteMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, params object[] parameters)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4, T5, T6, T7>(methodName), parameters);
        }
        public void ExecuteMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, params object[] parameters)
        {
            ExecuteMethod(GetMethodInfo<T1, T2, T3, T4, T5, T6, T7, T8>(methodName), parameters);
        }
        public void ExecuteMethod(string methodName, params Type[] types)
        {
            MethodInfo info = GetMethodInfo(methodName, types);
            ExecuteMethod(info);
        }
        public void ExecuteMethod(MethodInfo info,Action action=null)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (action!=null)
            {
                action();
            }
            if (info.IsStatic)
            {
                il.Emit(OpCodes.Call, info);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, info);
            }
        }
        public void ExecuteMethod(MethodInfo info, object[] instances)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (instances!=null)
            {
                for (int i = 0; i < instances.Length; i++)
                {
                    if (instances[i]!=null)
                    {
                        EData.NoErrorLoad(instances[i], il);
                    }
                }
            }
            
            if (info.IsStatic)
            {
                il.Emit(OpCodes.Call, info);
            }
            else
            {
                il.Emit(OpCodes.Callvirt, info);
            }
        }
    }
}
