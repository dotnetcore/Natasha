using System;
using System.Reflection.Emit;
using Natasha.Utils;
using Natasha.Core.Base;
using Natasha.Cache;

namespace Natasha
{
    //普通变量操作
    public class EVar : SingleType
    {
        private EVar(Type type)
            : base(type)
        {

        }
        private EVar(LocalBuilder builder, Type type)
            : base(builder, type)
        {

        }
        private EVar(int parameterIndex, Type type=null)
            : base(parameterIndex, type)
        {

        }
        private EVar(Type type, Action loadAction)
            : base(loadAction, type)
        {

        }

        #region 创建

        //创建参数形式的操作类
        public static EVar CreateVarFromParameter<T>(int parametersIndex)
        {
            return CreateVarFromParameter(parametersIndex, typeof(T));
        }
        public static EVar CreateVarFromParameter(int parametersIndex, Type type=null)
        {
            EVar model = new EVar(parametersIndex, type);
            return model;
        }
        //创建由临时变量指定的操作类
        public static EVar CreateVarFromBuilder<T>(LocalBuilder builder)
        {
            return CreateVarFromBuilder(builder, typeof(T));
        }
        public static EVar CreateVarFromBuilder(LocalBuilder builder, Type type)
        {
            EVar model = new EVar(builder, type);
            return model;
        }
        //创建无临时变量的操作类
        public static EVar CreateWithoutTempVar<S>(S value)
        {
            return CreateWithoutTempVar(value, typeof(S));
        }
        public static EVar CreateWithoutTempVar(object value, Type type)
        {
            EVar model = new EVar(type, null);
            model.Value = value;
            return model;

        }
        //创建由委托制定的操作类
        public static EVar CreateVarFromAction<T>(Action action)
        {
            return CreateVarFromAction(action, typeof(T));
        }
        public static EVar CreateVarFromAction(Action action, Type type=null)
        {
            EVar model = new EVar(type, action);
            return model;
        }
        //创建制定类型的临时变量操作类
        public static EVar CreateVar<T>()
        {
            return CreateVar(typeof(T));
        }
        public static EVar CreateVar(Type type)
        {
            EVar model = new EVar(type);
            return model;
        }
        //创建指定数据的临时变量操作类
        public static EVar CreateVarFromObject<T>(T value)
        {
            return CreateVarFromObject(value, typeof(T));
        }
        public static EVar CreateVarFromObject(object value, Type type)
        {
            EVar model = CreateVar(type);
            EData.LoadObject(value);
            model.Store();
            model.Instance = value;
            return model;
        }
        #endregion

        #region 隐式转换
        public static implicit operator EVar(int value)
        {
            return CreateWithoutTempVar(value); ;
        }
        public static implicit operator EVar(uint value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(short value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(ushort value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(long value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(ulong value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(float value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(double value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(decimal value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(byte value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(string value)
        {
            return CreateWithoutTempVar(value);
        }
        public static implicit operator EVar(char value)
        {
            return CreateWithoutTempVar((int)value);
        }
        public static implicit operator EVar(bool value)
        {
            return CreateWithoutTempVar(value);
        }

        #endregion



        #region 运算

        public static Action operator +(EVar source, object dest)
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

        public static Action operator -(EVar source, object dest)
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

        public static Action operator *(EVar source, object dest)
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

        public static Action operator /(EVar source, object dest)
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

        public static Action operator %(EVar source, object dest)
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

        public static Action operator >>(EVar source, int dest)
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
        public static Action operator <<(EVar source, int dest)
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
        public static Action operator |(EVar source, object dest)
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

        public static Action operator &(EVar source, object dest)
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

        public static Action operator >(EVar source, object dest)
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
        public static Action operator <(EVar source, object dest)
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

        public static Action operator <=(EVar source, object dest)
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
        public static Action operator >=(EVar source, object dest)
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

        public static Action operator ==(EVar source, object dest)
        {
            if (source.TypeHandler.IsValueType && source.TypeHandler.IsPrimitive)
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

                if (source.TypeHandler == typeof(string))
                {
                    source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
                }
            };
        }
        public static Action operator !=(EVar source, object dest)
        {
            if (source.TypeHandler.IsValueType && source.TypeHandler.IsPrimitive)
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

                if (source.TypeHandler == typeof(string))
                {
                    source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
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
        public static EVar operator ++(EVar source)
        {
            source.AddSelf();
            source.Store();
            return source;
        }
        #endregion

        #region 自减操作
        public static EVar operator --(EVar source)
        {
            source.SubSelf();
            source.Store();
            return source;
        }
        #endregion

    }
}
