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
                il.REmit(OpCodes.Ldarg_0);
                il.REmit(OpCodes.Unbox_Any, TypeHandler);
                il.REmit(OpCodes.Stloc_S, builder);

                EVar returnTrueResult = true;
                EVar returnFalseResult = false;

               
                EModel model = EModel.CreateModelFromBuilder(builder, TypeHandler);
                #region Property 
                Dictionary<string, PropertyInfo> properties = model.Struction.Properties;
                foreach (var item in properties)
                {
                    DebugHelper.WriteLine("检测" + item.Key + "是否为默认值:");
                    Type type = item.Value.PropertyType;
                    if (type.IsValueType && type.IsPrimitive)
                    {
                        EJudge.If(EDefault.IsDefault(type, () => { model.LPropertyValue(item.Key); }))(() =>
                        {

                        }).Else(() =>
                        {
                            MethodHelper.ReturnValue(false);
                        });

                    }
                    else if (type.IsClass)
                    {
                        EJudge.If(ENull.IsNull(() => { model.LPropertyValue(item.Key); }))(() =>
                        {

                        }).Else(() =>
                        {
                            MethodHelper.ReturnValue(false);
                        });
                    }
                }
                #endregion
                #region Fields
                Dictionary<string, FieldInfo> fields = model.Struction.Fields;
                foreach (var item in fields)
                {
                    DebugHelper.WriteLine("检测"+item.Key+"是否为默认值:");
                    Type type = item.Value.FieldType;
                    if (type.IsValueType && type.IsPrimitive)
                    {
                        EJudge.If(EDefault.IsDefault(type, model.DLoadValue(item.Key).DelayAction))(() =>
                       {

                       }).Else(() =>
                       {
                           MethodHelper.ReturnValue(false);
                       });
                    }
                    else if (type.IsClass)
                    {
                        EJudge.If(ENull.IsNull(model.DLoadValue(item.Key).DelayAction))(() =>
                        {

                        }).Else(() =>
                        {
                            MethodHelper.ReturnValue(false);
                        });
                    }
                }
                #endregion
                il.EmitBoolean(true);
            }, "Check").Compile(typeof(CheckStructDelegate));
            ClassCache.CheckStructDict[TypeHandler] = (CheckStructDelegate)func;
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
