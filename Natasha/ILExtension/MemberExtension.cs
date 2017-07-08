using Natasha;
using Natasha.Cache;


namespace System.Reflection.Emit
{


    public static class MemberExtension
    {
        public static bool IsNullable(this ILGenerator il, Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return true;
            }
            return false;
        }
        public static void CallNullableCtor(this ILGenerator il, Type type)
        {
            if (il.IsNullable(type))
            {
                ConstructorInfo ctor = type.GetConstructor(new Type[] { type.GenericTypeArguments[0] });
                il.REmit(OpCodes.Newobj, ctor);
            }
        }


        public static LocalBuilder LoadStructLocalBuilder(this ILGenerator il, Type type) {
            LocalBuilder tempBuidler = il.DeclareLocal(type);
            il.EmitStoreBuilder(tempBuidler);
            il.LoadBuilder(tempBuidler);
            return tempBuidler;
        }
        //public static void CallNullableValue(this ILGenerator il, Type type,bool IsProperty=false)
        //{
        //    if (il.IsNullable(type))
        //    {
        //        //PropertyInfo info = type.GetProperty("Value");
        //        if (IsProperty)
        //        {
        //          LocalBuilder tempBuidler = il.DeclareLocal(type);
        //            il.EmitStoreBuilder(tempBuidler);
        //            il.LoadBuilder(tempBuidler);
        //        }
        //        MethodInfo value = type.GetMethod("GetValueOrDefault",new Type[0]);
        //        MethodHelper.CallMethod(value);
        //        //EJudge.If(()=> { MethodHelper.CallMethod(hashInfo.GetGetMethod(true)); il.BoolInStack(typeof(bool)); })(() => {
        //        //    MethodHelper.CallMethod(info.GetGetMethod(true));
        //        //    //il.BoolInStack(type.GenericTypeArguments[0]);
        //        //    il.LoadOne(type.GenericTypeArguments[0]);
        //        //}).Else(() => {
        //        //    il.LoadOne(type.GenericTypeArguments[0]);
        //        //});
        //    }
        //}

        #region 属性操作
        public static void LoadPublicPropertyValue(this ILGenerator il, Action thisInStack, PropertyInfo info)
        {
            MethodInfo method = info.GetGetMethod(true);
            //静态属性
            if (!method.IsStatic)
            {
                thisInStack();
            }

            MethodHelper.CallMethod(method);
            il.BoolInStack(info.PropertyType);
            //il.CallNullableValue(info.PropertyType,true);
        }
        public static void LoadPublicProperty(this ILGenerator il, Action thisInStack, PropertyInfo info)
        {
            MethodInfo method = info.GetGetMethod(true);
            //静态属性
            if (!method.IsStatic)
            {
                thisInStack();
            }

            MethodHelper.CallMethod(method);
            if (info.PropertyType.IsValueType && ! info.PropertyType.IsPrimitive)
            {
                LocalBuilder builder = il.DeclareLocal(info.PropertyType);
                il.EmitStoreBuilder(builder);
                il.LoadBuilder(builder);
            }
            il.BoolInStack(info.PropertyType);
            //il.CallNullableValue(info.PropertyType,true);
        }
        public static void LoadPrivatePropertyValue(this ILGenerator il, Action thisInStack, PropertyInfo info)
        {
            //加载GetValue的第一个参数 即PropertyInfo
            il.REmit(OpCodes.Ldtoken, info.DeclaringType);
            il.REmit(OpCodes.Call, ClassCache.ClassHandle);
            il.REmit(OpCodes.Ldstr, info.Name);
            il.REmit(OpCodes.Ldc_I4_S, 60);
            il.REmit(OpCodes.Call, ClassCache.PropertyInfoGetter);

            //加载GetValue的第二个参数
            if (thisInStack != null)
            {
                thisInStack();
            };
            il.Packet(info.DeclaringType);

            //调用GetValue
            il.REmit(OpCodes.Callvirt, ClassCache.PropertyValueGetter);
            il.UnPacket(info.PropertyType);
            il.BoolInStack(info.PropertyType);
            //il.CallNullableValue(info.PropertyType,true);
        }
        public static void LoadPrivateProperty(this ILGenerator il, Action thisInStack, PropertyInfo info)
        {
            //加载GetValue的第一个参数 即PropertyInfo
            il.REmit(OpCodes.Ldtoken, info.DeclaringType);
            il.REmit(OpCodes.Call, ClassCache.ClassHandle);
            il.REmit(OpCodes.Ldstr, info.Name);
            il.REmit(OpCodes.Ldc_I4_S, 60);
            il.REmit(OpCodes.Callvirt, ClassCache.PropertyInfoGetter);

            //加载GetValue的第二个参数
            if (thisInStack != null)
            {
                thisInStack();
            };
            il.Packet(info.DeclaringType);

            //调用GetValue
            il.REmit(OpCodes.Callvirt, ClassCache.PropertyValueGetter);
            il.UnPacket(info.PropertyType);

            if (info.PropertyType.IsValueType && !info.PropertyType.IsPrimitive)
            {
                LocalBuilder builder = il.DeclareLocal(info.PropertyType);
                il.EmitStoreBuilder(builder);
                il.LoadBuilder(builder);
            }

            il.BoolInStack(info.PropertyType);
            //il.CallNullableValue(info.PropertyType,true);


        }
        public static void SetPrivateProperty(this ILGenerator il,Action thisInStack, PropertyInfo info, object value)
        {
            //加载SetValue的第一个参数 即PropertyInfo
            il.REmit(OpCodes.Ldtoken, info.DeclaringType);
            il.REmit(OpCodes.Call, ClassCache.ClassHandle);
            il.REmit(OpCodes.Ldstr, info.Name);
            il.REmit(OpCodes.Ldc_I4_S, 44);
            il.REmit(OpCodes.Call, ClassCache.PropertyInfoGetter);

            //加载SetValue的第二个参数
            if (thisInStack != null)
            {
                thisInStack();
            }
            il.Packet(info.DeclaringType);


            //加载SetValue的第三个参数
            il.NoErrorLoad(value, info.PropertyType);
            if (value != null)
            {
               il.Packet(info.PropertyType);
            }

            //调用SetValue
            il.REmit(OpCodes.Callvirt, ClassCache.PropertyValueSetter);

        }
        public static void SetPublicProperty(this ILGenerator il, Action thisInStack, PropertyInfo info, object value)
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
            il.NoErrorLoad(value, info.PropertyType);
            MethodHelper.CallMethod(method);
        }
        #endregion


        #region 字段操作    
        //加载私有字段
        public static void LoadPrivateFieldValue(this ILGenerator il, Action thisInStack, FieldInfo info)
        {
            //加载GetValue的第一个参数 即FieldInfo
            il.REmit(OpCodes.Ldtoken, info.DeclaringType);
            il.REmit(OpCodes.Call, ClassCache.ClassHandle);
            il.REmit(OpCodes.Ldstr, info.Name);
            il.REmit(OpCodes.Ldc_I4_S, 44);
            il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

            //加载GetValue的第二个参数
            if (thisInStack!=null)
            {
                thisInStack();
            }
            il.Packet(info.DeclaringType);

            //调用GetValue
            il.REmit(OpCodes.Callvirt, ClassCache.FieldValueGetter);
            
            //拆箱
            il.UnPacket(info.FieldType);
            il.BoolInStack(info.FieldType);

        }

        //public static void LoadPrivateField(this ILGenerator il, Action thisInStack, FieldInfo info)
        //{
        //    //加载GetValue的第一个参数 即FieldInfo
        //    il.REmit(OpCodes.Ldtoken, info.DeclaringType);
        //    il.REmit(OpCodes.Call, ClassCache.ClassHandle);
        //    il.REmit(OpCodes.Ldstr, info.Name);
        //    il.REmit(OpCodes.Ldc_I4_S, 44);
        //    il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

        //    //加载GetValue的第二个参数
        //    thisInStack();
        //    il.Packet(info.DeclaringType);

        //    //调用GetValue
        //    il.REmit(OpCodes.Callvirt, ClassCache.FieldValueGetter);

        //    //拆箱
        //    il.UnPacket(info.FieldType);
        //    il.BoolInStack(info.FieldType);

        //}
        //加载公有字段
        public static void LoadPublicFieldValue(this ILGenerator il, Action thisInStack, FieldInfo info)
        {
            if (info.IsStatic)
            {
                il.REmit(OpCodes.Ldsfld, info);
            }
            else
            {
                if (thisInStack != null)
                {
                    thisInStack();
                }
                il.REmit(OpCodes.Ldfld, info);
            }
            //il.CallNullableValue(info.FieldType);
            il.BoolInStack(info.FieldType);
        }
        //设置公有结构体字段
        public static void LoadPublicField(this ILGenerator il, Action thisInStack, FieldInfo info)
        {
            if (info.IsStatic)
            {
                il.REmit(OpCodes.Ldsflda, info);
            }
            else
            {
                if (thisInStack != null)
                {
                    thisInStack();
                }
                il.REmit(OpCodes.Ldflda, info);
            }
            il.BoolInStack(info.FieldType);
            //il.CallNullableValue(info.FieldType);
        }
        //设置私有字段
        public static void SetPrivateField(this ILGenerator il, Action thisInStack, FieldInfo info, object value)
        {
            //加载SetValue的第一个参数 即FieldInfo
            il.REmit(OpCodes.Ldtoken, info.DeclaringType);
            il.REmit(OpCodes.Call, ClassCache.ClassHandle);
            il.REmit(OpCodes.Ldstr, info.Name);
            il.REmit(OpCodes.Ldc_I4_S, 44);
            il.REmit(OpCodes.Callvirt, ClassCache.FieldInfoGetter);

            //加载SetValue的第二个参数
            if (thisInStack != null)
            {
                thisInStack();
            }
            il.Packet(info.DeclaringType);

            //加载SetValue的第三个参数
            il.NoErrorLoad(value, info.FieldType);
            if (value != null)
            {
                if (il.IsNullable(info.FieldType))
                {
                    il.Packet(info.FieldType);
                }
                else
                {
                    il.Packet(value.GetType());
                }
               
            }
            //调用SetValue
            il.REmit(OpCodes.Callvirt, ClassCache.FieldValueSetter);
        }
        //设置公有字段
        public static void SetPublicField(this ILGenerator il, Action thisInStack, FieldInfo info, object value)
        {
            if (info.IsStatic)
            {
                //填充静态字段
                il.NoErrorLoad(value, info.FieldType);
                il.REmit(OpCodes.Stsfld, info);
            }
            else
            {
                if (thisInStack != null)
                {
                    thisInStack();
                }
                il.NoErrorLoad(value, info.FieldType);
                il.REmit(OpCodes.Stfld, info);
            }
        }
        #endregion
    }
}
