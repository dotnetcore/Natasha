using ConsoleFormator;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Natasha.Reflection.Reportor
{
    public class FeasibilityHandler
    {
        static FeasibilityHandler()
        {
            Alignmentor.RegisterAlignmentPage();
        }
        public StringBuilder ResultRecoder;
        public FeasibilityHandler()
        {
            ResultRecoder = new StringBuilder();
            // AlignmentHelper.Alignment(Cache);
        }
        #region 设置输出标题
        public void SetHeadInfo(string headInfo)
        {
            ResultRecoder.Append(headInfo);
        }
        public void SetMemberType(FieldInfo info)
        {
            ResultRecoder.Append("Field");
        }
        public void SetMemberType(PropertyInfo info)
        {
            ResultRecoder.Append("Property");
        }
        public void SetMemberType(MethodInfo info)
        {
            ResultRecoder.Append("Method");
        }
        public void SetMemberName(MemberInfo info)
        {
            ResultRecoder.Append(info.Name);
        }
        #endregion

        #region 对定义类型进行分析
        public void DeclaringAnalysis(MemberInfo info)
        {
            DeclaringAnalysis(info.DeclaringType);
        }
        public void DeclaringAnalysis(Type type)
        {
            int i = ResultRecoder.Length;
#if NETSTANDARD1_3
            TypeInfo tempType = type.GetTypeInfo();
#else
            Type tempType = type;
#endif

            ResultRecoder.Append(tempType.IsNotPublic ? "Internal、" : "");

            ResultRecoder.Append(tempType.IsPublic ? "Public、" : "");

            ResultRecoder.Append(tempType.IsSealed ? "Sealed、" : "");

            ResultRecoder.Append(tempType.IsNested ? "Nested Public、" : "");

            ResultRecoder.Append(tempType.IsNestedFamily ? "Nested Protected、" : "");

            ResultRecoder.Append(tempType.IsNestedPrivate ? "Nested Private、" : "");

            ResultRecoder.Append(tempType.IsNestedAssembly ? "Nested Internal、" : "");

            ResultRecoder.Append(tempType.IsNestedFamANDAssem ? "Nested Protected Internal、" : "");

            ResultRecoder.Append(tempType.IsAbstract ? "Abstract、" : "");

            ResultRecoder.Append(tempType.IsByRef ? "ref、" : "");

            ResultRecoder.Append(tempType.IsInterface ? "Interface、" : "");

            ResultRecoder.Append(tempType.IsClass ? "Class、" : "");

            ResultRecoder.Append(tempType.IsArray ? "Array、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }
        }

        #endregion

        #region 对Field进行分析
        /// <summary>
        /// 对FieldInfo进行访问级别分析
        /// </summary>
        /// <param name="info">FieldInfo实例</param>
        public void ProtectionAnalysis(FieldInfo info)
        {
            int i = ResultRecoder.Length;

            ResultRecoder.Append(info.IsPublic ? "Public、" : "");

            ResultRecoder.Append(info.IsPrivate ? "Private、" : "");

            ResultRecoder.Append(info.IsFamily ? "Protected、" : "");

            ResultRecoder.Append(info.IsAssembly ? "Internal、" : "");

            ResultRecoder.Append(info.IsFamilyOrAssembly ? "Protected Internal、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }
        }
        /// <summary>
        /// 对FieldInfo进行特性分析
        /// </summary>
        /// <param name="info">FieldInfo实例</param>
        public void ParticularityAnalysis(FieldInfo info)
        {
            int i = ResultRecoder.Length;

#if NETSTANDARD1_3
            TypeInfo type = info.FieldType.GetTypeInfo();
#else
            Type type = info.FieldType;
#endif

            ResultRecoder.Append(type.IsGenericType ? "Generic、" : "");

            ResultRecoder.Append(info.IsStatic ? "Static、" : "");

            ResultRecoder.Append(type.IsAbstract ? "Abstract、" : "");

            ResultRecoder.Append(type.IsByRef ? "Ref、" : "");

            ResultRecoder.Append(info.IsInitOnly ? "ReadOnly、" : "");

            ResultRecoder.Append(info.IsLiteral ? "Const、" : "");

            ResultRecoder.Append(type.IsNested ? "Public Nested、" : "");

            ResultRecoder.Append(type.IsNestedFamily ? "Protected Nested、" : "");

            ResultRecoder.Append(type.IsNestedPrivate ? "Private Nested、" : "");

            //ResultRecoder.Append(type.IsSealed ? "Sealed、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }

        }

        /// <summary>
        /// 对FieldInfo进行类型分析
        /// </summary>
        /// <param name="info">FieldInfo实例</param>
        public void KindsAnalysis(FieldInfo info)
        {
            int i = ResultRecoder.Length;

#if NETSTANDARD1_3
            TypeInfo type = info.FieldType.GetTypeInfo();
#else
            Type type = info.FieldType;
#endif
            ResultRecoder.Append(type.IsClass ? "Class、" : "");

            ResultRecoder.Append(type.IsAnsiClass ? "AnsiClass、" : "");

            ResultRecoder.Append(type.IsInterface ? "Interface、" : "");

            ResultRecoder.Append(type.IsArray ? "Array、" : "");

            ResultRecoder.Append(type.IsEnum ? "Enum、" : "");

            ResultRecoder.Append(type.IsPrimitive ? "Primitive、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }

        }
        /// <summary>
        /// 对FieldInfo进行安全分析
        /// </summary>
        /// <param name="info">FieldInfo实例</param>
        public void SecurityAnalysis(FieldInfo info)
        {
#if !NETSTANDARD1_3
            int i = ResultRecoder.Length;

            ResultRecoder.Append(info.IsSecurityCritical ? "SecurityCritical、" : "");

            ResultRecoder.Append(info.IsSecuritySafeCritical ? "SecuritySafeCritical、" : "");

            ResultRecoder.Append(info.IsSecurityTransparent ? "SecurityTransparent、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }
#endif


        }

        public void SetOperatorAnalysis(FieldInfo info)
        {
            try
            {
                object obj = Activator.CreateInstance(info.DeclaringType);
                info.SetValue(obj, "Test");
                ResultRecoder.Append("√");
            }
            catch (Exception ex)
            {
                ResultRecoder.Append($"X ({ex.Message})");
            }
        }
        public void GetOperatorAnalysis(FieldInfo info)
        {
            try
            {
                object obj = Activator.CreateInstance(info.DeclaringType);
                object result = info.GetValue(obj);
                if (result != null)
                {
                    ResultRecoder.Append("√");
                }
                else
                {
                    ResultRecoder.Append("X");
                }
            }
            catch (Exception ex)
            {
                ResultRecoder.Append($"X ({ex.Message})");
            }
        }

        #endregion

        #region 对PropertyInfo进行分析
        /// <summary>
        /// 对PropertyInfo进行访问级别分析
        /// </summary>
        /// <param name="info">PropertyInfo实例</param>
        /// <param name="IsSetter">获取MethodInfo类型 {true:Setter, false:Getter}</param>
        public void ProtectionAnalysis(PropertyInfo info, bool IsSetter = false)
        {
            int i = ResultRecoder.Length;

            MethodInfo methodInfo;
            if (IsSetter)
            {
                methodInfo = info.GetSetMethod(true);
            }
            else
            {
                methodInfo = info.GetGetMethod(true);
            }
            ResultRecoder.Append(methodInfo.IsPublic ? "Public、" : "");

            ResultRecoder.Append(methodInfo.IsPrivate ? "Private、" : "");

            ResultRecoder.Append(methodInfo.IsFamily ? "Protected、" : "");

            ResultRecoder.Append(methodInfo.IsAssembly ? "Internal、" : "");

            ResultRecoder.Append(methodInfo.IsFamilyOrAssembly ? "Protected Internal、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }

        }
        /// <summary>
        /// 对PropertyInfo进行特性分析
        /// </summary>
        /// <param name="info">PropertyInfo实例</param>
        /// <param name="IsSetter">获取MethodInfo类型 {true:Setter, false:Getter}</param>
        public void ParticularityAnalysis(PropertyInfo info, bool IsSetter = false)
        {
            int i = ResultRecoder.Length;
#if NETSTANDARD1_3
            TypeInfo type = info.PropertyType.GetTypeInfo();
#else
            Type type = info.PropertyType;
#endif

            MethodInfo methodInfo;
            if (IsSetter)
            {
                methodInfo = info.GetSetMethod(true);
            }
            else
            {
                methodInfo = info.GetGetMethod(true);
            }

            ResultRecoder.Append(type.IsGenericType ? "Generic、" : "");

            ResultRecoder.Append(methodInfo.IsStatic ? "Static、" : "");

            ResultRecoder.Append(type.IsAbstract ? "Abstract、" : "");

            ResultRecoder.Append(type.IsByRef ? "Ref、" : "");

            ResultRecoder.Append(type.IsNested ? "Public Nested、" : "");

            ResultRecoder.Append(type.IsNestedFamily ? "Protected Nested、" : "");

            ResultRecoder.Append(type.IsNestedPrivate ? "Private Nested、" : "");

            //ResultRecoder.Append(type.IsSealed ? "Sealed、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }
        }

        /// <summary>
        /// 对PropertyInfo进行类型分析
        /// </summary>
        /// <param name="info">PropertyInfo实例</param>
        public void KindsAnalysis(PropertyInfo info)
        {
            int i = ResultRecoder.Length;

#if NETSTANDARD1_3
            TypeInfo type = info.PropertyType.GetTypeInfo();
#else
            Type type = info.PropertyType;
#endif

            ResultRecoder.Append(type.IsClass ? "Class、" : "");

            ResultRecoder.Append(type.IsAnsiClass ? "AnsiClass、" : "");

            ResultRecoder.Append(type.IsInterface ? "Interface、" : "");

            ResultRecoder.Append(type.IsArray ? "Array、" : "");

            ResultRecoder.Append(type.IsEnum ? "Enum、" : "");

            ResultRecoder.Append(type.IsPrimitive ? "Primitive、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }
        }
        /// <summary>
        /// 对PropertyInfo进行安全性分析
        /// </summary>
        /// <param name="info">PropertyInfo实例</param>
        /// <param name="IsSetter">获取MethodInfo类型 {true:Setter, false:Getter}</param>
        public void SecurityAnalysis(PropertyInfo info, bool IsSetter = false)
        {
            int i = ResultRecoder.Length;

            MethodInfo methodInfo;

            if (IsSetter)
            {
                methodInfo = info.GetSetMethod(true);
            }
            else
            {
                methodInfo = info.GetGetMethod(true);
            }

            SecurityAnalysis(methodInfo);
        }

        /// <summary>
        /// 测试Set方法
        /// </summary>
        /// <param name="info">PropertyInfo实例</param>
        public void SetOperatorAnalysis(PropertyInfo info)
        {
            try
            {
                object obj = Activator.CreateInstance(info.DeclaringType);
                info.SetValue(obj, "Test");
                ResultRecoder.Append("√");
            }
            catch (Exception ex)
            {
                ResultRecoder.Append($"X ({ex.Message})");
            }
        }
        /// <summary>
        /// 测试Get方法
        /// </summary>
        /// <param name="info">PropertyInfo实例</param>
        public void GetOperatorAnalysis(PropertyInfo info)
        {
            try
            {
                object obj = Activator.CreateInstance(info.DeclaringType);
                object result = info.GetValue(obj);
                if (result != null)
                {
                    ResultRecoder.Append("√");
                }
                else
                {
                    ResultRecoder.Append("X");
                }
            }
            catch (Exception ex)
            {
                ResultRecoder.Append($"X ({ex.Message})");
            }
        }
        #endregion

        #region 对MethodInfo进行分析

        /// <summary>
        /// 对MethodInfo进行访问级别分析
        /// </summary>
        /// <param name="info">MethodInfo实例</param>
        public void ProtectionAnalysis(MethodInfo info)
        {
            int i = ResultRecoder.Length;

            ResultRecoder.Append(info.IsPublic ? "Public、" : "");

            ResultRecoder.Append(info.IsPrivate ? "Private、" : "");

            ResultRecoder.Append(info.IsFamily ? "Protected、" : "");

            ResultRecoder.Append(info.IsAssembly ? "Internal、" : "");

            ResultRecoder.Append(info.IsFamilyOrAssembly ? "Protected Internal、" : "");

            if (i != ResultRecoder.Length)
            {
                if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }
            }
        }
        /// <summary>
        /// 对MethodInfo进行特性分析
        /// </summary>
        /// <param name="info">MethodInfo实例</param>
        public void ParticularityAnalysis(MethodInfo info)
        {
            int i = ResultRecoder.Length;
#if NETSTANDARD1_3
            TypeInfo type = info.ReturnType.GetTypeInfo();
#else
            Type type = info.ReturnType;
#endif

            ResultRecoder.Append(type.IsGenericType ? "Generic、" : "");

            ResultRecoder.Append(info.IsStatic ? "Static、" : "");

            ResultRecoder.Append(type.IsAbstract ? "Abstract、" : "");

            ResultRecoder.Append(type.IsByRef ? "Ref、" : "");

            ResultRecoder.Append(type.IsNested ? "Public Nested、" : "");

            ResultRecoder.Append(type.IsNestedFamily ? "Protected Nested、" : "");

            ResultRecoder.Append(type.IsNestedPrivate ? "Private Nested、" : "");

            ResultRecoder.Append(type.IsSealed ? "Sealed、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }

            if (i != ResultRecoder.Length) { if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); } }
        }

        /// <summary>
        /// 对MethodInfo进行类型分析
        /// </summary>
        /// <param name="info">MethodInfo实例</param>
        public void KindsAnalysis(MethodInfo info)
        {
            int i = ResultRecoder.Length;

#if NETSTANDARD1_3
            TypeInfo type = info.ReturnType.GetTypeInfo();
#else
            Type type = info.ReturnType;
#endif

            ResultRecoder.Append(type.IsClass ? "Class、" : "");

            ResultRecoder.Append(type.IsAnsiClass ? "AnsiClass、" : "");

            ResultRecoder.Append(type.IsInterface ? "Interface、" : "");

            ResultRecoder.Append(type.IsArray ? "Array、" : "");

            ResultRecoder.Append(type.IsEnum ? "Enum、" : "");

            ResultRecoder.Append(type.IsPrimitive ? "Primitive、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }
        }

        /// <summary>
        /// 对MethodInfo实例进行安全性分析
        /// </summary>
        /// <param name="info">MethodInfo实例</param>
        public void SecurityAnalysis(MethodInfo info)
        {
#if !NETSTANDARD1_3
            int i = ResultRecoder.Length;

            ResultRecoder.Append(info.IsSecurityCritical ? "SecurityCritical、" : "");

            ResultRecoder.Append(info.IsSecuritySafeCritical ? "SecuritySafeCritical、" : "");

            ResultRecoder.Append(info.IsSecurityTransparent ? "SecurityTransparent、" : "");

            if (i != ResultRecoder.Length) { ResultRecoder.RemoveLastest(1); }
#endif
            
        }
        #endregion


        #region Emit可行性测试
        public void EmitAnalysis(FieldInfo info)
        {
#if NETSTANDARD1_3
            TypeInfo info_type = info.DeclaringType.GetTypeInfo();
#else
            Type info_type = info.DeclaringType;
#endif
            if (!info_type.IsClass)
            {

                if (info.IsStatic)
                {
                    try
                    {
                        DynamicMethod method = new DynamicMethod("Field" + Guid.NewGuid().ToString(), typeof(string), new Type[] { });
                        ILGenerator il = method.GetILGenerator();
                        il.Emit(OpCodes.Ldstr, "Emit");
                        il.Emit(OpCodes.Stsfld, info);
                        il.Emit(OpCodes.Ldsflda, info);
                        il.Emit(OpCodes.Ret);
                        Func<string> action = (Func<string>)(method.CreateDelegate(typeof(Func<string>)));
                        string result = action();
                        if (result == "Emit")
                        {
                            ResultRecoder.Append("√");
                        }
                        else
                        {
                            ResultRecoder.Append("X");
                        }

                    }
                    catch (Exception ex)
                    {
                        ResultRecoder.Append($"X ({ex.Message})");
                    }
                }
                else
                {
                    try
                    {
                        DynamicMethod method = new DynamicMethod("Field" + Guid.NewGuid().ToString(), typeof(string), new Type[] { });
                        ILGenerator il = method.GetILGenerator();
#if NETSTANDARD1_3
                        ConstructorInfo ctor = info.DeclaringType.GetConstructor(null);
#else
             ConstructorInfo ctor = info.DeclaringType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
#endif

                        il.Emit(OpCodes.Initobj, info.DeclaringType);
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Ldstr, "Emit");
                        il.Emit(OpCodes.Stfld, info);
                        il.Emit(OpCodes.Ldflda, info);
                        il.Emit(OpCodes.Ret);
                        Func<string> action = (Func<string>)(method.CreateDelegate(typeof(Func<string>)));

                        string result = action();

                        if (result == "Emit")
                        {
                            ResultRecoder.Append("√");
                        }
                        else
                        {
                            ResultRecoder.Append("X");
                        }
                    }
                    catch (Exception ex)
                    {
                        ResultRecoder.Append($"X ({ex.Message})");
                    }

                }
            }
            else
            {
                if (info.IsStatic)
                {
                    try
                    {
                        DynamicMethod method = new DynamicMethod("Field" + Guid.NewGuid().ToString(), typeof(string), new Type[] { });
                        ILGenerator il = method.GetILGenerator();
                        il.Emit(OpCodes.Ldstr, "Emit");
                        il.Emit(OpCodes.Stsfld, info);
                        il.Emit(OpCodes.Ldsfld, info);
                        il.Emit(OpCodes.Ret);

                        Func<string> action = (Func<string>)(method.CreateDelegate(typeof(Func<string>)));

                        string result = action();
                        if (result == "Emit")
                        {
                            ResultRecoder.Append("√");
                        }
                        else
                        {
                            ResultRecoder.Append("X");
                        }

                    }
                    catch (Exception ex)
                    {
                        ResultRecoder.Append($"X ({ex.Message})");
                    }
                }
                else
                {
                    try
                    {
                        DynamicMethod method = new DynamicMethod("Field" + Guid.NewGuid().ToString(), typeof(string), new Type[] { });
                        ILGenerator il = method.GetILGenerator();
#if NETSTANDARD1_3
                        ConstructorInfo ctor = info.DeclaringType.GetConstructor(null);
#else
                        ConstructorInfo ctor = info.DeclaringType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
#endif
                        il.Emit(OpCodes.Newobj, ctor);
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Ldstr, "Emit");
                        il.Emit(OpCodes.Stfld, info);
                        il.Emit(OpCodes.Ldfld, info);
                        il.Emit(OpCodes.Ret);
                        Func<string> action = (Func<string>)(method.CreateDelegate(typeof(Func<string>)));

                        string result = action();
                        if (result == "Emit")
                        {
                            ResultRecoder.Append("√");
                        }
                        else
                        {
                            ResultRecoder.Append("X");
                        }
                    }
                    catch (Exception ex)
                    {
                        ResultRecoder.Append($"X ({ex.Message})");
                    }

                }
            }
        }
        public void EmitAnalysis(PropertyInfo info)
        {
            MethodInfo getter = info.GetGetMethod(true);
            MethodInfo setter = info.SetMethod;
#if NETSTANDARD1_3
            TypeInfo info_type = info.DeclaringType.GetTypeInfo();
#else
            Type info_type = info.DeclaringType;
#endif
            if (!info_type.IsClass)
            {

                if (setter.IsStatic)
                {
                    try
                    {
                        DynamicMethod method = new DynamicMethod("Property" + Guid.NewGuid().ToString(), typeof(string), new Type[] { });
                        ILGenerator il = method.GetILGenerator();
                        il.Emit(OpCodes.Ldstr, "Emit");
                        il.Emit(OpCodes.Call, setter);
                        il.Emit(OpCodes.Call, getter);
                        il.Emit(OpCodes.Ret);
                        Func<string> action = (Func<string>)(method.CreateDelegate(typeof(Func<string>)));
                        string result = action();
                        if (result == "Emit")
                        {
                            ResultRecoder.Append("√");
                        }
                        else
                        {
                            ResultRecoder.Append("X");
                        }

                    }
                    catch (Exception ex)
                    {
                        ResultRecoder.Append($"X ({ex.Message})");
                    }
                }
                else
                {
                    try
                    {
                        DynamicMethod method = new DynamicMethod("Property" + Guid.NewGuid().ToString(), typeof(string), new Type[] { });
                        ILGenerator il = method.GetILGenerator();
#if NETSTANDARD1_3
                        ConstructorInfo ctor = info.DeclaringType.GetConstructor(null);
#else
             ConstructorInfo ctor = info.DeclaringType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
#endif
                        //ConstructorInfo ctor = info.DeclaringType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
                        il.Emit(OpCodes.Initobj, info.DeclaringType);
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Ldstr, "Emit");
                        il.Emit(OpCodes.Callvirt, setter);
                        il.Emit(OpCodes.Callvirt, getter);
                        il.Emit(OpCodes.Ret);
                        Func<string> action = (Func<string>)(method.CreateDelegate(typeof(Func<string>)));

                        string result = action();
                        if (result == "Emit")
                        {
                            ResultRecoder.Append("√");
                        }
                        else
                        {
                            ResultRecoder.Append("X");
                        }
                    }
                    catch (Exception ex)
                    {
                        ResultRecoder.Append($"X ({ex.Message})");
                    }

                }
            }
            else
            {
                if (setter.IsStatic)
                {
                    try
                    {
                        DynamicMethod method = new DynamicMethod("Property" + Guid.NewGuid().ToString(), typeof(string), new Type[] { });
                        ILGenerator il = method.GetILGenerator();
                        il.Emit(OpCodes.Ldstr, "Emit");
                        il.Emit(OpCodes.Call, setter);
                        il.Emit(OpCodes.Call, getter);
                        il.Emit(OpCodes.Ret);
                        Func<string> action = (Func<string>)(method.CreateDelegate(typeof(Func<string>)));
                        string result = action();
                        if (result == "Emit")
                        {
                            ResultRecoder.Append("√");
                        }
                        else
                        {
                            ResultRecoder.Append("X");
                        }

                    }
                    catch (Exception ex)
                    {
                        ResultRecoder.Append($"X ({ex.Message})");
                    }
                }
                else
                {
                    try
                    {
                        DynamicMethod method = new DynamicMethod("Property" + Guid.NewGuid().ToString(), typeof(string), new Type[] { });
                        ILGenerator il = method.GetILGenerator();
#if NETSTANDARD1_3
                        ConstructorInfo ctor = info.DeclaringType.GetConstructor(null);
#else
             ConstructorInfo ctor = info.DeclaringType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
#endif
                        //onstructorInfo ctor = info.DeclaringType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
                        il.Emit(OpCodes.Newobj, ctor);
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Ldstr, "Emit");
                        il.Emit(OpCodes.Callvirt, setter);
                        il.Emit(OpCodes.Callvirt, getter);
                        il.Emit(OpCodes.Ret);
                        Func<string> action = (Func<string>)(method.CreateDelegate(typeof(Func<string>)));
                        string result = action();
                        if (result == "Emit")
                        {
                            ResultRecoder.Append("√");
                        }
                        else
                        {
                            ResultRecoder.Append("X");
                        }
                    }
                    catch (Exception ex)
                    {
                        ResultRecoder.Append($"X ({ex.Message})");
                    }
                }
            }
        }
        #endregion
    }
}
