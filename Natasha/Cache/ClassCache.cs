using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Natasha.Cache
{
    //主要是针对类操作的一些缓存
    public static class ClassCache
    {
        public static Dictionary<string, ClassStruction> ClassInfoDict;
        public static Dictionary<string, Type> DynamicClassDict;
        public static Dictionary<Type, Func<object, bool>> CheckStructDict;
        public static Dictionary<Type, Action<ILGenerator>> ConstructorDict;

        public static HashSet<string> NoCloneTypes;

        public static MethodInfo StringCompare;
        public static MethodInfo ClassCompare;
        public static MethodInfo ClassHandle;
        public static MethodInfo FieldInfoGetter;
        public static MethodInfo FieldValueGetter;
        public static MethodInfo FieldValueSetter;
        public static MethodInfo PropertyInfoGetter;
        public static MethodInfo ProeprtyValueGetter;
        public static MethodInfo PropertyValueSetter;

        static ClassCache()
        {
            StringCompare = typeof(string).GetMethod("Equals", new Type[] { typeof(string), typeof(string) });
            ClassCompare = typeof(object).GetMethod("Equals", new Type[] { typeof(object), typeof(object) });
            ClassHandle = typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) });
            FieldInfoGetter = typeof(Type).GetMethod("GetField", new Type[] { typeof(string), typeof(BindingFlags) });
            FieldValueGetter = typeof(FieldInfo).GetMethod("GetValue", new Type[] { typeof(object) });
            FieldValueSetter = typeof(FieldInfo).GetMethod("SetValue", new Type[] { typeof(object), typeof(object) });
            PropertyInfoGetter = typeof(Type).GetMethod("GetProperty", new Type[] { typeof(string), typeof(BindingFlags) });
            ProeprtyValueGetter = typeof(PropertyInfo).GetMethod("GetValue", new Type[] { typeof(object) });
            PropertyValueSetter = typeof(PropertyInfo).GetMethod("SetValue", new Type[] { typeof(object), typeof(object) });
            NoCloneTypes = new HashSet<string>();
            ClassInfoDict = new Dictionary<string, ClassStruction>();
            DynamicClassDict = new Dictionary<string, Type>();
            CheckStructDict = new Dictionary<Type, Func<object, bool>>();
            ConstructorDict = new Dictionary<Type, Action<ILGenerator>>();

            FillNoCloneCollection();
        }


        public static void FillNoCloneCollection()
        {
            NoCloneTypes.Add("__DynamicallyInvokableAttribute");
            NoCloneTypes.Add("SecuritySafeCriticalAttribute");
            NoCloneTypes.Add("ReliabilityContractAttribute");
            NoCloneTypes.Add("NonVersionableAttribute");
            NoCloneTypes.Add("SecurityCriticalAttribute");
        }
        

        public static void SetConstructor(Type type,Action<ILGenerator> action)
        {
            ConstructorDict[type] = action;
        }
    }
}
