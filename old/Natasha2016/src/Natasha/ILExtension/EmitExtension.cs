using Natasha;


namespace System.Reflection.Emit
{
    public static class EmitExtension
    {
        //获取链式调用model
        public static EModel GetLink(this ILGenerator il,EModel model, Type type)
        {
            EModel returnModel = EModel.CreateModelFromAction(null, type);
            returnModel._currentMemeberName = model._currentMemeberName;
            returnModel._currentPrivateType = model._currentPrivateType;
            model._currentPrivateType = null;
            returnModel.CompareType = model.CompareType;

            returnModel.PrewCallOption = model.TempOption;
            returnModel.PreCallType = model.PreCallType;
            return returnModel;
        }

        internal static void EmitLoadArg(this ILGenerator il, int index) 
        {
            switch (index)
            {
                case 0:
                    il.REmit(OpCodes.Ldarg_0);
                    break;
                case 1:
                    il.REmit(OpCodes.Ldarg_1);
                    break;
                case 2:
                    il.REmit(OpCodes.Ldarg_2);
                    break;
                case 3:
                    il.REmit(OpCodes.Ldarg_3);
                    break;
                default:
                    if (index <= Byte.MaxValue)
                    {
                        il.REmit(OpCodes.Ldarg_S, (byte)index);
                    }
                    else
                    {
                        il.REmit(OpCodes.Ldarg, index);
                    }
                    break;
            }
        }

        internal static void EmitLoadArgAddress(this ILGenerator il, int index)
        {
            if (index <= Byte.MaxValue)
            {
                il.REmit(OpCodes.Ldarga_S, (byte)index);
            }
            else
            {
                il.REmit(OpCodes.Ldarga, index);
            }
        }

        internal static void EmitStoreArg(this ILGenerator il, int index)
        {

            if (index <= Byte.MaxValue)
            {
                il.REmit(OpCodes.Starg_S, (byte)index);
            }
            else
            {
                il.REmit(OpCodes.Starg, index);
            }
        }

        internal static void EmitLoadBuidler(this ILGenerator il, LocalBuilder builder)
        {
            int index = builder.LocalIndex;
            switch (index)
            {
                case 0:
                    il.REmit(OpCodes.Ldloc_0);
                    break;
                case 1:
                    il.REmit(OpCodes.Ldloc_1);
                    break;
                case 2:
                    il.REmit(OpCodes.Ldloc_2);
                    break;
                case 3:
                    il.REmit(OpCodes.Ldloc_3);
                    break;
                default:
                    if (index <= Byte.MaxValue)
                    {
                        il.REmit(OpCodes.Ldloc_S, (byte)index);
                    }
                    else
                    {
                        il.REmit(OpCodes.Ldloc, index);
                    }
                    break;
            }
        }

        internal static void EmitLoadBuidlerAddress(this ILGenerator il, LocalBuilder builder)
        {
            int index = builder.LocalIndex;
            if (index <= Byte.MaxValue)
            {
                il.REmit(OpCodes.Ldloca_S, (byte)index);
            }
            else
            {
                il.REmit(OpCodes.Ldloca, index);
            }
        }

        internal static void EmitStoreBuilder(this ILGenerator il, LocalBuilder builder)
        {
            int index = builder.LocalIndex;

            if (index <= Byte.MaxValue)
            {
                switch (index)
                {
                    case 0:
                        il.REmit(OpCodes.Stloc_0);
                        break;
                    case 1:
                        il.REmit(OpCodes.Stloc_1);
                        break;
                    case 2:
                        il.REmit(OpCodes.Stloc_2);
                        break;
                    case 3:
                        il.REmit(OpCodes.Stloc_3);
                        break;
                    default:
                        il.REmit(OpCodes.Stloc_S, (byte)index);
                        break;
                }
            }
            else
            {
                il.REmit(OpCodes.Stloc, index);
            }
        }
        /// <summary>
        /// Emits a Ldind* instruction for the appropriate type
        /// </summary>
        internal static void EmitLoadValueIndirect(this ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                if (type == typeof(int))
                {
                    il.REmit(OpCodes.Ldind_I4);
                }
                else if (type == typeof(uint))
                {
                    il.REmit(OpCodes.Ldind_U4);
                }
                else if (type == typeof(short))
                {
                    il.REmit(OpCodes.Ldind_I2);
                }
                else if (type == typeof(ushort))
                {
                    il.REmit(OpCodes.Ldind_U2);
                }
                else if (type == typeof(long) || type == typeof(ulong))
                {
                    il.REmit(OpCodes.Ldind_I8);
                }
                else if (type == typeof(char))
                {
                    il.REmit(OpCodes.Ldind_I2);
                }
                else if (type == typeof(bool))
                {
                    il.REmit(OpCodes.Ldind_I1);
                }
                else if (type == typeof(float))
                {
                    il.REmit(OpCodes.Ldind_R4);
                }
                else if (type == typeof(double))
                {
                    il.REmit(OpCodes.Ldind_R8);
                }
                else
                {
                    il.REmit(OpCodes.Ldobj, type);
                }
            }
            else
            {
                il.REmit(OpCodes.Ldind_Ref);
            }
        }
        /// <summary>
        /// Emits a Stind* instruction for the appropriate type.
        /// </summary>
        internal static void EmitStoreValueIndirect(this ILGenerator il, Type type)
        {

            if (type.IsValueType)
            {
                if (type == typeof(int))
                {
                    il.REmit(OpCodes.Stind_I4);
                }
                else if (type == typeof(short))
                {
                    il.REmit(OpCodes.Stind_I2);
                }
                else if (type == typeof(long) || type == typeof(ulong))
                {
                    il.REmit(OpCodes.Stind_I8);
                }
                else if (type == typeof(char))
                {
                    il.REmit(OpCodes.Stind_I2);
                }
                else if (type == typeof(bool))
                {
                    il.REmit(OpCodes.Stind_I1);
                }
                else if (type == typeof(float))
                {
                    il.REmit(OpCodes.Stind_R4);
                }
                else if (type == typeof(double))
                {
                    il.REmit(OpCodes.Stind_R8);
                }
                else
                {
                    il.REmit(OpCodes.Stobj, type);
                }
            }
            else
            {
                il.REmit(OpCodes.Stind_Ref);
            }
        }

        // Emits the Ldelem* instruction for the appropriate type

        internal static void LoadElement(this ILGenerator il, Type type)
        {

            if (!type.IsValueType)
            {
                il.REmit(OpCodes.Ldelem_Ref);
            }
            else if (type.IsEnum)
            {
                il.REmit(OpCodes.Ldelem, type);
            }
            else
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                    case TypeCode.SByte:
                        il.REmit(OpCodes.Ldelem_I1);
                        break;
                    case TypeCode.Byte:
                        il.REmit(OpCodes.Ldelem_U1);
                        break;
                    case TypeCode.Int16:
                        il.REmit(OpCodes.Ldelem_I2);
                        break;
                    case TypeCode.Char:
                    case TypeCode.UInt16:
                        il.REmit(OpCodes.Ldelem_U2);
                        break;
                    case TypeCode.Int32:
                        il.REmit(OpCodes.Ldelem_I4);
                        break;
                    case TypeCode.UInt32:
                        il.REmit(OpCodes.Ldelem_U4);
                        break;
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        il.REmit(OpCodes.Ldelem_I8);
                        break;
                    case TypeCode.Single:
                        il.REmit(OpCodes.Ldelem_R4);
                        break;
                    case TypeCode.Double:
                        il.REmit(OpCodes.Ldelem_R8);
                        break;
                    default:
                        il.REmit(OpCodes.Ldelem, type);
                        break;
                }
            }
        }

        /// <summary>
        /// Emits a Stelem* instruction for the appropriate type.
        /// </summary>
        internal static void StoreElement(this ILGenerator il, Type type)
        {
            if (type.IsEnum)
            {
                il.REmit(OpCodes.Stelem, type);
                return;
            }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    il.REmit(OpCodes.Stelem_I1);
                    break;
                case TypeCode.Char:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    il.REmit(OpCodes.Stelem_I2);
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    il.REmit(OpCodes.Stelem_I4);
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    il.REmit(OpCodes.Stelem_I8);
                    break;
                case TypeCode.Single:
                    il.REmit(OpCodes.Stelem_R4);
                    break;
                case TypeCode.Double:
                    il.REmit(OpCodes.Stelem_R8);
                    break;
                default:
                    if (type.IsValueType)
                    {
                        il.REmit(OpCodes.Stelem, type);
                    }
                    else
                    {
                        il.REmit(OpCodes.Stelem_Ref);
                    }
                    break;
            }
        }

        // Emits the Ldelem* instruction for the appropriate type

        internal static void EmitLoadElement(this ILGenerator il, Type type)
        {
            if (!type.IsValueType)
            {
                il.Emit(OpCodes.Ldelem_Ref);
            }
            else if (type.IsEnum)
            {
                il.Emit(OpCodes.Ldelem, type);
            }
            else
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                    case TypeCode.SByte:
                        il.Emit(OpCodes.Ldelem_I1);
                        break;
                    case TypeCode.Byte:
                        il.Emit(OpCodes.Ldelem_U1);
                        break;
                    case TypeCode.Int16:
                        il.Emit(OpCodes.Ldelem_I2);
                        break;
                    case TypeCode.Char:
                    case TypeCode.UInt16:
                        il.Emit(OpCodes.Ldelem_U2);
                        break;
                    case TypeCode.Int32:
                        il.Emit(OpCodes.Ldelem_I4);
                        break;
                    case TypeCode.UInt32:
                        il.Emit(OpCodes.Ldelem_U4);
                        break;
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        il.Emit(OpCodes.Ldelem_I8);
                        break;
                    case TypeCode.Single:
                        il.Emit(OpCodes.Ldelem_R4);
                        break;
                    case TypeCode.Double:
                        il.Emit(OpCodes.Ldelem_R8);
                        break;
                    default:
                        il.Emit(OpCodes.Ldelem, type);
                        break;
                }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private static void EmitNumericConversion(this ILGenerator il, Type typeFrom, Type typeTo, bool isChecked)
        {
            bool isFromUnsigned = TypeUtils.IsUnsigned(typeFrom);
            bool isFromFloatingPoint = TypeUtils.IsFloatingPoint(typeFrom);
            if (typeTo == typeof(Single))
            {
                if (isFromUnsigned)
                    il.REmit(OpCodes.Conv_R_Un);
                il.REmit(OpCodes.Conv_R4);
            }
            else if (typeTo == typeof(Double))
            {
                if (isFromUnsigned)
                    il.REmit(OpCodes.Conv_R_Un);
                il.REmit(OpCodes.Conv_R8);
            }
            else
            {
                TypeCode tc = Type.GetTypeCode(typeTo);
                if (isChecked)
                {
                    // Overflow checking needs to know if the source value on the IL stack is unsigned or not.
                    if (isFromUnsigned)
                    {
                        switch (tc)
                        {
                            case TypeCode.SByte:
                                il.REmit(OpCodes.Conv_Ovf_I1_Un);
                                break;
                            case TypeCode.Int16:
                                il.REmit(OpCodes.Conv_Ovf_I2_Un);
                                break;
                            case TypeCode.Int32:
                                il.REmit(OpCodes.Conv_Ovf_I4_Un);
                                break;
                            case TypeCode.Int64:
                                il.REmit(OpCodes.Conv_Ovf_I8_Un);
                                break;
                            case TypeCode.Byte:
                                il.REmit(OpCodes.Conv_Ovf_U1_Un);
                                break;
                            case TypeCode.UInt16:
                            case TypeCode.Char:
                                il.REmit(OpCodes.Conv_Ovf_U2_Un);
                                break;
                            case TypeCode.UInt32:
                                il.REmit(OpCodes.Conv_Ovf_U4_Un);
                                break;
                            case TypeCode.UInt64:
                                il.REmit(OpCodes.Conv_Ovf_U8_Un);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (tc)
                        {
                            case TypeCode.SByte:
                                il.REmit(OpCodes.Conv_Ovf_I1);
                                break;
                            case TypeCode.Int16:
                                il.REmit(OpCodes.Conv_Ovf_I2);
                                break;
                            case TypeCode.Int32:
                                il.REmit(OpCodes.Conv_Ovf_I4);
                                break;
                            case TypeCode.Int64:
                                il.REmit(OpCodes.Conv_Ovf_I8);
                                break;
                            case TypeCode.Byte:
                                il.REmit(OpCodes.Conv_Ovf_U1);
                                break;
                            case TypeCode.UInt16:
                            case TypeCode.Char:
                                il.REmit(OpCodes.Conv_Ovf_U2);
                                break;
                            case TypeCode.UInt32:
                                il.REmit(OpCodes.Conv_Ovf_U4);
                                break;
                            case TypeCode.UInt64:
                                il.REmit(OpCodes.Conv_Ovf_U8);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.SByte:
                            il.REmit(OpCodes.Conv_I1);
                            break;
                        case TypeCode.Byte:
                            il.REmit(OpCodes.Conv_U1);
                            break;
                        case TypeCode.Int16:
                            il.REmit(OpCodes.Conv_I2);
                            break;
                        case TypeCode.UInt16:
                        case TypeCode.Char:
                            il.REmit(OpCodes.Conv_U2);
                            break;
                        case TypeCode.Int32:
                            il.REmit(OpCodes.Conv_I4);
                            break;
                        case TypeCode.UInt32:
                            il.REmit(OpCodes.Conv_U4);
                            break;
                        case TypeCode.Int64:
                            if (isFromUnsigned)
                            {
                                il.REmit(OpCodes.Conv_U8);
                            }
                            else
                            {
                                il.REmit(OpCodes.Conv_I8);
                            }
                            break;
                        case TypeCode.UInt64:
                            if (isFromUnsigned || isFromFloatingPoint)
                            {
                                il.REmit(OpCodes.Conv_U8);
                            }
                            else
                            {
                                il.REmit(OpCodes.Conv_I8);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }


        
    }
}
