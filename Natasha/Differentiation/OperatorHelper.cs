using Natasha.Cache;
using Natasha.Core;
using Natasha.Debug;
using System;
using System.Reflection.Emit;

namespace Natasha.Utils
{
    public class OperatorHelper
    {
        public static Action CreateOperatorAction(IOperator source, object dest, Action action)
        {
            return () => {
                source.RunCompareAction();
                if (dest is IOperator)
                {
                    ((IOperator)dest).RunCompareAction();
                }
                else
                {
                    DataHelper.LoadObject(dest);
                }
                action();
            }; 
        }
        public static Action CreateCompareAction(IOperator source, object dest, OpCode code)
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
                    DataHelper.LoadObject(dest);
                }
            };
        }
        public static void Add()
        {
           ThreadCache.GetIL().Emit(OpCodes.Add);
           DebugHelper.WriteLine("Add");
        }
        public static void Sub()
        {
            ThreadCache.GetIL().Emit(OpCodes.Sub);
            DebugHelper.WriteLine("Sub");
        }
        public static void Mul()
        {
            ThreadCache.GetIL().Emit(OpCodes.Mul);
            DebugHelper.WriteLine("Mul");
        }
        public static void Div()
        {
            ThreadCache.GetIL().Emit(OpCodes.Div);
            DebugHelper.WriteLine("Div");
        }
        public static void Rem()
        {
            ThreadCache.GetIL().Emit(OpCodes.Rem);
            DebugHelper.WriteLine("Rem");
        }
        public static void Shr(int dest)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (dest < 255)
            {
                il.Emit(OpCodes.Ldc_I4_S, dest);
                DebugHelper.WriteLine("Ldc_I4_S", dest);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, dest);
                DebugHelper.WriteLine("Ldc_I4", dest);
            }

            il.Emit(OpCodes.Ldc_I4_S, 31);
            il.Emit(OpCodes.And);
            il.Emit(OpCodes.Shr);

            DebugHelper.WriteLine("Ldc_I4_S", 31);
            DebugHelper.WriteLine("And");
            DebugHelper.WriteLine("Shr");

        }

        public static void Shl(int dest)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (dest < 255)
            {
                il.Emit(OpCodes.Ldc_I4_S, dest);
                DebugHelper.WriteLine("Ldc_I4_S", dest);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, dest);
                DebugHelper.WriteLine("Ldc_I4", dest);
            }

            il.Emit(OpCodes.Ldc_I4_S, 31);
            il.Emit(OpCodes.And);
            il.Emit(OpCodes.Shl);

            DebugHelper.WriteLine("Ldc_I4_S", 31);
            DebugHelper.WriteLine("And");
            DebugHelper.WriteLine("Shl");

        }

        public static void Or()
        {
            ThreadCache.GetIL().Emit(OpCodes.Or);
            DebugHelper.WriteLine("Or");
        }
        public static void And()
        {
            ThreadCache.GetIL().Emit(OpCodes.And);
            DebugHelper.WriteLine("And");
        }


        #region 拆装箱操作
        public static void UnPacket(Type type)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (type.IsClass && type != typeof(string) && type != typeof(object))
            {
                il.Emit(OpCodes.Castclass, type);
                DebugHelper.WriteLine("Castclass", type.Name);
            }
            else
            {
                il.Emit(OpCodes.Unbox_Any, type);
                DebugHelper.WriteLine("Unbox_Any", type.Name);
            }
        }
        public static void Packet(Type type)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
                DebugHelper.WriteLine("Box", type.Name);
            }
        }
        #endregion
        //复制压栈
        public static void Dup()
        {
            ThreadCache.GetIL().Emit(OpCodes.Dup);
        }

        //弹栈
        public static void Pop()
        {
            ThreadCache.GetIL().Emit(OpCodes.Pop);
        }
        //初始化结构体
        public static void InitObject(Type type)
        {
            ThreadCache.GetIL().Emit(OpCodes.Initobj, type);
            DebugHelper.WriteLine("Initobj", type.ToString());
        }
    }
}
