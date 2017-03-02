using Natasha.Cache;
using Natasha.Core;
using Natasha.Core.Base;
using System;
using System.Reflection.Emit;
using System.Threading;

namespace Natasha.Utils
{
    //神器的数据操作类
    public class EData
    {
        //各种数据给我就能入栈
        public static void NoErrorLoad(object value,ILGenerator ilHandler)
        {
            ENull enull = value as ENull;
            if (value != null)
            {
                Type type = value.GetType();
                ILoadInstance instance = value as ILoadInstance;
                LocalBuilder builder = value as LocalBuilder;
                IOperator modelOperator = value as IOperator;
                if (value is Action)
                {
                    ((Action)value)();
                }
                else if (instance!=null)
                {
                    instance.Load();
                }
                else if (builder != null)
                {
                    ilHandler.Emit(OpCodes.Ldloc, builder);
                }
                else if (enull != null)
                {
                    ilHandler.Emit(OpCodes.Ldnull);
                }
                else if (type.IsClass && type != typeof(string))
                {
                    EModel model = EModel.CreateModelFromObject(value, type);
                    model.Load();
                }
                else if (type.IsEnum)
                {
                    ilHandler.Emit(OpCodes.Ldc_I4, (int)value);
                }
                else if (type.IsValueType && !type.IsPrimitive)
                {
                    EModel model;
                    if (EStruckCheck.IsDefaultStruct(value, type))
                    {
                        model = EModel.CreateModel(type);
                    }
                    else
                    {
                        model = EModel.CreateModelFromObject(value, type);
                    }
                    model.Load();
                }
                else
                {
                    EData.LoadObject(value);
                }
            }
            
        }
      

        /// <summary>
        /// 根据索引加载参数
        /// </summary>
        /// <param name="index">参数索引</param>
        public static void LoadParameter(int index) {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldarg_S, index);
        }
        public static void Load(string value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldstr, value);
        }
        public static void Load(byte value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_I4, value);
        }
        public static void Load(int value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_I4, value);
        }
        public static void Load(short value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_I4, value);
        }
        public static void Load(ushort value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_I4, value);
        }
        public static void Load(uint value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_I4, value);
        }
        public static void Load(long value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_I4, value);
            il.Emit(OpCodes.Conv_I8);
        }
        public static void Load(ulong data)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_I4, data);
            il.Emit(OpCodes.Conv_I8);
        }
        public static void Load(decimal value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_I4, (int)value);
        }
        public static void Load(float value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_R4, value);
        }
        public static void Load(double value)
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ldc_R8, value);
        }
        public static void Load(bool value)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (value)
            {
                il.Emit(OpCodes.Ldc_I4_1);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4_0);
            }
            ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
        }

        //给我一个普通变量我就入栈
        public static void LoadObject(object value)
        {
            if (value == null)
            {
                return;
            }
            ILGenerator il = ThreadCache.GetIL();
            Type EType = value.GetType();
            if (EType == typeof(int))
            {
                int result;
                if (int.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_I4, result);
                }
            } 
            else if (EType == typeof(string))
            {
                string result = value.ToString();
                il.Emit(OpCodes.Ldstr, result);
            }
            else if (EType == typeof(double))
            {
                double result;
                if (double.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_R8, result);
                }
            }
            else if (EType == typeof(float))
            {
                float result;
                if (float.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_R4, result);
                }
            }
            else if (EType == typeof(bool))
            {
                if ((bool)value)
                {
                    il.Emit(OpCodes.Ldc_I4_1);
                }
                else
                {
                    il.Emit(OpCodes.Ldc_I4_0);
                }
                ThreadCache.CodeDict[Thread.CurrentThread.ManagedThreadId] = OpCodes.Brfalse_S;
            }
            else if (EType == typeof(byte))
            {
                int result;
                if (int.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_I4, result);
                }
            }
            else if (EType == typeof(short))
            {
                short result;
                if (short.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_I4, result);
                }
            }
            else if (EType == typeof(ushort))
            {
                ushort result;
                if (ushort.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_I4, result);
                }
            }
            else if (EType == typeof(long))
            {
                long result;
                if (long.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_I4, result);
                    il.Emit(OpCodes.Conv_I8);
                }
            }
            else if (EType == typeof(ulong))
            {
                ulong result;
                if (ulong.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_I4, result);
                    il.Emit(OpCodes.Conv_I8);
                }
            }
            else if (EType == typeof(decimal))
            {
                decimal result;
                if (decimal.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_I4, (int)result);
                }
            }
        }

        //自增量
        public static object GetSelfOperator(Type type)
        {
            object value = null;
            if (type == typeof(int))
            {
                value = 1;
            }
            else if (type == typeof(double))
            {
                value = (double)1;
            }
            else if (type == typeof(long))
            {
                value = (long)1;
            }
            else if (type == typeof(float))
            {
                value = (float)1;
            }
            else if (type == typeof(short))
            {
                value = (short)1;
            }
            else if (type == typeof(byte))
            {
                value = (byte)1;
            }
            else if (type == typeof(uint))
            {
                value = (uint)1;
            }
            else if (type == typeof(ulong))
            {
                value = (ulong)1;
            }
            else if (type == typeof(ushort))
            {
                value = (ushort)1;
            }
            return value;
        }

        //自增量入栈
        public static void LoadSelfObject(Type type)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (type == typeof(int))
            {
                il.Emit(OpCodes.Ldc_I4_1);
            }
            else if (type == typeof(double))
            {
                il.Emit(OpCodes.Ldc_R8, 1.00);
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Conv_I8);
            }
            else if (type == typeof(float))
            {
                il.Emit(OpCodes.Ldc_R4, 1.0);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4_1);
            }
        }
    }
}
