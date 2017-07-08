using Natasha.Cache;
using Natasha.Core;

using Natasha.Utils;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Natasha
{
    //自定义类
    public class ClassBuilder : ILoadInstance
    {
        private TypeBuilder classHandler;
        public ClassStruction Struction;
        public string Name;
        public ILGenerator il;

        private ClassBuilder(string name, TypeAttributes option)
        {
            Name = name;

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(Assembly.GetEntryAssembly().GetName(), AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
            classHandler = moduleBuilder.DefineType(name, option);

            Struction = new ClassStruction();
            Struction.Name = Name;
        }
        public void EndBuilder()
        {
            ClassCache.DynamicClassDict[Name] = classHandler.CreateType();
        }
        public static ClassBuilder CreateModel(string name)
        {
            return CreateModel(name, TypeAttributes.Class | TypeAttributes.Public);
        }
        public static ClassBuilder CreateModel(string name, TypeAttributes option)
        {
            return new ClassBuilder(name, option);
        }

        #region 默认构造函数
        public void CreateDefaultConstructor()
        {
            CreateConstructor((til) => { til.REmit(OpCodes.Ret); });
        }
        public void CreateConstructor(Action<ILGenerator> action, params Type[] args)
        {
            CreateConstructor(action, MethodAttributes.Public, args);
        }
        public void CreateConstructor(Action<ILGenerator> action, MethodAttributes option, params Type[] args)
        {
            ConstructorBuilder constructorBuilder = classHandler.DefineConstructor(
                option, CallingConventions.Standard, args);
            ILGenerator tempIL = constructorBuilder.GetILGenerator();

            tempIL.Emit(OpCodes.Ldarg_0);
            tempIL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes));
            action(tempIL);
        }
        #endregion

        #region 创建字段
        public FieldInfo CreateField<T>(string name, FieldAttributes option)
        {
            return CreateField(name, typeof(T), option);
        }
        public FieldInfo CreateField(string name, Type type, FieldAttributes option)
        {
            FieldInfo field = classHandler.DefineField(name, type, option);
            Struction.Fields[name] = field;
            return field;
        }
        #endregion

        #region 创建属性
        public PropertyInfo CreateProperty<T>(string name)
        {
            return CreateProperty(name, typeof(T));
        }
        public PropertyInfo CreateProperty(string name, Type type)
        {
            return CreateProperty(name, type, PropertyAttributes.None);
        }
        public PropertyInfo CreateProperty<T>(string name, PropertyAttributes option)
        {
            return CreateProperty(name, typeof(T), option);
        }
        public PropertyInfo CreateProperty(string name, Type type, PropertyAttributes option)
        {
            PropertyBuilder propertyInfo = classHandler.DefineProperty(name, option, type, null);
            FieldInfo innerField = CreateField("_" + name, type, FieldAttributes.Private);

            MethodBuilder setMethod = classHandler.DefineMethod("setter_" + name, MethodAttributes.Public, null, new Type[] { type });
            ILGenerator set_il = setMethod.GetILGenerator();
            set_il.REmit(OpCodes.Ldarg_0);
            set_il.REmit(OpCodes.Ldarg_1);
            set_il.REmit(OpCodes.Stfld, innerField);
            set_il.REmit(OpCodes.Ret);

            MethodBuilder getMethod = classHandler.DefineMethod("getter_" + name, MethodAttributes.Public, type, null);
            ILGenerator get_il = getMethod.GetILGenerator();
            get_il.REmit(OpCodes.Ldarg_0);
            get_il.REmit(OpCodes.Ldfld, innerField);
            get_il.REmit(OpCodes.Ret);

            propertyInfo.SetSetMethod(setMethod);
            propertyInfo.SetGetMethod(getMethod);

            Struction.Properties[name] = propertyInfo;

            return propertyInfo;
        }
        public PropertyInfo CreateProperty<T>(string name, PropertyAttributes option, Action<ILGenerator> SetMethod, Action<ILGenerator> GetMethod)
        {
            return CreateProperty(name, typeof(T), option, SetMethod, GetMethod);
        }
        public PropertyInfo CreateProperty(string name, Type type, PropertyAttributes option, Action<ILGenerator> SetMethod, Action<ILGenerator> GetMethod)
        {
            PropertyBuilder propertyInfo = classHandler.DefineProperty(name, option, type, null);
            MethodBuilder setMethod = null;
            if (SetMethod != null)
            {
                setMethod = classHandler.DefineMethod(name, MethodAttributes.Public, null, new Type[] { type });
                SetMethod(setMethod.GetILGenerator());
            }
            MethodBuilder getMethod = null;
            if (GetMethod != null)
            {
                getMethod = classHandler.DefineMethod(name, MethodAttributes.Public, type, null);
                GetMethod(getMethod.GetILGenerator());
            }
            propertyInfo.SetSetMethod(setMethod);
            propertyInfo.SetGetMethod(getMethod);
            Struction.Properties[name] = propertyInfo;
            return propertyInfo;
        }
        #endregion

        #region 创建函数

        public void CreateMethod<R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            MakeMethodBody<R>(name, option, action, new Type[0]);
        }
        public void CreateMethod<T1, R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            Type[] ParameterTypes = new Type[1];
            ParameterTypes[0] = typeof(T1);
            MakeMethodBody<R>(name, option, action, ParameterTypes);
        }
        public void CreateMethod<T1, T2, R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            Type[] ParameterTypes = new Type[2];
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            MakeMethodBody<R>(name, option, action, ParameterTypes);
        }
        public void CreateMethod<T1, T2, T3, R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            Type[] ParameterTypes = new Type[3];
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            MakeMethodBody<R>(name, option, action, ParameterTypes);
        }
        public void CreateMethod<T1, T2, T3, T4, R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            Type[] ParameterTypes = new Type[4];
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            MakeMethodBody<R>(name, option, action, ParameterTypes);
        }
        public void CreateMethod<T1, T2, T3, T4, T5, R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            Type[] ParameterTypes = new Type[5];
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            MakeMethodBody<R>(name, option, action, ParameterTypes);
        }
        public void CreateMethod<T1, T2, T3, T4, T5, T6, R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            Type[] ParameterTypes = new Type[6];
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            ParameterTypes[5] = typeof(T6);
            MakeMethodBody<R>(name, option, action, ParameterTypes);
        }
        public void CreateMethod<T1, T2, T3, T4, T5, T6, T7, R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            Type[] ParameterTypes = new Type[7];
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            ParameterTypes[5] = typeof(T6);
            ParameterTypes[6] = typeof(T7);
            MakeMethodBody<R>(name, option, action, ParameterTypes);
        }
        public void CreateMethod<T1, T2, T3, T4, T5, T6, T7, T8, R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            Type[] ParameterTypes = new Type[8];
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            ParameterTypes[5] = typeof(T6);
            ParameterTypes[6] = typeof(T7);
            ParameterTypes[7] = typeof(T8);
            MakeMethodBody<R>(name, option, action, ParameterTypes);
        }
        public void CreateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(string name, MethodAttributes option, Action<ClassBuilder> action)
        {
            Type[] ParameterTypes = new Type[9];
            ParameterTypes[0] = typeof(T1);
            ParameterTypes[1] = typeof(T2);
            ParameterTypes[2] = typeof(T3);
            ParameterTypes[3] = typeof(T4);
            ParameterTypes[4] = typeof(T5);
            ParameterTypes[5] = typeof(T6);
            ParameterTypes[6] = typeof(T7);
            ParameterTypes[7] = typeof(T8);
            ParameterTypes[8] = typeof(T9);
            MakeMethodBody<R>(name, option, action, ParameterTypes);
        }
        private void MakeMethodBody<R>(string name, MethodAttributes option, Action<ClassBuilder> action, Type[] parameterTypes)
        {
            Type returnType = null;
            if (typeof(R) == typeof(ENull))
            {
                returnType = null;
            }
            else
            {
                returnType = typeof(R);
            }

            MethodBuilder methodInfo = classHandler.DefineMethod(name, option, returnType, parameterTypes);
            il = methodInfo.GetILGenerator();


            int ThreadId = Thread.CurrentThread.ManagedThreadId;
            ThreadCache.TKeyDict[ThreadId] = name;
            ThreadCache.TILDict[ThreadId] = il;


            if (action != null)
            {
                action(this);
                Struction.Methods[name] = methodInfo;
            }

            ThreadCache.TILDict.TryRemove(ThreadId,out il);
            ThreadCache.TKeyDict.TryRemove(ThreadId,out name);
        }
        #endregion

        #region 为函数操作提供操作
        #region Field操作

        public void SField(string fieldName, object value)
        {
            FieldInfo info = Struction.Fields[fieldName];

            if (info.IsStatic)
            {
                //填充静态字段
                il.NoErrorLoad(value);
                il.REmit(OpCodes.Stsfld, info);
            }
            else
            {
                //如果是结构体需要加载地址
                This();
                il.NoErrorLoad(value);
                il.REmit(OpCodes.Stfld, info);
            }
        }
        public void SField(string fieldName, Action action)
        {
            SField(fieldName, (object)action);
        }
        #endregion

        #region Property操作
        public void SProperty(string propertyName, object value)
        {
            PropertyInfo info = Struction.Properties[propertyName];
            MethodInfo method = info.GetSetMethod(true);

            //静态属性
            if (!method.IsStatic)
            {
                This();
            }

            il.NoErrorLoad(value);

            MethodHelper.CallMethod(method);
        }
        public void SProperty(string propertyName, Action action)
        {
            SProperty(propertyName, (object)action);
        }
        #endregion

        #region Method操作
        public ComplexType EMethod(string methodName, Action<ILGenerator> actionLoadValue)
        {
            MethodInfo info = Struction.Methods[methodName];
            if (info.IsStatic)
            {
                actionLoadValue(il);
                il.REmit(OpCodes.Call, info);
            }
            else
            {
                This();
                actionLoadValue(il);
                il.REmit(OpCodes.Callvirt, info);
            }
            il.BoolInStack(info.ReturnType);
            return EModel.CreateModelFromAction(null, Struction.TypeHandler);
        }
        public ComplexType EMethod(string methodName)
        {
            return EMethod(methodName, (il) => { });
        }
        #endregion
        #endregion

        #region 嵌套调用接口
        public ComplexType LField(string fieldName)
        {
            FieldInfo info = Struction.Fields[fieldName];
            Type type = info.FieldType;

            //静态字段
            if (info.IsStatic)
            {
                if (type.IsValueType && !type.IsPrimitive)
                {
                    il.REmit(OpCodes.Ldsflda, info);
                }
                else
                {
                    il.REmit(OpCodes.Ldsfld, info);
                }
            }
            else
            {
                //加载地址
                This();
                if (type.IsValueType && !type.IsPrimitive)
                {
                    il.REmit(OpCodes.Ldflda, info);
                }
                else
                {
                    il.REmit(OpCodes.Ldfld, info);
                }
            }
            //如果单独加载了bool类型的值
            il.BoolInStack(type);
            return EModel.CreateModelFromAction(null, type);
        }

        public ComplexType LPropertyValue(string propertyName)
        {
            PropertyInfo info = Struction.Properties[propertyName];
            Type type = info.PropertyType;
            MethodInfo method = info.GetGetMethod(true);

            //静态属性
            if (method.IsStatic)
            {
                il.REmit(OpCodes.Call, method);
            }
            else
            {
                This();
                il.REmit(OpCodes.Callvirt, method);
            }
            il.BoolInStack(type);
            return EModel.CreateModelFromAction(null, type);
        }

        public ComplexType LFieldValue(string fieldName)
        {
            FieldInfo info = Struction.Fields[fieldName];
            Type type = info.FieldType;

            //静态字段
            if (info.IsStatic)
            {
                il.REmit(OpCodes.Ldsfld, info);
            }
            else
            {
                This();
                il.REmit(OpCodes.Ldfld, info);
            }
            //如果单独加载了bool类型的值
            if (type == typeof(bool))
            {
                ThreadCache.SetJudgeCode(OpCodes.Brfalse_S);
            }
            return EModel.CreateModelFromAction(null, type);
        }

        public void Load()
        {
            il.REmit(OpCodes.Ldarg_0);
        }

        public void This()
        {
            il.REmit(OpCodes.Ldarg_0);
        }

        #endregion
    }
}
