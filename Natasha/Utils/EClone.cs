using Natasha.Cache;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Utils
{
    //克隆类
    public static class EClone
    {
        /// <summary>
        /// 深度复制对象，并压入栈中，创建一个新的临时变量
        /// </summary>
        /// <param name="value">类或者结构体或者数组</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static LocalBuilder GetCloneBuilder(object value, Type type)
        {
            ILGenerator il = ThreadCache.GetIL();
            if (value == null)
            {
                LocalBuilder builder = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Stloc_S, builder);
                return builder;
            }
            else if (type.IsValueType && !type.IsPrimitive)
            {
                LocalBuilder builder = il.DeclareLocal(type);
                if (type.IsEnum)
                {
                    EmitHelper.LoadObject((int)value);
                    il.Emit(OpCodes.Stloc_S, builder);
                    return builder;
                }
                if (EStruckCheck.IsDefaultStruct(value, type))
                {
                    EModel structModel = EModel.CreateModel(type).UseDefaultConstructor();
                    structModel.Load();
                    il.Emit(OpCodes.Stloc_S, builder);
                    return builder;
                }
            }
            else if (type.IsArray)
            {
                Array tempArray = (Array)value;
                Type instanceType = value.GetType().GetElementType();
                EArray array = EArray.CreateArraySpecifiedLength(value.GetType(), tempArray.Length);
                for (int i = 0; i < tempArray.Length; i += 1)
                {
                    object result = tempArray.GetValue(i);

                    if (result != null)
                    {
                        if (array.ArrayIsStruct && !array.BaseType.IsEnum)
                        {
                            if (EStruckCheck.IsDefaultStruct(result, instanceType))
                            {
                                continue;
                            }
                        }
                        array.StoreArray(i, result );
                    }
                }
                return array.Builder;
            }
            if (!EReflector.GetMethodDict.ContainsKey(type))
            {
                EReflector.Create(type);
            }

            Dictionary<string, GetterDelegate> GetDict = EReflector.GetMethodDict[type];
            ClassStruction struction = ClassCache.ClassInfoDict[type.Name];
            EModel model = EModel.CreateModel(type).UseDefaultConstructor();

            foreach (var item in struction.Properties)
            {
                MethodInfo info = item.Value.GetSetMethod(true);
                if (info == null || GetDict[item.Key] == null || info.IsPrivate)
                {
                    continue;
                }

                object result = GetDict[item.Key](value);
                if (result==null)
                {
                    continue;
                }
                model.SProperty(item.Key, result);
            }

            foreach (var item in struction.Fields)
            {
                object result = GetDict[item.Key](value);
                if (result == null)
                {
                    continue;
                }
                
                model.SField(item.Key, result);
            }
            return model.Builder;
        }
    }
}
