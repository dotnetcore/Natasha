using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Natasha.Cache
{
    //主要是针对类操作的一些缓存
    public delegate bool CheckStructDelegate(object value);


    public static class ClassCache
    {
        private static readonly object CacheLock = new object();
        public static ConcurrentDictionary<string, ClassStruction> ClassInfoDict;
        public static ConcurrentDictionary<string, Type> DynamicClassDict;
        public static ConcurrentDictionary<Type, CheckStructDelegate> CheckStructDict;
        public static ConcurrentDictionary<Type, Action<ILGenerator>> ConstructorDict;

        public static HashSet<string> NoCloneTypes;

        public static MethodInfo StringCompare;
        public static MethodInfo StringJoin;
        public static MethodInfo ClassCompare;
        public static MethodInfo ClassHandle;
        public static MethodInfo FieldInfoGetter;
        public static MethodInfo FieldValueGetter;
        public static MethodInfo FieldValueSetter;
        public static MethodInfo PropertyInfoGetter;
        public static MethodInfo PropertyValueGetter;
        public static MethodInfo PropertyValueSetter;
        public static MethodInfo DelegateMethod;

        static ClassCache()
        {
            if (ClassInfoDict==null)
            {
                lock (CacheLock)
                {
                    if (ClassInfoDict==null)
                    {
                        StringCompare = typeof(string).GetMethod("Equals", new Type[] { typeof(string), typeof(string) });
                        ClassCompare = typeof(object).GetMethod("Equals", new Type[] { typeof(object), typeof(object) });
                        ClassHandle = typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) });
                        FieldInfoGetter = typeof(Type).GetMethod("GetField", new Type[] { typeof(string), typeof(BindingFlags) });
                        FieldValueGetter = typeof(FieldInfo).GetMethod("GetValue", new Type[] { typeof(object) });
                        FieldValueSetter = typeof(FieldInfo).GetMethod("SetValue", new Type[] { typeof(object), typeof(object) });
                        PropertyInfoGetter = typeof(Type).GetMethod("GetProperty", new Type[] { typeof(string), typeof(BindingFlags) });
                        PropertyValueGetter = typeof(PropertyInfo).GetMethod("GetValue", new Type[] { typeof(object) });
                        PropertyValueSetter = typeof(PropertyInfo).GetMethod("SetValue", new Type[] { typeof(object), typeof(object) });
                        StringJoin = typeof(string).GetMethod("Concat", new Type[] { typeof(string), typeof(string) });
                        NoCloneTypes = new HashSet<string>();
                        ClassInfoDict = new ConcurrentDictionary<string, ClassStruction>();
                        DynamicClassDict = new ConcurrentDictionary<string, Type>();
                        CheckStructDict = new ConcurrentDictionary<Type, CheckStructDelegate>();
                        ConstructorDict = new ConcurrentDictionary<Type, Action<ILGenerator>>();

                        FillNoCloneCollection();
                    }
                }
            }
        }


        public static void FillNoCloneCollection()
        {
            NoCloneTypes.Add("__DynamicallyInvokableAttribute");
            NoCloneTypes.Add("SecuritySafeCriticalAttribute");
            NoCloneTypes.Add("ReliabilityContractAttribute");
            NoCloneTypes.Add("NonVersionableAttribute");
            NoCloneTypes.Add("SecurityCriticalAttribute");
            NoCloneTypes.Add("ObsoleteAttribute");
            NoCloneTypes.Add("CompilerGeneratedAttribute");
        }
        

        public static void SetConstructor(Type type,Action<ILGenerator> action)
        {
            ConstructorDict[type] = action;
        }

        public static void Initialize()
        {

        }
    }
}
