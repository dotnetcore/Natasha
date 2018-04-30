using Natasha;
using Natasha.Cache;
using Natasha.Core;

using System;
using System.Reflection.Emit;

namespace System.Reflection.Emit
{
    public static class OperatorExtension
    {
        public static Action CreateOperatorAction(this ILGenerator il,IOperator source, object dest, Action action)
        {
            return () => {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    il.NoErrorLoad(dest);
                }
                action();
            }; 
        }
        public static Action CreateCompareAction(this ILGenerator il,IOperator source, object dest, OpCode code)
        {
            return () => {
                source.RunCompareAction();
                ThreadCache.SetJudgeCode(code);
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    il.NoErrorLoad(dest);
                }
            };
        }

        public static void StringAdd(this ILGenerator il)
        {
            il.REmit(OpCodes.Call,ClassCache.StringJoin);
        }
        public static void Add(this ILGenerator il)
        {
           il.REmit(OpCodes.Add);
        }
        public static void Sub(this ILGenerator il)
        {
            il.REmit(OpCodes.Sub);
        }
        public static void Mul(this ILGenerator il)
        {
            il.REmit(OpCodes.Mul);
        }
        public static void Div(this ILGenerator il)
        {
            il.REmit(OpCodes.Div);
        }
        public static void Rem(this ILGenerator il)
        {
            il.REmit(OpCodes.Rem);
        }
        public static void Shr(this ILGenerator il,int dest)
        {
            if (dest < 255)
            {
                il.REmit(OpCodes.Ldc_I4_S, dest);
            }
            else
            {
                il.REmit(OpCodes.Ldc_I4, dest);
            }

            il.REmit(OpCodes.Ldc_I4_S, 31);
            il.REmit(OpCodes.And);
            il.REmit(OpCodes.Shr);

        }
        public static void Shl(this ILGenerator il,int dest)
        {
            if (dest < 255)
            {
                il.REmit(OpCodes.Ldc_I4_S, dest);
            }
            else
            {
                il.REmit(OpCodes.Ldc_I4, dest);
            }

            il.REmit(OpCodes.Ldc_I4_S, 31);
            il.REmit(OpCodes.And);
            il.REmit(OpCodes.Shl);

        }
        public static void Or(this ILGenerator il)
        {
            il.REmit(OpCodes.Or);
        }
        public static void And(this ILGenerator il)
        {
            il.REmit(OpCodes.And);
        }


        #region 拆装箱操作
        public static void UnPacket(this ILGenerator il,Type type)
        {
            if (type.IsClass && type != typeof(string) && type != typeof(object))
            {
                il.REmit(OpCodes.Castclass, type);
            }
            else if(type.IsValueType)
            {
                il.REmit(OpCodes.Unbox_Any, type);
            }
        }
        public static void Packet(this ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                il.REmit(OpCodes.Box, type);
            }
        }
        #endregion
        //复制压栈
        public static void Dup(this ILGenerator il)
        {
            il.REmit(OpCodes.Dup);
        }

        public static EModel Dup(this ILGenerator il,EModel model,Type type)
        {
            il.REmit(OpCodes.Dup);
            return il.GetLink(model,type);
        }

        //弹栈
        public static void Pop(this ILGenerator il)
        {
            il.REmit(OpCodes.Pop);
        }
        //初始化结构体
        public static void InitObject(this ILGenerator il,Type type)
        {
            il.REmit(OpCodes.Initobj, type);
        }
    }
}
