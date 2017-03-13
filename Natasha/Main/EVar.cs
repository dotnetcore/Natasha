using System;
using System.Reflection.Emit;
using Natasha.Utils;
using Natasha.Core.Base;
using Natasha.Cache;
using Natasha.Debug;
using Natasha.Core;

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
            DataHelper.LoadObject(value);
            model.Store();
            model.Value = value;
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
            return OperatorHelper.CreateOperatorAction(source, dest, OperatorHelper.Add);
        }

        public static Action operator -(EVar source, object dest)
        {
            return OperatorHelper.CreateOperatorAction(source, dest, OperatorHelper.Sub);
        }

        public static Action operator *(EVar source, object dest)
        {
            return OperatorHelper.CreateOperatorAction(source, dest, OperatorHelper.Mul);
        }

        public static Action operator /(EVar source, object dest)
        {
            return OperatorHelper.CreateOperatorAction(source, dest, OperatorHelper.Div);
        }

        public static Action operator %(EVar source, object dest)
        {
            return OperatorHelper.CreateOperatorAction(source, dest, OperatorHelper.Rem);
        }

        public static Action operator >>(EVar source, int dest)
        {
            return () =>
            {
                source.RunCompareAction();
                OperatorHelper.Shr(dest);
            };
        }
        public static Action operator <<(EVar source, int dest)
        {
            return () =>
            {
                source.RunCompareAction();
                OperatorHelper.Shl(dest);
            };
        }
        public static Action operator |(EVar source, object dest)
        {
            return OperatorHelper.CreateOperatorAction(source, dest, OperatorHelper.Or);
        }

        public static Action operator &(EVar source, object dest)
        {
            return OperatorHelper.CreateOperatorAction(source, dest, OperatorHelper.And);
        }

        public static Action operator >(EVar source, object dest)
        {
            return OperatorHelper.CreateCompareAction(source, dest, OpCodes.Ble_S);
        }
        public static Action operator <(EVar source, object dest)
        {
            return OperatorHelper.CreateCompareAction(source, dest, OpCodes.Bge_S);
        }

        public static Action operator <=(EVar source, object dest)
        {
            return OperatorHelper.CreateCompareAction(source, dest, OpCodes.Bgt_S);
        }
        public static Action operator >=(EVar source, object dest)
        {
            return OperatorHelper.CreateCompareAction(source, dest, OpCodes.Blt_S);
        }

        public static Action operator ==(EVar source, object dest)
        {
            return () =>
            {
                source.RunCompareAction();
                if (source.TypeHandler.IsValueType && source.TypeHandler.IsPrimitive)
                {
                    ThreadCache.SetJudgeCode(OpCodes.Bne_Un_S);
                }
                else
                {
                    ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
                }
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    DataHelper.LoadObject(dest);
                }

                if (source.TypeHandler == typeof(string))
                {
                    source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
                    DebugHelper.WriteLine("Call "+ ClassCache.StringCompare.Name);
                }
            };
        }
        public static Action operator !=(EVar source, object dest)
        {
            
            return () =>
            {
                source.RunCompareAction();
                if (source.TypeHandler.IsValueType && source.TypeHandler.IsPrimitive)
                {
                    ThreadCache.SetJudgeCode(OpCodes.Beq_S);
                }
                else
                {
                    ThreadCache.SetJudgeCode(OpCodes.Brtrue_S);
                }
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    DataHelper.LoadObject(dest);
                }

                if (source.TypeHandler == typeof(string))
                {
                    source.ilHandler.Emit(OpCodes.Call, ClassCache.StringCompare);
                    DebugHelper.WriteLine("Call", ClassCache.StringCompare.Name);
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
