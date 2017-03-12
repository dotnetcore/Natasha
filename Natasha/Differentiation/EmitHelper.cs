using Natasha.Cache;
using Natasha.Core;
using Natasha.Core.Base;
using Natasha.Debug;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Utils
{
    public class EmitHelper
    {
        #region Code映射
        public static OpCode GetLoadCode(Type type)
        {
            if (type.IsByRef || !type.IsValueType)
            {
                DebugHelper.Write("Ldloc_S\t\t");
                return OpCodes.Ldloc_S;

            }
            else
            {
                DebugHelper.Write("Ldloca_S\t");
                return OpCodes.Ldloca_S;
            }
        }
        public static OpCode GetArgsCode(Type type, int i = 4)
        {
            if (type != null && (type.IsByRef || !type.IsValueType))
            {
                DebugHelper.Write("Ldarga_S\t");
                return OpCodes.Ldarga_S;
            }
            else
            {
                if (i == 0)
                {
                    DebugHelper.WriteLine("Ldarg_0");
                    return OpCodes.Ldarg_0;
                }
                else if (i == 1)
                {
                    DebugHelper.WriteLine("Ldarg_1");
                    return OpCodes.Ldarg_1;
                }
                else if (i == 2)
                {
                    DebugHelper.WriteLine("Ldarg_2");
                    return OpCodes.Ldarg_2;
                }
                else if (i == 3)
                {
                    DebugHelper.WriteLine("Ldarg_3");
                    return OpCodes.Ldarg_3;
                }
                else
                {
                    DebugHelper.Write("Ldarg_S\t");
                    return OpCodes.Ldarg_S;
                }
            }
        }
        public static OpCode GetCallCode(Type type, FieldInfo info)
        {
            if (type.IsValueType || info.IsStatic)
            {
                DebugHelper.Write("Call\t");
                return OpCodes.Call;
            }
            else
            {
                DebugHelper.Write("Callvirt\t");
                return OpCodes.Callvirt;
            }
        }

        #endregion
        public static void CallMethod(Type type, MethodInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (type.IsValueType || info.IsStatic)
            {
                DebugHelper.WriteLine("Call", info.Name);
                il.Emit(OpCodes.Call, info);
            }
            else
            {
                DebugHelper.WriteLine("Callvirt", info.Name);
                il.Emit(OpCodes.Callvirt, info);
            }
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

        #region 属性操作
        public static void LoadPublicProperty(Action thisInStack, PropertyInfo info)
        {
            MethodInfo method = info.GetGetMethod(true);
            //静态属性
            if (!method.IsStatic)
            {
                thisInStack();
            }
            CallMethod(info.DeclaringType, method);
        }
        public static void LoadPrivateProperty(Action thisInStack, PropertyInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            //加载GetValue的第一个参数 即PropertyInfo
            il.Emit(OpCodes.Ldtoken, info.DeclaringType);
            il.Emit(OpCodes.Call, ClassCache.ClassHandle);
            il.Emit(OpCodes.Ldstr, info.Name);
            il.Emit(OpCodes.Ldc_I4_S, 44);
            il.Emit(OpCodes.Callvirt, ClassCache.PropertyInfoGetter);

            DebugHelper.WriteLine("Ldtoken", info.DeclaringType.Name);
            DebugHelper.WriteLine("Call", ClassCache.ClassHandle.Name);
            DebugHelper.WriteLine("Ldstr", info.Name);
            DebugHelper.WriteLine("Ldc_I4_S", 44);
            DebugHelper.WriteLine("Callvirt", ClassCache.PropertyInfoGetter.Name);

            //加载GetValue的第二个参数
            if (thisInStack != null)
            {
                thisInStack();
            };
            Packet(info.DeclaringType);

            //调用GetValue
            il.Emit(OpCodes.Callvirt, ClassCache.PropertyValueGetter);
            DebugHelper.WriteLine("Callvirt", ClassCache.PropertyValueGetter.Name);
            UnPacket(info.PropertyType);



        }
        public static void SetPrivateProperty(Action thisInStack, PropertyInfo info, object value)
        {
            ILGenerator il = ThreadCache.GetIL();
            //加载SetValue的第一个参数 即PropertyInfo
            il.Emit(OpCodes.Ldtoken, info.DeclaringType);
            il.Emit(OpCodes.Call, ClassCache.ClassHandle);
            il.Emit(OpCodes.Ldstr, info.Name);
            il.Emit(OpCodes.Ldc_I4_S, 44);
            il.Emit(OpCodes.Call, ClassCache.PropertyInfoGetter);

            DebugHelper.WriteLine("Ldtoken", info.DeclaringType.Name);
            DebugHelper.WriteLine("Call", ClassCache.ClassHandle.Name);
            DebugHelper.WriteLine("Ldstr", info.Name);
            DebugHelper.WriteLine("Ldc_I4_S", 44);
            DebugHelper.WriteLine("Call", ClassCache.PropertyInfoGetter.Name);

            //加载SetValue的第二个参数
            if (thisInStack != null)
            {
                thisInStack();
            }
            Packet(info.DeclaringType);


            //加载SetValue的第三个参数
            EmitHelper.NoErrorLoad(value, il);
            if (value != null)
            {
                Packet(value.GetType());
            }

            //调用SetValue
            il.Emit(OpCodes.Callvirt, ClassCache.PropertyValueSetter);
            DebugHelper.WriteLine("Callvirt", ClassCache.PropertyValueSetter.Name);


        }
        public static void SetPublicProperty(Action thisInStack, PropertyInfo info, object value)
        {
            MethodInfo method = info.GetSetMethod(true);
            //静态属性
            if (!method.IsStatic)
            {
                if (thisInStack != null)
                {
                    thisInStack();
                }
            }
            EmitHelper.NoErrorLoad(value, ThreadCache.GetIL());
            CallMethod(info.DeclaringType, method);
        }
        #endregion


        #region 字段操作    
        //加载私有字段
        public static void LoadPrivateField(Action thisInStack, FieldInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            //加载GetValue的第一个参数 即FieldInfo
            il.Emit(OpCodes.Ldtoken, info.DeclaringType);
            il.Emit(OpCodes.Call, ClassCache.ClassHandle);
            il.Emit(OpCodes.Ldstr, info.Name);
            il.Emit(OpCodes.Ldc_I4_S, 44);
            il.Emit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

            DebugHelper.WriteLine("Ldtoken", info.DeclaringType.Name);
            DebugHelper.WriteLine("Call", ClassCache.ClassHandle.Name);
            DebugHelper.WriteLine("Ldstr", info.Name);
            DebugHelper.WriteLine("Ldc_I4_S", 44);
            DebugHelper.WriteLine("Callvirt", ClassCache.FieldInfoGetter.Name);

            //加载GetValue的第二个参数
            thisInStack();
            Packet(info.DeclaringType);

            //调用GetValue
            il.Emit(OpCodes.Callvirt, ClassCache.FieldValueGetter);
            DebugHelper.WriteLine("Callvirt", ClassCache.FieldValueGetter.Name);
            //拆箱
            UnPacket(info.FieldType);



        }
        //加载公有字段
        public static void LoadPublicField(Action thisInStack, FieldInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (info.IsStatic)
            {
                il.Emit(OpCodes.Ldsfld, info);
                DebugHelper.WriteLine("Ldsfld", info.Name);
            }
            else
            {
                if (thisInStack != null)
                {
                    thisInStack();
                }
                il.Emit(OpCodes.Ldfld, info);
                DebugHelper.WriteLine("Ldfld", info.Name);
            }
        }
        //设置公有结构体字段
        public static void LoadPublicStructField(Action thisInStack, FieldInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            Type type = info.FieldType;
            if (info.IsStatic)
            {
                il.Emit(OpCodes.Ldsflda, info);
                DebugHelper.WriteLine("Ldsflda", info.Name);
            }
            else
            {
                if (thisInStack != null)
                {
                    thisInStack();
                }
                il.Emit(OpCodes.Ldflda, info);
                DebugHelper.WriteLine("Ldflda", info.Name);
            }
        }
        //设置私有字段
        public static void SetPrivateField(Action thisInStack, FieldInfo info, object value)
        {
            ILGenerator il = ThreadCache.GetIL();
            //加载SetValue的第一个参数 即FieldInfo
            il.Emit(OpCodes.Ldtoken, info.DeclaringType);
            il.Emit(OpCodes.Call, ClassCache.ClassHandle);
            il.Emit(OpCodes.Ldstr, info.Name);
            il.Emit(OpCodes.Ldc_I4_S, 44);
            il.Emit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

            DebugHelper.WriteLine("Ldtoken", info.DeclaringType.Name);
            DebugHelper.WriteLine("Call", ClassCache.ClassHandle.Name);
            DebugHelper.WriteLine("Ldstr", info.Name);
            DebugHelper.WriteLine("Ldc_I4_S", 44);
            DebugHelper.WriteLine("Callvirt", ClassCache.FieldInfoGetter.Name);

            //加载SetValue的第二个参数
            if (thisInStack!=null)
            {
                thisInStack();
            }
            Packet(info.DeclaringType);

            //加载SetValue的第三个参数
            EmitHelper.NoErrorLoad(value, il);
            if (value != null)
            {
                Packet(value.GetType());
            }
            //调用SetValue
            il.Emit(OpCodes.Callvirt, ClassCache.FieldValueSetter);
            DebugHelper.WriteLine("Callvirt", ClassCache.FieldValueSetter.Name);


        }

        //设置公有字段
        public static void SetPublicField(Action thisInStack, FieldInfo info, object value)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (info.IsStatic)
            {
                //填充静态字段
                EmitHelper.NoErrorLoad(value, il);
                il.Emit(OpCodes.Stsfld, info);
                DebugHelper.WriteLine("Stsfld", info.Name);
            }
            else
            {
                if (thisInStack!=null)
                {
                    thisInStack();
                }
                EmitHelper.NoErrorLoad(value, il);
                il.Emit(OpCodes.Stfld, info);
                DebugHelper.WriteLine("Stfld", info.Name);
            }
        }
        #endregion


        //返回
        public static void Return()
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ret);
            DebugHelper.WriteLine("Ret");
        }

        //压栈并返回
        public static void ReturnValue(object value)
        {
            NoErrorLoad(value,ThreadCache.GetIL());
            Return();
        }

        //初始化结构体
        public static void InitObject(Type type)
        {
            ThreadCache.GetIL().Emit(OpCodes.Initobj, type);
            DebugHelper.WriteLine("Initobj", type.ToString());
        }

        //获取链式调用model
        public static EModel GetLink(Type type)
        {
            return EModel.CreateModelFromAction(null, type);
        }


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



        #region 数据入栈
        //各种数据给我就能入栈
        public static void NoErrorLoad(object value, ILGenerator il)
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
                    il.Emit(OpCodes.Ldloc_S, builder.LocalIndex);
                    DebugHelper.WriteLine("Ldloc_S", builder.LocalIndex.ToString());
                }
                else if (enull != null)
                {
                    il.Emit(OpCodes.Ldnull);
                    DebugHelper.WriteLine("Ldnull");
                }
                else if (value is Type)
                {
                    il.Emit(OpCodes.Ldtoken, (Type)value);
                    il.Emit(OpCodes.Call, ClassCache.ClassHandle);

                    DebugHelper.WriteLine("Ldtoken", ((Type)value).Name);
                    DebugHelper.WriteLine("Call", ClassCache.ClassHandle.Name);
                }
                else if (type.IsClass && type != typeof(string))
                {
                    EModel model = EModel.CreateModelFromObject(value, type);
                    model.Load();
                }
                else if (type.IsEnum)
                {
                    il.Emit(OpCodes.Ldc_I4, (int)value);
                    DebugHelper.WriteLine("Ldc_I4", (int)value);
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
                    LoadObject(value);
                }
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
                DebugHelper.WriteLine("Ldnull");
            }
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
                    if (result < 255)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, result);
                        DebugHelper.WriteLine("Ldc_I4_S", result);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, result);
                        DebugHelper.WriteLine("Ldc_I4", result);
                    }
                }
            }
            else if (EType == typeof(string))
            {
                string result = value.ToString();
                il.Emit(OpCodes.Ldstr, result);
                DebugHelper.WriteLine("Ldstr", result);
            }
            else if (EType == typeof(double))
            {
                double result;
                if (double.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_R8, result);
                    DebugHelper.WriteLine("Ldc_R8", result.ToString());
                }
            }
            else if (EType == typeof(float))
            {
                float result;
                if (float.TryParse(value.ToString(), out result))
                {
                    il.Emit(OpCodes.Ldc_R4, result);
                    DebugHelper.WriteLine("Ldc_R4", result.ToString());
                }
            }
            else if (EType == typeof(bool))
            {
                if ((bool)value)
                {
                    il.Emit(OpCodes.Ldc_I4_1);
                    DebugHelper.WriteLine("Ldc_I4_1");
                }
                else
                {
                    il.Emit(OpCodes.Ldc_I4_0);
                    DebugHelper.WriteLine("Ldc_I4_0");
                }
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            else if (EType == typeof(long) || EType == typeof(ulong))
            {
                long result;
                if (long.TryParse(value.ToString(), out result))
                {
                    if (result < 255)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, result);
                        DebugHelper.WriteLine("Ldc_I4_S", result.ToString());
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, result);
                        DebugHelper.WriteLine("Ldc_I4", result.ToString());
                    }
                    il.Emit(OpCodes.Conv_I8);
                    DebugHelper.WriteLine("Conv_I8");
                }
            }
            else
            {
                int result;
                if (int.TryParse(value.ToString(), out result))
                {
                    if (result < 255)
                    {
                        il.Emit(OpCodes.Ldc_I4_S, result);
                        DebugHelper.WriteLine("Ldc_I4_S",result);
                    }
                    else
                    {
                        il.Emit(OpCodes.Ldc_I4, result);
                        DebugHelper.WriteLine("Ldc_I4", result);
                    }
                }
            }
        }


        //自增量入栈
        public static void LoadSelfObject(Type type)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (type == typeof(int))
            {
                il.Emit(OpCodes.Ldc_I4_1);
                DebugHelper.WriteLine("Ldc_I4_1");
            }
            else if (type == typeof(double))
            {
                il.Emit(OpCodes.Ldc_R8, 1.00);
                DebugHelper.WriteLine("Ldc_R8 1.00");
            }
            else if (type == typeof(long) || type == typeof(ulong))
            {
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Conv_I8);
                DebugHelper.WriteLine("Ldc_I4_1");
                DebugHelper.WriteLine("Conv_I8");
            }
            else if (type == typeof(float))
            {
                il.Emit(OpCodes.Ldc_R4, 1.0);
                DebugHelper.WriteLine("Ldc_R4 1.0");
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4_1);
                DebugHelper.WriteLine("Ldc_I4_1");
            }
        }
        #endregion

    }
}
