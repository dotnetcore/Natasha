
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Utils
{
    //为深度复制提供快速反射
    public static class EReflector
    {
        public static Dictionary<Type, Dictionary<string, GetterDelegate>> GetMethodDict;
        public static Dictionary<Type, Dictionary<string, SetterDelegate>> SetMethodDict;
        static EReflector()
        {
            GetMethodDict = new Dictionary<Type, Dictionary<string, GetterDelegate>>();
            SetMethodDict = new Dictionary<Type, Dictionary<string, SetterDelegate>>();
        }

        public static void Create(Type type)
        {
            ClassStruction model = EModel.CreateModelFromAction(null, type).Struction;
            #region GetMethod
            Dictionary<string, GetterDelegate> GetDict = new Dictionary<string, GetterDelegate>();
            foreach (var item in model.Properties)
            {
                GetDict[item.Key] = GetterFunc(model, item.Value);
            }
            foreach (var item in model.Fields)
            {
                GetDict[item.Key] = GetterFunc(model, item.Value);
            }
            GetMethodDict[type] = GetDict;
            #endregion

            #region SetMethod
            Dictionary<string, SetterDelegate> SetDict = new Dictionary<string, SetterDelegate>();
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


        public static GetterDelegate GetterFunc(ClassStruction model, PropertyInfo info)
        {
            MethodInfo getter = info.GetGetMethod(true);
            if (getter == null || getter.IsPrivate || getter.GetParameters().Length > 0)
            {
                return null;
            }
            return (GetterDelegate)(EHandler.CreateMethod<object, object>((til) =>
            {
                LocalBuilder builder = null;
                if (!getter.IsStatic)
                {
                    builder = til.DeclareLocal(model.TypeHandler);
                    til.REmit(OpCodes.Ldarg_0);
                    til.UnPacket(model.TypeHandler);
                    til.REmit(OpCodes.Stloc_S, builder.LocalIndex);
                }
                EModel localModel = EModel.CreateModelFromBuilder(builder, model.TypeHandler);
                localModel.LPropertyValue(info.Name).Packet();
            },"Getter " + info.DeclaringType.Name+ " " + info.Name).Compile(typeof(GetterDelegate)));
        }
        public static GetterDelegate GetterFunc(ClassStruction model, FieldInfo info)
        {
            return (GetterDelegate)(EHandler.CreateMethod<object, object>((til) =>
            {
                LocalBuilder builder = null;
                if (!info.IsStatic)
                {
                    builder = til.DeclareLocal(model.TypeHandler);
                    til.REmit(OpCodes.Ldarg_0);
                    til.UnPacket(model.TypeHandler);
                    til.REmit(OpCodes.Stloc_S, builder.LocalIndex);
                }

                EModel localModel = EModel.CreateModelFromBuilder(builder, model.TypeHandler);
                localModel.LFieldValue(info.Name).Packet();
            }, "Getter "+info.DeclaringType.Name + " " + info.Name).Compile(typeof(GetterDelegate)));
        }
        public static SetterDelegate SetterFunc(ClassStruction model, PropertyInfo info)
        {
            if (info.GetSetMethod(true) == null || info.GetSetMethod(true).IsPrivate)
            {
                return null;
            }
            return (SetterDelegate)(EHandler.CreateMethod<object, object, ENull>((til) =>
            {
                LocalBuilder builder = null;
                if (!info.GetSetMethod(true).IsStatic)
                {
                    builder = til.DeclareLocal(model.TypeHandler);
                    til.REmit(OpCodes.Ldarg_0);
                    til.UnPacket(model.TypeHandler);
                    til.REmit(OpCodes.Stloc_S, builder.LocalIndex);
                }

                EModel localModel = EModel.CreateModelFromBuilder(builder, model.TypeHandler);
                localModel.SProperty(info.Name, () =>
                {
                    til.REmit(OpCodes.Ldarg_1);
                    til.UnPacket(info.PropertyType);
                });
            }, "Setter " + info.DeclaringType.Name + " " + info.Name).Compile(typeof(SetterDelegate)));
        }
        public static SetterDelegate SetterFunc(ClassStruction model, FieldInfo info)
        {
            return (SetterDelegate)(EHandler.CreateMethod<object, object, ENull>((til) =>
            {
                LocalBuilder builder = null;
                if (!info.IsStatic)
                {
                    builder = til.DeclareLocal(model.TypeHandler);
                    til.REmit(OpCodes.Ldarg_0);
                    til.UnPacket(model.TypeHandler);
                    til.REmit(OpCodes.Stloc_S, builder.LocalIndex);
                }

                EModel localModel = EModel.CreateModelFromBuilder(builder, model.TypeHandler);
                localModel.SField(info.Name, () =>
                {
                    til.REmit(OpCodes.Ldarg_1);
                    til.UnPacket(info.FieldType);
                });
            },"Setter " + info.DeclaringType.Name + " " + info.Name).Compile(typeof(SetterDelegate)));
        }
    }
}
