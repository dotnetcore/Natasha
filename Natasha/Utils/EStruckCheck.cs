using Natasha.Cache;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Utils
{
    //检测结构体是否为默认值
    public class EStruckCheck
    {
        public static void Create(Type TypeHandler)
        {
            Delegate func = EHandler.CreateMethod<object, bool>((il) =>
            {
                LocalBuilder builder = il.DeclareLocal(TypeHandler);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Unbox_Any, TypeHandler);
                il.Emit(OpCodes.Stloc, builder);
                EModel model = EModel.CreateModelFromBuilder(builder, TypeHandler);
                #region Property 
                Dictionary<string, PropertyInfo> properties = model.Struction.Properties;
                foreach (var item in properties)
                {
                    Type type = item.Value.PropertyType;
                    if (type.IsValueType && type.IsPrimitive)
                    {
                        EJudge.If(EDefault.IsDefault(type, () => { model.LProperty(item.Key); }))(() =>
                        {

                        }).Else(() =>
                        {
                            il.Emit(OpCodes.Ldc_I4_0);
                            il.Emit(OpCodes.Ret);
                        });

                    }
                    else if (type.IsClass)
                    {
                        EJudge.If(ENull.StringIsNull(() => { model.LProperty(item.Key); }))(() =>
                        {

                        }).Else(() =>
                        {
                            il.Emit(OpCodes.Ldc_I4_0);
                            il.Emit(OpCodes.Ret);
                        });
                    }
                }
                #endregion
                #region Fields
                Dictionary<string, FieldInfo> fields = model.Struction.Fields;
                foreach (var item in fields)
                {
                    Type type = item.Value.FieldType;
                    if (type.IsValueType && type.IsPrimitive)
                    {
                        EJudge.If(EDefault.IsDefault(type, () => { model.LField(item.Key); }))(() =>
                        {

                        }).Else(() =>
                        {
                            il.Emit(OpCodes.Ldc_I4_0);
                            il.Emit(OpCodes.Ret);
                        });
                    }
                    else if (type.IsClass)
                    {
                        EJudge.If(ENull.StringIsNull(() => { model.LField(item.Key); }))(() =>
                        {

                        }).Else(() =>
                        {
                            il.Emit(OpCodes.Ldc_I4_0);
                            il.Emit(OpCodes.Ret);
                        });
                    }
                }
                #endregion
                il.Emit(OpCodes.Ldc_I4_1);
            }, "Check").Compile();
            ClassCache.CheckStructDict[TypeHandler] = (Func<object, bool>)func;
        }
        public static void Create<T>()
        {
            Create(typeof(T));
        }
        public static bool IsDefaultStruct<T>(T value)
        {
            return IsDefaultStruct(value, typeof(T));
        }
        public static bool IsDefaultStruct(object value, Type type)
        {
            if (!ClassCache.CheckStructDict.ContainsKey(type))
            {
                Create(type);
            }
            return ClassCache.CheckStructDict[type](value);
        }
    }
}
