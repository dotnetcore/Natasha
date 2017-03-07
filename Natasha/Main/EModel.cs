using Natasha.Cache;
using Natasha.Core;
using Natasha.Core.Base;
using Natasha.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha
{
    public class EModel : ComplexType
    {
        public static implicit operator EModel(Type value)
        {
            EModel instance = new EModel(value);
            return instance;
        }

        private EModel _model;
        public EModel Model
        {
            get
            {
                LoadEnd();
                return this;
            }
            set
            {
                _model = value;
            }

        }

        #region 初始化
        private EModel(Type type)
            : base(type)
        {

        }
        private EModel(Type type, Action action)
            : base(action, type)
        {

        }

        private EModel(LocalBuilder builder, Type type)
            : base(builder, type)
        {

        }
        private EModel(int parameterIndex, Type type)
            : base(parameterIndex, type)
        {

        }
        public EModel UseDefaultConstructor()
        {
            if (TypeHandler.IsClass && !TypeHandler.IsArray && TypeHandler != typeof(string) && Builder != null)
            {
                //使用默认构造函数
                ConstructorInfo ctor = TypeHandler.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
                if (ctor == null)
                {
                    throw new InvalidOperationException("Type [" + TypeHandler.FullName + "] should have default public or non-public constructor");
                }
                ilHandler.Emit(OpCodes.Newobj, ctor);
                ilHandler.Emit(OpCodes.Stloc, Builder);
            }
            return this;
        }

        public EModel Constructor<T1>(params object[] parameters)
        {
            Type[] arrayType = new Type[1];
            arrayType[0] = typeof(T1);
            ConstructorInfo ctor = TypeHandler.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, arrayType, null);
            return ExecuteConstructor(ctor, parameters);
        }
        public EModel Constructor<T1, T2>(params object[] parameters)
        {
            Type[] arrayType = new Type[2];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            ConstructorInfo ctor = TypeHandler.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, arrayType, null);
            return ExecuteConstructor(ctor, parameters);
        }
        public EModel Constructor<T1, T2, T3>(params object[] parameters)
        {
            Type[] arrayType = new Type[3];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            ConstructorInfo ctor = TypeHandler.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, arrayType, null);
            return ExecuteConstructor(ctor, parameters);
        }
        public EModel Constructor<T1, T2, T3, T4>(params object[] parameters)
        {
            Type[] arrayType = new Type[4];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            ConstructorInfo ctor = TypeHandler.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, arrayType, null);
            return ExecuteConstructor(ctor, parameters);
        }
        public EModel Constructor<T1, T2, T3, T4, T5>(params object[] parameters)
        {
            Type[] arrayType = new Type[4];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            arrayType[4] = typeof(T5);
            ConstructorInfo ctor = TypeHandler.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, arrayType, null);
            return ExecuteConstructor(ctor, parameters);
        }
        public EModel Constructor<T1, T2, T3, T4, T5, T6>(params object[] parameters)
        {
            Type[] arrayType = new Type[5];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            arrayType[4] = typeof(T5);
            arrayType[5] = typeof(T6);
            ConstructorInfo ctor = TypeHandler.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, arrayType, null);
            return ExecuteConstructor(ctor, parameters);
        }
        public EModel Constructor<T1, T2, T3, T4, T5, T6, T7>(params object[] parameters)
        {
            Type[] arrayType = new Type[5];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            arrayType[4] = typeof(T5);
            arrayType[5] = typeof(T6);
            arrayType[6] = typeof(T7);
            ConstructorInfo ctor = TypeHandler.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, arrayType, null);
            return ExecuteConstructor(ctor, parameters);
        }
        public EModel Constructor<T1, T2, T3, T4, T5, T6, T7, T8>(params object[] parameters)
        {
            Type[] arrayType = new Type[5];
            arrayType[0] = typeof(T1);
            arrayType[1] = typeof(T2);
            arrayType[2] = typeof(T3);
            arrayType[3] = typeof(T4);
            arrayType[4] = typeof(T5);
            arrayType[5] = typeof(T6);
            arrayType[6] = typeof(T7);
            arrayType[7] = typeof(T8);
            ConstructorInfo ctor = TypeHandler.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, arrayType, null);
            return ExecuteConstructor(ctor, parameters);
        }
        private EModel ExecuteConstructor(ConstructorInfo ctor, object[] parameters)
        {
            if (ctor == null)
            {
                throw new InvalidOperationException("Type [" + TypeHandler.FullName + "] should have default public or non-public constructor");
            }
            for (int i = 0; i < parameters.Length; i += 1)
            {
                EData.NoErrorLoad(parameters[i], ilHandler);
            }
            ilHandler.Emit(OpCodes.Newobj, ctor);
            ilHandler.Emit(OpCodes.Stloc, Builder);
            return this;
        }
        //创建参数模式的model操作类
        public static EModel CreateModelFromParameter<T>(int parametersIndex)
        {
            return CreateModelFromParameter(parametersIndex, typeof(T));
        }
        public static EModel CreateModelFromParameter(int parametersIndex, Type type)
        {
            EModel model = new EModel(parametersIndex, type);
            return model;
        }

        //创建临时变量模式的model临时变量
        public static EModel CreateModelFromBuilder<T>(LocalBuilder builder)
        {
            return CreateModelFromBuilder(builder, typeof(T));
        }
        public static EModel CreateModelFromBuilder(LocalBuilder builder, Type type)
        {
            EModel model = new EModel(builder, type);
            return model;
        }

        //创建委托模式的model操作类
        public static EModel CreateModelFromAction<T>(Action action)
        {
            return CreateModelFromAction(action, typeof(T));
        }
        public static EModel CreateModelFromAction(Action action, Type type)
        {
            EModel model = new EModel(type, action);
            return model;
        }
        //创建带有指定的外部数据的model临时变量
        public static EModel CreateModelFromObject<T>(T value)
        {
            return CreateModelFromObject(value, typeof(T));
        }
        public static EModel CreateModelFromObject(object value, Type type)
        {
            EModel model = CreateModelFromAction(null, type);
            model.Builder = EClone.GetCloneBuilder(value, type);
            model.Instance = value;
            return model;
        }

        //创建已经初始化的本地临时变量
        public static EModel CreateModel<T>()
        {
            return CreateModel(typeof(T));
        }
        public static EModel CreateModel(Type type)
        {
            return new EModel(type);
        }

        //操作动态类
        public static EModel CreateDynamicClass(string classKey)
        {
            EModel model = null;
            if (ClassCache.DynamicClassDict.ContainsKey(classKey))
            {
                Type type = ClassCache.DynamicClassDict[classKey];
                model = new EModel(type);
            }
            return model;
        }
        public static EModel CreateDynamicClass(string classKey, LocalBuilder builder)
        {
            EModel instance = null;
            if (ClassCache.DynamicClassDict.ContainsKey(classKey))
            {
                Type type = ClassCache.DynamicClassDict[classKey];
                instance = new EModel(builder, type);
            }
            return instance;
        }
        public static EModel CreateDynamicClass(string classKey, int parameterIndex)
        {
            EModel instance = null;

            if (ClassCache.DynamicClassDict.ContainsKey(classKey))
            {
                Type type = ClassCache.DynamicClassDict[classKey];
                instance = new EModel(parameterIndex, type);
            }
            return instance;
        }
        #endregion

        #region 运算

        public static Action operator +(EModel source, object dest)
        {
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
                source.ilHandler.Emit(OpCodes.Add);
            };

        }

        public static Action operator -(EModel source, object dest)
        {
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
                source.ilHandler.Emit(OpCodes.Sub);
            };
        }

        public static Action operator *(EModel source, object dest)
        {
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
                source.ilHandler.Emit(OpCodes.Mul);
            };
        }

        public static Action operator /(EModel source, object dest)
        {
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
                source.ilHandler.Emit(OpCodes.Div);
            };
        }

        public static Action operator %(EModel source, object dest)
        {
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
                source.ilHandler.Emit(OpCodes.Rem);
            };
        }

        public static Action operator >>(EModel source, int dest)
        {
            return () =>
            {
                source.RunCompareAction();
                source.ilHandler.Emit(OpCodes.Ldc_I4, dest);
                source.ilHandler.Emit(OpCodes.Ldc_I4_S, 31);
                source.ilHandler.Emit(OpCodes.And);
                source.ilHandler.Emit(OpCodes.Shr);
            };
        }
        public static Action operator <<(EModel source, int dest)
        {
            return () =>
            {
                ILGenerator il = ThreadCache.GetIL();
                source.RunCompareAction();
                source.ilHandler.Emit(OpCodes.Ldc_I4, dest);
                source.ilHandler.Emit(OpCodes.Ldc_I4_S, 31);
                source.ilHandler.Emit(OpCodes.And);
                source.ilHandler.Emit(OpCodes.Shl);
            };
        }
        public static Action operator |(EModel source, object dest)
        {
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
                source.ilHandler.Emit(OpCodes.Or);
            };
        }

        public static Action operator &(EModel source, object dest)
        {
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
                source.ilHandler.Emit(OpCodes.And);
            };
        }

        public static Action operator >(EModel source, object dest)
        {
            ThreadCache.SetJudgeCode(OpCodes.Ble_S);
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
            };
        }
        public static Action operator <(EModel source, object dest)
        {
            ThreadCache.SetJudgeCode(OpCodes.Bge_S);
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
            };
        }

        public static Action operator <=(EModel source, object dest)
        {
            ThreadCache.SetJudgeCode(OpCodes.Bgt_S);
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
            };
        }
        public static Action operator >=(EModel source, object dest)
        {
            ThreadCache.SetJudgeCode(OpCodes.Blt_S);
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
            };
        }
        public static Action operator ==(EModel source, object dest)
        {
            if (source.CompareType.IsValueType && source.CompareType.IsPrimitive)
            {
                ThreadCache.SetJudgeCode(OpCodes.Bne_Un_S);
            }
            else
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }

            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }
                if (source.CompareType == typeof(string))
                {
                    source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
                }
                else if (source.CompareType.IsClass || (source.CompareType.IsValueType && !source.CompareType.IsPrimitive))
                {
                    source.ilHandler.Emit(OpCodes.Call, ClassCache.ClassCompare);
                }
            };
        }
        public static Action operator !=(EModel source, object dest)
        {
            if (source.CompareType.IsValueType && source.CompareType.IsPrimitive)
            {
                ThreadCache.SetJudgeCode(OpCodes.Beq_S);
            }
            else
            {
                ThreadCache.SetJudgeCode(OpCodes.Brtrue_S);
            }
            return () =>
            {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    EData.LoadObject(dest);
                }

                if (source.CompareType == typeof(string))
                {
                    source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
                }
                else if (source.CompareType.IsClass || (source.CompareType.IsValueType && !source.CompareType.IsPrimitive))
                {
                    source.ilHandler.Emit(OpCodes.Call, ClassCache.ClassCompare);
                }
            };
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region 自加运算


        public static EModel operator ++(EModel source)
        {
            source.AddSelf();
            return source;
        }


        #endregion
        #region 自减操作
        public static EModel operator --(EModel source)
        {
            source.SubSelf();
            return source;
        }

        #endregion

    }
}
