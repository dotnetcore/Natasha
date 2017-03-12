using Natasha.Cache;
using Natasha.Debug;
using Natasha.Utils;
using System;
using System.Reflection.Emit;
using System.Threading;

namespace Natasha
{
    //创建动态方法由此开始
    public class EHandler
    {

        private DynamicMethod newMethod;
        private ILGenerator il;
        private Type MethodType;

        private static int _index;
        public static int Index
        {
            get { return _index += 1; }
        }


        private Type[] ParameterTypes;
        private Type ReturnType;
        private string methodName;
        private string key;
        public EHandler(string name = "EmitMethod")
        {
            methodName = name;
            _index = 0;
        }
        /// <summary>
        /// 创建动态方法
        /// </summary>
        /// <typeparam name="R">返回值</typeparam>
        /// <param name="action">方法体</param>
        /// <param name="ilKey">当前线程下的另一个动态方法</param>
        /// <returns></returns>
        public static EHandler CreateMethod<R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        public static EHandler CreateMethod<T1, R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes<T1>();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action<T1>);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<T1,R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        public static EHandler CreateMethod<T1, T2, R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes<T1, T2>();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action<T1,T2>);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<T1,T2, R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        public static EHandler CreateMethod<T1, T2, T3, R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes<T1, T2, T3>();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action<T1, T2,T3>);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<T1, T2, T3, R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        public static EHandler CreateMethod<T1, T2, T3, T4, R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4>();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action<T1, T2, T3, T4>);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<T1, T2, T3, T4, R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        public static EHandler CreateMethod<T1, T2, T3, T4, T5, R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5>();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action<T1, T2, T3, T4, T5>);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<T1, T2, T3, T4, T5, R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        public static EHandler CreateMethod<T1, T2, T3, T4, T5, T6, R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6>();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action<T1, T2, T3, T4, T5, T6>);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<T1, T2, T3, T4, T5, T6, R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        public static EHandler CreateMethod<T1, T2, T3, T4, T5, T6, T7, R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6 ,T7>();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action<T1, T2, T3, T4, T5, T6, T7>);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<T1, T2, T3, T4, T5, T6, T7, R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        public static EHandler CreateMethod<T1, T2, T3, T4, T5, T6, T7, T8, R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6, T7,T8>();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8>);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<T1, T2, T3, T4, T5, T6, T7, T8, R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        public static EHandler CreateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(Action<ILGenerator> action,string ilKey=null)
        {
            EHandler handler = new EHandler();
            handler.ParameterTypes = MethodHelper.GetGenericTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
            if (typeof(R) == typeof(ENull))
            {
                handler.MethodType = typeof(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>);
                handler.ReturnType = null;
            }
            else
            {
                handler.MethodType = typeof(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>);
                handler.ReturnType = typeof(R);
            }
            handler.MakeMethodBody(action,ilKey);
            return handler;
        }
        private void MakeMethodBody(Action<ILGenerator> action, string ilKey)
        {
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            newMethod = new DynamicMethod(methodName + Index, ReturnType, ParameterTypes);

            il = newMethod.GetILGenerator();
            if (ilKey!=null)
            {

                DebugHelper.Start(ilKey);
                key = ilKey;
                ThreadCache.TKeyDict[ThreadId] = key;
                ThreadCache.TILDict[ThreadId] = il;
            }
            else
            {
                DebugHelper.Start(methodName + Index);
                ThreadCache.ILDict[ThreadId] = il;
            }
            
            if (action != null)
            {
                action(il);
            }
        }

        //编译成委托
        public Delegate Compile(Type type=null)
        {
            
            il.Emit(OpCodes.Ret);
           
            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            if (key!=null)
            {
                ThreadCache.TILDict.Remove(ThreadId);
                ThreadCache.TKeyDict.Remove(ThreadId);
                DebugHelper.WriteLine(key + "函数返回");
                DebugHelper.End();
            }
            else
            {
                
                ThreadCache.ILDict.Remove(ThreadId);
                DebugHelper.WriteLine("函数返回");
                DebugHelper.End();
            }
            if (type==null)
            {
                return newMethod.CreateDelegate(MethodType);
            }
            else
            {
                return newMethod.CreateDelegate(type);
            }
            
        }
    }
}
