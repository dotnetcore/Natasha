using Natasha;
using Natasha.Cache;
using Natasha.Core;

using Natasha.Utils;

namespace System.Reflection.Emit
{
    public static class DataExtension
    {
        #region 数据入栈
        //各种数据给我就能入栈
        public static void NoErrorLoad(this ILGenerator il, object value,Type type)
        {
            if (il.IsNullable(type))
            {
                if (value == null)
                {
                    EModel model = EModel.CreateModel(type).UseDefaultConstructor();
                    model.Load();
                }
                else
                {
                    Action action = value as Action;
                    if (action==null)
                    {
                        il.NoErrorLoad(value, type.GenericTypeArguments[0]);
                        ConstructorInfo ctor = type.GetConstructor(new Type[] { type.GenericTypeArguments[0] });
                        il.REmit(OpCodes.Newobj, ctor);
                    }
                    else
                    {
                        action();
                        ConstructorInfo ctor = type.GetConstructor(new Type[] { type.GenericTypeArguments[0] });
                        il.REmit(OpCodes.Newobj, ctor);
                    }
                }
            }
            else
            {
                il.NoErrorLoad(value);
            }
        }
        public static void NoErrorLoad(this ILGenerator il, object value)
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
                else if (instance != null)
                {
                    instance.Load();
                }
                else if (builder != null)
                {
                    il.EmitLoadBuidler(builder);
                }
                else if (enull != null)
                {

                    il.REmit(OpCodes.Ldnull);
                }
                else if (value is Type)
                {
                    il.REmit(OpCodes.Ldtoken, (Type)value);
                    il.REmit(OpCodes.Call, ClassCache.ClassHandle);
                }
                else if (type.IsClass && type != typeof(string))
                {
                    EModel model = EModel.CreateModelFromObject(value, type);
                    model.Load();
                }
                else if (type.IsEnum)
                {
                    il.REmit(OpCodes.Ldc_I4, (int)value);
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
                    il.LoadObject(value, type);
                    //il.CallNullableCtor(type);
                }
            }
            else
            {
                il.REmit(OpCodes.Ldnull);
            }
        }

        public static void LoadOne(this ILGenerator il, Type type)
        {
            if (type == typeof(int))
            {
                il.REmit(OpCodes.Ldc_I4_1);
            }
            else if (type == typeof(double))
            {
                il.REmit(OpCodes.Ldc_R8, (double)1.00);
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                il.REmit(OpCodes.Ldc_I4_1);
                il.REmit(OpCodes.Conv_I8);
            }
            else if (type == typeof(float))
            {
                il.REmit(OpCodes.Ldc_R4, (float)1.0);
            }
            else
            {
                il.REmit(OpCodes.Ldc_I4_1);
            }
        }

        public static void BoolInStack(this ILGenerator il, Type type)
        {
            if (type == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
        }
        internal static void EmitDefault(this ILGenerator il, Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Object:
                case TypeCode.DateTime:
                    if (type.IsValueType)
                    {
                        LocalBuilder lb = il.DeclareLocal(type);
                        il.REmit(OpCodes.Ldloca, lb);
                        il.REmit(OpCodes.Initobj, type);
                        il.REmit(OpCodes.Ldloc, lb);
                    }
                    else
                    {
                        il.REmit(OpCodes.Ldnull);
                    }
                    break;

                case TypeCode.Empty:
                case TypeCode.String:
                case TypeCode.DBNull:
                    il.REmit(OpCodes.Ldnull);
                    break;

                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    il.REmit(OpCodes.Ldc_I4_0);
                    break;

                case TypeCode.Int64:
                case TypeCode.UInt64:
                    il.REmit(OpCodes.Ldc_I4_0);
                    il.REmit(OpCodes.Conv_I8);
                    break;

                case TypeCode.Single:
                    il.REmit(OpCodes.Ldc_R4, default(Single));
                    break;

                case TypeCode.Double:
                    il.REmit(OpCodes.Ldc_R8, default(Double));
                    break;

                case TypeCode.Decimal:
                    il.REmit(OpCodes.Ldc_I4_0);
                    il.REmit(OpCodes.Newobj, typeof(Decimal).GetConstructor(new Type[] { typeof(int) }));
                    break;

                default:
                    break;
            }
        }
        public static void LoadBuilder(this ILGenerator il, LocalBuilder builder, bool LoadAddress = true)
        {
            Type type = builder.LocalType;
            if (type.IsByRef || !type.IsValueType || !LoadAddress)
            {
                il.EmitLoadBuidler(builder);
            }
            else
            {
                il.EmitLoadBuidlerAddress(builder);
            }
        }

        public static void LoadParameter(this ILGenerator il, Type type,int i,bool LoadAddress=true)
        {
            if (type != null && (type.IsByRef || type.IsValueType) && LoadAddress)
            {
                il.EmitLoadArgAddress(i);
            }
            else
            {
                il.EmitLoadArg(i);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        internal static void EmitNew(this ILGenerator il, ConstructorInfo ci)
        {

            if (ci.DeclaringType.ContainsGenericParameters)
            {
                return;
            }

            il.REmit(OpCodes.Newobj, ci);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
        internal static void EmitNew(this ILGenerator il, Type type, Type[] paramTypes)
        {
            ConstructorInfo ci = type.GetConstructor(paramTypes);
            il.EmitNew(ci);
        }

        internal static void EmitNull(this ILGenerator il)
        {
            il.REmit(OpCodes.Ldnull);
        }

        internal static void EmitString(this ILGenerator il, string value)
        {
            il.REmit(OpCodes.Ldstr, value);
        }

        internal static void EmitBoolean(this ILGenerator il, bool value)
        {
            if (value)
            {
                il.REmit(OpCodes.Ldc_I4_1);
            }
            else
            {
                il.REmit(OpCodes.Ldc_I4_0);
            }
            ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
        }

        internal static void EmitChar(this ILGenerator il, char value)
        {
            il.EmitInt(value);
            il.REmit(OpCodes.Conv_U2);
        }

        internal static void EmitByte(this ILGenerator il, byte value)
        {
            il.EmitInt(value);
            il.REmit(OpCodes.Conv_U1);
        }

        internal static void EmitSByte(this ILGenerator il, sbyte value)
        {
            il.EmitInt(value);
            il.REmit(OpCodes.Conv_I1);
        }

        internal static void EmitShort(this ILGenerator il, short value)
        {
            il.EmitInt(value);
            il.REmit(OpCodes.Conv_I2);
        }

        internal static void EmitUShort(this ILGenerator il, ushort value)
        {
            il.EmitInt(value);
            il.REmit(OpCodes.Conv_U2);
        }

        internal static void EmitInt(this ILGenerator il, int value)
        {
            OpCode c;
            switch (value)
            {
                case -1:
                    c = OpCodes.Ldc_I4_M1;
                    break;
                case 0:
                    c = OpCodes.Ldc_I4_0;
                    break;
                case 1:
                    c = OpCodes.Ldc_I4_1;
                    break;
                case 2:
                    c = OpCodes.Ldc_I4_2;
                    break;
                case 3:
                    c = OpCodes.Ldc_I4_3;
                    break;
                case 4:
                    c = OpCodes.Ldc_I4_4;
                    break;
                case 5:
                    c = OpCodes.Ldc_I4_5;
                    break;
                case 6:
                    c = OpCodes.Ldc_I4_6;
                    break;
                case 7:
                    c = OpCodes.Ldc_I4_7;
                    break;
                case 8:
                    c = OpCodes.Ldc_I4_8;
                    break;
                default:
                    if (value >= -128 && value <= 127)
                    {
                        il.REmit(OpCodes.Ldc_I4_S, (sbyte)value);
                    }
                    else
                    {
                        il.REmit(OpCodes.Ldc_I4, value);
                    }
                    return;
            }
            il.REmit(c);
        }

        internal static void EmitUInt(this ILGenerator il, uint value)
        {
            il.EmitInt((int)value);
            il.REmit(OpCodes.Conv_U4);
        }

        internal static void EmitLong(this ILGenerator il, long value)
        {
            if (int.MinValue<=value && value <= int.MaxValue)
            {
                il.EmitInt((int)value);
            }
            else
            {
                il.REmit(OpCodes.Ldc_I8, value);
            }
            il.REmit(OpCodes.Conv_I8);
        }

        internal static void EmitULong(this ILGenerator il, ulong value)
        {
            il.REmit(OpCodes.Ldc_I8, (long)value);
            il.REmit(OpCodes.Conv_U8);
        }

        internal static void EmitDouble(this ILGenerator il, double value)
        {
            il.REmit(OpCodes.Ldc_R8, value);
        }

        internal static void EmitSingle(this ILGenerator il, float value)
        {
            il.REmit(OpCodes.Ldc_R4, value);
        }
        internal static void EmitDecimal(this ILGenerator il, decimal value)
        {
            if (Decimal.Truncate(value) == value)
            {
                if (Int32.MinValue <= value && value <= Int32.MaxValue)
                {
                    int intValue = Decimal.ToInt32(value);
                    il.EmitInt(intValue);
                    il.EmitNew(typeof(Decimal).GetConstructor(new Type[] { typeof(int) }));
                }
                else if (Int64.MinValue <= value && value <= Int64.MaxValue)
                {
                    long longValue = Decimal.ToInt64(value);
                    il.EmitLong(longValue);
                    il.EmitNew(typeof(Decimal).GetConstructor(new Type[] { typeof(long) }));
                }
                else
                {
                    il.EmitDecimalBits(value);
                }
            }
            else
            {
                il.EmitDecimalBits(value);
            }
        }

        private static void EmitDecimalBits(this ILGenerator il, decimal value)
        {
            int[] bits = Decimal.GetBits(value);
            il.EmitInt(bits[0]);
            il.EmitInt(bits[1]);
            il.EmitInt(bits[2]);
            il.EmitBoolean((bits[3] & 0x80000000) != 0);
            il.EmitByte((byte)(bits[3] >> 16));
            il.EmitNew(typeof(decimal).GetConstructor(new Type[] { typeof(int), typeof(int), typeof(int), typeof(bool), typeof(byte) }));
        }

        internal static bool LoadObject(this ILGenerator il, object value, Type type)
        {
            if (type.IsEnum)
            {
                value = (int)value;
                type = typeof(int);
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    il.EmitBoolean((bool)value);
                    return true;
                case TypeCode.SByte:
                    il.EmitSByte((sbyte)value);
                    return true;
                case TypeCode.Int16:
                    il.EmitShort((short)value);
                    return true;
                case TypeCode.Int32:
                    il.EmitInt((int)value);
                    return true;
                case TypeCode.Int64:
                    il.EmitLong((long)value);
                    return true;
                case TypeCode.Single:
                    il.EmitSingle((float)value);
                    return true;
                case TypeCode.Double:
                    il.EmitDouble((double)value);
                    return true;
                case TypeCode.Char:
                    il.EmitChar((char)value);
                    return true;
                case TypeCode.Byte:
                    il.EmitByte((byte)value);
                    return true;
                case TypeCode.UInt16:
                    il.EmitUShort((ushort)value);
                    return true;
                case TypeCode.UInt32:
                    il.EmitUInt((uint)value);
                    return true;
                case TypeCode.UInt64:
                    il.EmitULong((ulong)value);
                    return true;
                case TypeCode.Decimal:
                    il.EmitDecimal((decimal)value);
                    return true;
                case TypeCode.String:
                    il.EmitString((string)value);
                    return true;
                default:
                    return false;
            }
        }
        #endregion
    }
}
