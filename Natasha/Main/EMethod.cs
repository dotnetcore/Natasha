using Natasha.Cache;
using Natasha.Utils;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Natasha
{
    //函数操作类
    public class EMethod
    {
        private Type _typeHandler;
        private Type[] _genericTypes;
        private Type[] _methodTypes;
        private EMethod()
        {
        }

        public static implicit operator EMethod(Type value)
        {
            EMethod instance = new EMethod();
            if (value != null)
            {
                instance._typeHandler = value;
            }
            return instance;
        }
        public EMethod Use<T>()
        {
            return Use(typeof(T));
        }
        public EMethod Use(Type type)
        {
            _typeHandler = type;
            return this;
        }
        public static EMethod Load<T>(Type type)
        {
            return Load(typeof(T));
        }
        public static EMethod Load(Type type)
        {
            EMethod isntance = type;
            return isntance;
        }
        public static EMethod Load(EModel instance)
        {
            EMethod isntance = instance.TypeHandler;
            instance.This();
            return isntance;
        }
        public static EMethod Load(EVar value)
        {
            EMethod isntance = value.TypeHandler;
            value.This();
            return isntance;
        }

        public EMethod AddGenricType(params Type[] types)
        {
            _genericTypes = types;
            return this;
        }
        public EMethod AddGenricType<T1>()
        {
            _genericTypes = MethodHelper.GetGenericTypes<T1>();
            return this;
        }
        public EMethod AddGenricType<T1, T2>()
        {
            _genericTypes = MethodHelper.GetGenericTypes<T1, T2>();
            return this;
        }
        public EMethod AddGenricType<T1, T2, T3>()
        {
            _genericTypes = MethodHelper.GetGenericTypes<T1, T2, T3>();
            return this;
        }
        public EMethod AddGenricType<T1, T2, T3, T4>()
        {
            _genericTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4>();
            return this;
        }
        public EMethod AddGenricType<T1, T2, T3, T4, T5>()
        {
            _genericTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5>();
            return this;
        }
        public EMethod AddGenricType<T1, T2, T3, T4, T5, T6>()
        {
            _genericTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6>();
            return this;
        }
        public EMethod AddGenricType<T1, T2, T3, T4, T5, T6, T7>()
        {
            _genericTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6, T7>();
            return this;
        }
        public EMethod AddGenricType<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            _genericTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6, T7, T8>();
            return this;
        }

        public EMethod ExecuteMethod(string methodName, Action action = null)
        {
            _methodTypes= MethodHelper.GetGenericTypes();
            return Execute(GetCommonMethodInfo(methodName), action);
        }
        public EMethod ExecuteMethod<T1>(string methodName, Action action = null)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1>();
            return Execute(GetCommonMethodInfo(methodName), action);
        }
        public EMethod ExecuteMethod<T1, T2>(string methodName, Action action = null)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2>();
            return Execute(GetCommonMethodInfo(methodName), action);
        }
        public EMethod ExecuteMethod<T1, T2, T3>(string methodName, Action action = null)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3>();
            return Execute(GetCommonMethodInfo(methodName), action);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4>(string methodName, Action action = null)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4>();
            return Execute(GetCommonMethodInfo(methodName), action);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4, T5>(string methodName, Action action = null)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5>();
            return Execute(GetCommonMethodInfo(methodName), action);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4, T5, T6>(string methodName, Action action = null)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6>();
            return Execute(GetCommonMethodInfo(methodName), action);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, Action action = null)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6, T7>();
            return Execute(GetCommonMethodInfo(methodName), action);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, Action action = null)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6, T7, T8>();
            return Execute(GetCommonMethodInfo(methodName), action);
        }
        public EMethod ExecuteMethod(string methodName, params object[] parameters)
        {
            _methodTypes = MethodHelper.GetGenericTypes();
            return Execute(GetCommonMethodInfo(methodName), new object[0]);
        }
        public EMethod ExecuteMethod<T1>(string methodName, params object[] parameters)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1>();
            return Execute(GetCommonMethodInfo(methodName), parameters);
        }
        public EMethod ExecuteMethod<T1, T2>(string methodName, params object[] parameters)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2>();
            return Execute(GetCommonMethodInfo(methodName), parameters);
        }
        public EMethod ExecuteMethod<T1, T2, T3>(string methodName, params object[] parameters)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3>();
            return Execute(GetCommonMethodInfo(methodName), parameters);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4>(string methodName, params object[] parameters)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4>();
            return Execute(GetCommonMethodInfo(methodName), parameters);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4, T5>(string methodName, params object[] parameters)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5>();
            return Execute(GetCommonMethodInfo(methodName), parameters);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4, T5, T6>(string methodName, params object[] parameters)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6>();
            return Execute(GetCommonMethodInfo(methodName), parameters);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4, T5, T6, T7>(string methodName, params object[] parameters)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6, T7>();
            return Execute(GetCommonMethodInfo(methodName), parameters);
        }
        public EMethod ExecuteMethod<T1, T2, T3, T4, T5, T6, T7, T8>(string methodName, params object[] parameters)
        {
            _methodTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6, T7, T8>();
            return Execute(GetCommonMethodInfo(methodName), parameters);
        }
        public EMethod Execute(string methodName, params Type[] types)
        {
            _methodTypes = types;
            return Execute(GetCommonMethodInfo(methodName));
        }

        public EMethod Execute(MethodInfo info, Action action = null)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (action != null)
            {
                action();
            }
            EmitHelper.CallMethod(_typeHandler, info);
            if (info.ReturnType != null)
            {
                EMethod newMethod = info.ReturnType;
                return newMethod;
            }
            else
            {
                return null;
            }
        }
        public EMethod Execute(MethodInfo info, object[] instances)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (instances != null)
            {
                for (int i = 0; i < instances.Length; i++)
                {
                    if (instances[i] != null)
                    {
                        EmitHelper.NoErrorLoad(instances[i], il);
                    }
                }
            }
            EmitHelper.CallMethod(_typeHandler, info);
            if (info.ReturnType != null)
            {
                EMethod newMethod = info.ReturnType;
                return newMethod;
            }
            else
            {
                return null;
            }
        }


        public MethodInfo GetCommonMethodInfo(string methodName)
        {

            MethodInfo methodInfo = _typeHandler.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static, null, _methodTypes, null);
            if (methodInfo != null && !methodInfo.IsGenericMethod)
            {
                return methodInfo;
            }
            MethodInfo[] methods = _typeHandler.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            for (int i = 0; i < methods.Length; i += 1)
            {
                if (methods[i].Name == methodName)
                {
                    if (!methods[i].IsGenericMethod)
                    {
                        methodInfo = methods[i];
                    }
                    else
                    {
                        methodInfo = methods[i].MakeGenericMethod(_genericTypes);
                    }
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    int Length = parameters.Length;
                    int methodTypeLength = _methodTypes.Length;
                    int increase = 0;
                    if (methodInfo.IsDefined(typeof(ExtensionAttribute), false))
                    {
                        Length -= 1;
                        increase = 1;
                    }
                    if (methodTypeLength == 0)
                    {
                        if (Length == 0)
                        {
                            break;
                        }
                    }
                    else if (methodTypeLength == Length)
                    {
                        bool shut = true;
                        for (int j = 0; j < methodTypeLength; j += 1)
                        {
                            if (_methodTypes[j] != parameters[j + increase].ParameterType)
                            {
                                shut = false;
                            }
                        }
                        if (shut)
                        {
                            break;
                        }
                    }
                }
            }
            return methodInfo;
        }
    }
}
