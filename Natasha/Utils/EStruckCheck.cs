using Natasha.Cache;
using Natasha.Debug;
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
                il.Emit(OpCodes.Stloc_S, builder);

                
                DebugHelper.WriteLine("Ldarg_0");
                DebugHelper.WriteLine("Unbox_Any "+ TypeHandler.Name);
                DebugHelper.WriteLine("Stloc_S "+ builder);

                EVar returnTrueResult = true;
                EVar returnFalseResult = false;

               
                EModel model = EModel.CreateModelFromBuilder(builder, TypeHandler);
                DebugHelper.WriteLine("检测属性是否为默认值");
                #region Property 
                Dictionary<string, PropertyInfo> properties = model.Struction.Properties;
                foreach (var item in properties)
                {
                    DebugHelper.WriteLine("检测" + item.Key + "是否为默认值:");
                    Type type = item.Value.PropertyType;
                    if (type.IsValueType && type.IsPrimitive)
                    {
                        EJudge.If(EDefault.IsDefault(type, () => { model.LProperty(item.Key); }))(() =>
                        {

                        }).Else(() =>
                        {
                            EmitHelper.ReturnValue(false);
                        });

                    }
                    else if (type.IsClass)
                    {
                        EJudge.If(ENull.StringIsNull(() => { model.LProperty(item.Key); }))(() =>
                        {

                        }).Else(() =>
                        {
                            EmitHelper.ReturnValue(false);
                        });
                    }
                }
                #endregion
                DebugHelper.WriteLine("检测字段是否为默认值");
                #region Fields
                Dictionary<string, FieldInfo> fields = model.Struction.Fields;
                foreach (var item in fields)
                {
                    DebugHelper.WriteLine("检测"+item.Key+"是否为默认值:");
                    Type type = item.Value.FieldType;
                    if (type.IsValueType && type.IsPrimitive)
                    {
                        EJudge.If(EDefault.IsDefault(type, model.DLoad(item.Key).DelayAction))(() =>
                       {

                       }).Else(() =>
                       {
                           EmitHelper.ReturnValue(false);
                       });
                    }
                    else if (type.IsClass)
                    {
                        EJudge.If(ENull.StringIsNull(model.DLoad(item.Key).DelayAction))(() =>
                        {

                        }).Else(() =>
                        {
                            EmitHelper.ReturnValue(false);
                        });
                    }
                }
                #endregion
                EData.LoadObject(true);
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
