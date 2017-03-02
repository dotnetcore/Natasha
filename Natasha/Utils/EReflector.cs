using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Utils
{
    //为深度复制提供快速反射
    public static class EReflector
    {
        public static Dictionary<Type, Dictionary<string, Func<object, object>>> GetMethodDict;
        public static Dictionary<Type, Dictionary<string, Action<object, object>>> SetMethodDict;
        static EReflector()
        {
            GetMethodDict = new Dictionary<Type, Dictionary<string, Func<object, object>>>();
            SetMethodDict = new Dictionary<Type, Dictionary<string, Action<object, object>>>();
        }

        public static void Create(Type type)
        {
            ClassStruction model = EModel.CreateModelFromAction(null, type).Struction;
            #region GetMethod
            Dictionary<string, Func<object, object>> GetDict = new Dictionary<string, Func<object, object>>();
            foreach (var item in model.Properties)
            {
                GetDict[item.Key] = GetterFunc(model,item.Value);
            }
            foreach (var item in model.Fields)
            {
                GetDict[item.Key] = GetterFunc(model, item.Value);
            }
            GetMethodDict[type] = GetDict;
            #endregion

            #region SetMethod
            Dictionary<string, Action<object, object>> SetDict = new Dictionary<string, Action<object, object>>();
            foreach (var item in model.Properties)
            {
                SetDict[item.Key] = SetterFunc(model, item.Value);
            }
            foreach (var item in model.Fields)
            {
                SetDict[item.Key] = SetterFunc(model, item.Value);
            }
            SetMethodDict[type] = SetDict;
            #endregion

        }
        public static void Create<T>()
        {
            Create(typeof(T));
        }


        public static Func<object,object> GetterFunc(ClassStruction model,PropertyInfo info)
        {
            MethodInfo getter = info.GetGetMethod(true);
            if (getter == null || getter.IsPrivate || getter.GetParameters().Length>0)
            {
                return null;
            }
            return (Func<object, object>)(EHandler.CreateMethod<object, object>((til) =>
            {
                LocalBuilder builder = til.DeclareLocal(model.TypeHandler);
                til.Emit(OpCodes.Ldarg_0);
                til.Emit(OpCodes.Unbox_Any, model.TypeHandler);
                til.Emit(OpCodes.Stloc, builder);
                EModel localModel = EModel.CreateModelFromBuilder(builder, model.TypeHandler);
                EPacket.Packet(info.PropertyType, () => { localModel.LProperty(info.Name); });
            }, "Getter").Compile());
        }
        public static Func<object, object> GetterFunc(ClassStruction model, FieldInfo info)
        {
            return (Func<object, object>)(EHandler.CreateMethod<object, object>((til) =>
            {
                LocalBuilder builder = til.DeclareLocal(model.TypeHandler);
                til.Emit(OpCodes.Ldarg_0);
                til.Emit(OpCodes.Unbox_Any, model.TypeHandler);
                til.Emit(OpCodes.Stloc, builder);
                EModel localModel = EModel.CreateModelFromBuilder(builder, model.TypeHandler);
                EPacket.Packet(info.FieldType, () => { localModel.LField(info.Name); });
            }, "Getter").Compile());
        }
        public static Action<object, object> SetterFunc(ClassStruction model, PropertyInfo info)
        {
            if (info.GetSetMethod(true) == null || info.GetSetMethod(true).IsPrivate)
            {
                return null;
            }
            return (Action<object, object>)(EHandler.CreateMethod<object, object, ENull>((til) =>
            {
                LocalBuilder builder = til.DeclareLocal(model.TypeHandler);
                til.Emit(OpCodes.Ldarg_0);
                til.Emit(OpCodes.Unbox_Any, model.TypeHandler);
                til.Emit(OpCodes.Stloc, builder);
                EModel localModel = EModel.CreateModelFromBuilder(builder, model.TypeHandler);
                localModel.SProperty(info.Name, () => {
                    localModel.ilHandler.Emit(OpCodes.Ldarg_1);
                    EPacket.UnPacket(info.PropertyType);
                });
            }, "Setter").Compile());
        }
        public static Action<object, object> SetterFunc(ClassStruction model, FieldInfo info)
        {
            return (Action<object, object>)(EHandler.CreateMethod<object, object, ENull>((til) =>
            {
                LocalBuilder builder = til.DeclareLocal(model.TypeHandler);
                til.Emit(OpCodes.Ldarg_0);
                til.Emit(OpCodes.Unbox_Any, model.TypeHandler);
                til.Emit(OpCodes.Stloc, builder);
                EModel localModel = EModel.CreateModelFromBuilder(builder, model.TypeHandler);
                localModel.SField(info.Name, () => {
                        localModel.ilHandler.Emit(OpCodes.Ldarg_1);
                        EPacket.UnPacket(info.FieldType);
                });
            }, "Setter").Compile());
        }
    }
}
