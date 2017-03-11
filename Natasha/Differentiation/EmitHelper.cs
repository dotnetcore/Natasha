using Natasha.Cache;
using Natasha.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Natasha.Utils
{
    public class EmitHelper
    {
        public static OpCode GetLoadCode(Type type)
        {
            if (type.IsByRef || !type.IsValueType )
            {
                DebugHelper.Write("OpCodes.Ldloc_S ");
                return OpCodes.Ldloc_S;
                
            }
            else
            {
                DebugHelper.Write("OpCodes.Ldloca_S ");
                return OpCodes.Ldloca_S;
            }
        }
        public static OpCode GetArgsCode(Type type, int i = 4)
        {
            if (type.IsByRef || !type.IsValueType)
            {
                DebugHelper.Write("OpCodes.Ldarga_S ");
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
                    DebugHelper.Write("OpCodes.Ldarg_S ");
                    return OpCodes.Ldarg_S;
                }
            }
        }
        public static OpCode GetCallCode(Type type,FieldInfo info)
        {
            if (type.IsValueType || info.IsStatic)
            {
                DebugHelper.Write("OpCodes.Call ");
                return OpCodes.Call;
            }
            else
            {
                DebugHelper.Write("OpCodes.Callvirt ");
                return OpCodes.Callvirt;
            }
        }
        public static void CallMethod(Type type, MethodInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (type.IsValueType || info.IsStatic)
            {
                DebugHelper.WriteLine("Call "+info.Name);
                il.Emit(OpCodes.Call,info);
            }
            else
            {
                DebugHelper.WriteLine("Callvirt " + info.Name);
                il.Emit(OpCodes.Callvirt, info);
            }
        }

        public static void UnPacket(Type type)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (type.IsClass && type != typeof(string) && type != typeof(object))
            {
                il.Emit(OpCodes.Castclass, type);
                DebugHelper.WriteLine("Castclass " + type.Name);
            }
            else
            {
                il.Emit(OpCodes.Unbox_Any, type);
                DebugHelper.WriteLine("Unbox_Any " + type.Name);
            }
        }
        public static void Packet(Type type)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Box, type);
                DebugHelper.WriteLine("Box " + type.Name);
            }
        }

        public static void LoadPublicField(Action action, FieldInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (info.IsStatic)
            {
                il.Emit(OpCodes.Ldsfld, info);
                DebugHelper.WriteLine("Ldsfld " + info.Name);
            }
            else
            {
                if (action!=null)
                {
                    action();
                }
                il.Emit(OpCodes.Ldfld, info);
                DebugHelper.WriteLine("Ldfld " + info.Name);
            }
        }

        public static void LoadPublicStructField(Action action, FieldInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            Type type = info.FieldType;
            if (info.IsStatic)
            {
               il.Emit(OpCodes.Ldsflda, info);
                DebugHelper.WriteLine("Ldsflda " + info.Name);
            }
            else
            {
                if (action!=null)
                {
                    action();
                }
                il.Emit(OpCodes.Ldflda, info);
                DebugHelper.WriteLine("Ldflda " + info.Name);
            }
        }

        public static void LoadPrivateField(Action action, FieldInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            //加载GetValue的第一个参数 即FieldInfo
            il.Emit(OpCodes.Ldtoken, info.DeclaringType);
            il.Emit(OpCodes.Call, ClassCache.ClassHandle);
            il.Emit(OpCodes.Ldstr, info.Name);
            il.Emit(OpCodes.Ldc_I4_S, 44);
            il.Emit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

            DebugHelper.WriteLine("Ldtoken " + info.DeclaringType.Name);
            DebugHelper.WriteLine("Call " + ClassCache.ClassHandle.Name);
            DebugHelper.WriteLine("Ldstr " + info.Name);
            DebugHelper.WriteLine("Ldc_I4_S " + 44);
            DebugHelper.WriteLine("Callvirt " + ClassCache.FieldInfoGetter.Name);

            //加载GetValue的第二个参数
            action();
            Packet(info.DeclaringType);

            //调用GetValue
            il.Emit(OpCodes.Callvirt, ClassCache.FieldValueGetter);
            DebugHelper.WriteLine("Callvirt " + ClassCache.FieldValueGetter.Name);
            //拆箱
            UnPacket(info.FieldType);

            
           
        }

        public static void LoadPublicProperty(Action action, PropertyInfo info)
        {
            MethodInfo method = info.GetGetMethod(true);
            //静态属性
            if (!method.IsStatic)
            {
                action();
            }
            CallMethod(info.DeclaringType, method);
        }
        public static void LoadPrivateProperty(Action action, PropertyInfo info)
        {
            ILGenerator il = ThreadCache.GetIL();
            //加载GetValue的第一个参数 即PropertyInfo
            il.Emit(OpCodes.Ldtoken, info.DeclaringType);
            il.Emit(OpCodes.Call, ClassCache.ClassHandle);
            il.Emit(OpCodes.Ldstr, info.Name);
            il.Emit(OpCodes.Ldc_I4_S, 44);
            il.Emit(OpCodes.Callvirt, ClassCache.PropertyInfoGetter);

            DebugHelper.WriteLine("Ldtoken " + info.DeclaringType.Name);
            DebugHelper.WriteLine("Call " + ClassCache.ClassHandle.Name);
            DebugHelper.WriteLine("Ldstr " + info.Name);
            DebugHelper.WriteLine("Ldc_I4_S " + 44);
            DebugHelper.WriteLine("Callvirt " + ClassCache.PropertyInfoGetter.Name);

            //加载GetValue的第二个参数
            if (action!=null)
            {
                action();
            };
            Packet(info.DeclaringType);

            //调用GetValue
            il.Emit(OpCodes.Callvirt, ClassCache.PropertyValueGetter);
            DebugHelper.WriteLine("Callvirt " + ClassCache.PropertyValueGetter.Name);
            UnPacket(info.PropertyType);

            
            
        }

        public static void SetPrivateProperty(Action action, PropertyInfo info,object value)
        {
            ILGenerator il = ThreadCache.GetIL();
            //加载SetValue的第一个参数 即PropertyInfo
            il.Emit(OpCodes.Ldtoken, info.DeclaringType);
            il.Emit(OpCodes.Call, ClassCache.ClassHandle);
            il.Emit(OpCodes.Ldstr, info.Name);
            il.Emit(OpCodes.Ldc_I4_S, 44);
            il.Emit(OpCodes.Call, ClassCache.PropertyInfoGetter);

            DebugHelper.WriteLine("Ldtoken " + info.DeclaringType.Name);
            DebugHelper.WriteLine("Call " + ClassCache.ClassHandle.Name);
            DebugHelper.WriteLine("Ldstr " + info.Name);
            DebugHelper.WriteLine("Ldc_I4_S " + 44);
            DebugHelper.WriteLine("Call " + ClassCache.PropertyInfoGetter.Name);

            //加载SetValue的第二个参数
            if (action!=null)
            {
                action();
            }
            Packet(info.DeclaringType);


            //加载SetValue的第三个参数
            EData.NoErrorLoad(value, il);
            if (value != null)
            {
               Packet(value.GetType());
            }

            //调用SetValue
            il.Emit(OpCodes.Callvirt, ClassCache.PropertyValueSetter);
            DebugHelper.WriteLine("Callvirt " + ClassCache.PropertyValueSetter.Name);


        }

        public static void SetPublicProperty(Action action, PropertyInfo info, object value)
        {
            MethodInfo method = info.GetSetMethod(true);
            //静态属性
            if (!method.IsStatic)
            {
                if (action!=null)
                {
                    action();
                }
            }
            EData.NoErrorLoad(value, ThreadCache.GetIL());
            CallMethod(info.DeclaringType, method);
        }

        public static void SetPrivateField(Action action, FieldInfo info, object value)
        {
            ILGenerator il = ThreadCache.GetIL();
            //加载SetValue的第一个参数 即FieldInfo
            il.Emit(OpCodes.Ldtoken, info.DeclaringType);
            il.Emit(OpCodes.Call, ClassCache.ClassHandle);
            il.Emit(OpCodes.Ldstr, info.Name);
            il.Emit(OpCodes.Ldc_I4_S, 44);
            il.Emit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

            DebugHelper.WriteLine("Ldtoken " + info.DeclaringType.Name);
            DebugHelper.WriteLine("Call " + ClassCache.ClassHandle.Name);
            DebugHelper.WriteLine("Ldstr " + info.Name);
            DebugHelper.WriteLine("Ldc_I4_S " + 44);
            DebugHelper.WriteLine("Callvirt " + ClassCache.FieldInfoGetter.Name);

            //加载SetValue的第二个参数
            if (action!=null)
            {
                action();
            }
            Packet(info.DeclaringType);

            //加载SetValue的第三个参数
            EData.NoErrorLoad(value, il);
            if (value != null)
            {
                Packet(value.GetType());
            }
            //调用SetValue
            il.Emit(OpCodes.Callvirt, ClassCache.FieldValueSetter);
            DebugHelper.WriteLine("Callvirt " + ClassCache.FieldValueSetter.Name);


        }

        public static void SetPublicField(Action action, FieldInfo info, object value)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (info.IsStatic)
            {
                //填充静态字段
                EData.NoErrorLoad(value, il);
                il.Emit(OpCodes.Stsfld, info);
                DebugHelper.WriteLine("Stsfld " + info.Name);
            }
            else
            {
                if (action!=null)
                {
                    action();
                }
                EData.NoErrorLoad(value, il);
                il.Emit(OpCodes.Stfld, info);
                DebugHelper.WriteLine("Stfld " + info.Name);
            }
        }

        public static void Return()
        {
            ILGenerator il = ThreadCache.GetIL();
            il.Emit(OpCodes.Ret);
            DebugHelper.WriteLine("Ret");
        }

        public static void ReturnValue(object value)
        {
            EData.NoErrorLoad(value,ThreadCache.GetIL());
            Return();
        }

        public static void InitObject(Type type)
        {
            ThreadCache.GetIL().Emit(OpCodes.Initobj, type);
            DebugHelper.WriteLine("Initobj " + type.ToString());
        }
    }
}
