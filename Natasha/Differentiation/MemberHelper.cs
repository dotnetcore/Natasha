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
    public static class MemberHelper
    {
        #region 属性操作
        public static void LoadPublicProperty(Action thisInStack, PropertyInfo info)
        {
            MethodInfo method = info.GetGetMethod(true);
            //静态属性
            if (!method.IsStatic)
            {
                thisInStack();
            }
            MethodHelper.CallMethod(info.DeclaringType, method);
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
            OperatorHelper.Packet(info.DeclaringType);

            //调用GetValue
            il.Emit(OpCodes.Callvirt, ClassCache.PropertyValueGetter);
            DebugHelper.WriteLine("Callvirt", ClassCache.PropertyValueGetter.Name);
            OperatorHelper.UnPacket(info.PropertyType);



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
            OperatorHelper.Packet(info.DeclaringType);


            //加载SetValue的第三个参数
            DataHelper.NoErrorLoad(value, il);
            if (value != null)
            {
                OperatorHelper.Packet(value.GetType());
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
            DataHelper.NoErrorLoad(value, ThreadCache.GetIL());
            MethodHelper.CallMethod(info.DeclaringType, method);
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
            OperatorHelper.Packet(info.DeclaringType);

            //调用GetValue
            il.Emit(OpCodes.Callvirt, ClassCache.FieldValueGetter);
            DebugHelper.WriteLine("Callvirt", ClassCache.FieldValueGetter.Name);
            //拆箱
            OperatorHelper.UnPacket(info.FieldType);



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
            if (thisInStack != null)
            {
                thisInStack();
            }
            OperatorHelper.Packet(info.DeclaringType);

            //加载SetValue的第三个参数
            DataHelper.NoErrorLoad(value, il);
            if (value != null)
            {
                OperatorHelper.Packet(value.GetType());
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
                DataHelper.NoErrorLoad(value, il);
                il.Emit(OpCodes.Stsfld, info);
                DebugHelper.WriteLine("Stsfld", info.Name);
            }
            else
            {
                if (thisInStack != null)
                {
                    thisInStack();
                }
                DataHelper.NoErrorLoad(value, il);
                il.Emit(OpCodes.Stfld, info);
                DebugHelper.WriteLine("Stfld", info.Name);
            }
        }
        #endregion
    }
}
