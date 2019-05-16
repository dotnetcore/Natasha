using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class DynamicInstance<T>: DynamicInstance
    {
        public static ConcurrentDictionary<Type, Dictionary<string, Action<DynamicInstance, T>>> GetDynamicCache;
        public static ConcurrentDictionary<Type, Dictionary<string, Action<DynamicInstance, T>>> SetDynamicCache;
       
        public static ConcurrentDictionary<Type, Func<T>> CtorMapping;
        public Dictionary<string, Action<DynamicInstance, T>> DynamicGet;
        public Dictionary<string, Action<DynamicInstance, T>> DynamicSet;


        static DynamicInstance()
        {
            GetDynamicCache = new ConcurrentDictionary<Type, Dictionary<string, Action<DynamicInstance, T>>>();
            SetDynamicCache = new ConcurrentDictionary<Type, Dictionary<string, Action<DynamicInstance, T>>>();
            CtorMapping = new ConcurrentDictionary<Type, Func<T>>();
        }
        public DynamicInstance(Type type=null)
        {
            if (typeof(T) != typeof(object))
            {
                type = typeof(T);
            }
            if (!GetDynamicCache.ContainsKey(type))
            {
                GetDynamicCache[type] = new Dictionary<string, Action<DynamicInstance, T>>();
                SetDynamicCache[type] = new Dictionary<string, Action<DynamicInstance, T>>();
                InitType(type);
            }
            _instance = CtorMapping[type]();
            DynamicGet = GetDynamicCache[type];
            DynamicSet = SetDynamicCache[type];
        }


        private T _instance;

        public DynamicInstance<T> Load(T instance)
        {
            _instance = instance;
            return this;
        }


        public static void InitType(Type type)
        {
            ScriptBuilder ctor = new ScriptBuilder();
            if (typeof(T)==typeof(object))
            {
                ctor.Namespace(type);
            }
            CtorMapping[type] = ctor
                .Namespace<T>()
                .Body($@"return new {type.Name}();")
                .Return<T>()
                .Create<Func<T>>();


            var members = type.GetMembers();
            string body = "";
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].MemberType == MemberTypes.Field)
                {
                    FieldInfo info = (FieldInfo)members[i];
                    ScriptBuilder getBuilder = new ScriptBuilder();
                    if (typeof(T) == typeof(object))
                    {
                        body = $@"proxy.{TypeMemberMapping[info.FieldType]}=(({type.Name})instance).{info.Name};";
                    }
                    else
                    {
                        body = $@"proxy.{TypeMemberMapping[info.FieldType]}=instance.{info.Name};";
                    }
                    var delegateGetAction = getBuilder
                        .Namespace(type)
                        .Namespace(info.FieldType)
                        .Namespace<DynamicInstance>()
                        .Param<DynamicInstance>("proxy")
                        .Param<T>("instance")
                        .Body(body)
                        .Return()
                        .Create<Action<DynamicInstance, T>>();
                    GetDynamicCache[type][info.Name] = delegateGetAction;


                    ScriptBuilder setBuilder = new ScriptBuilder();
                    var delegateSetAction = setBuilder
                        .Namespace(type)
                        .Namespace(info.FieldType)
                        .Namespace<DynamicInstance>()
                        .Param<DynamicInstance>("proxy")
                        .Param<T>("instance")
                        .Body($@"(({type.Name})instance).{info.Name}=proxy.{TypeMemberMapping[info.FieldType]};")
                        .Return()
                        .Create<Action<DynamicInstance, T>>();
                    SetDynamicCache[type][info.Name] = delegateSetAction;

                }
                else if (members[i].MemberType == MemberTypes.Property)
                {
                    PropertyInfo info = (PropertyInfo)members[i];
                    ScriptBuilder getBuilder = new ScriptBuilder();
                   
                    if (typeof(T)==typeof(object))
                    {
                        body = $@"proxy.{TypeMemberMapping[info.PropertyType]}=(({type.Name})instance).{info.Name};";
                    }
                    else
                    {
                        body = $@"proxy.{TypeMemberMapping[info.PropertyType]}=instance.{info.Name};";
                    }
                    var delegateGetAction = getBuilder
                        .Namespace(type)
                        .Namespace(info.PropertyType)
                        .Namespace<DynamicInstance>()
                        .Param<DynamicInstance>("proxy")
                        .Param<T>("instance")
                        .Body(body)
                        .Return()
                        .Create<Action<DynamicInstance, T>>();
                    GetDynamicCache[type][info.Name] = delegateGetAction;


                    ScriptBuilder setBuilder = new ScriptBuilder();
                    var delegateSetAction = setBuilder
                        .Namespace(type)
                        .Namespace(info.PropertyType)
                        .Namespace<DynamicInstance>()
                        .Param<DynamicInstance>("proxy")
                        .Param<T>("instance")
                        .Body($@"(({type.Name})instance).{info.Name}=proxy.{TypeMemberMapping[info.PropertyType]};")
                        .Return()
                        .Create<Action<DynamicInstance, T>>();
                    SetDynamicCache[type][info.Name] = delegateSetAction;
                }
            }
        }

        public DynamicInstance<T> Get(string name)
        {
            DynamicGet[name](this, _instance);
            return this;
        }

        public DynamicInstance<T> Set(string name)
        {
            DynamicSet[name](this, _instance);
            return this;
        }

    }
    public class DynamicInstance
    {
        internal static ConcurrentDictionary<Type, string> TypeMemberMapping;
        static DynamicInstance()
        {
            TypeMemberMapping = new ConcurrentDictionary<Type, string>();
            FieldInfo[] infos = typeof(DynamicInstance).GetFields();
            for (int i = 0; i < infos.Length; i++)
            {
                TypeMemberMapping[infos[i].FieldType] = infos[i].Name;
            }
        }

        public bool BoolValue;
        public byte ByteValue;
        public sbyte SByteValue;
        public short ShortValue;
        public ushort UShortValue;
        public int IntValue;
        public uint UIntValue;
        public long LongValue;
        public ulong ULongValue;

        public float FloatValue;
        public double DoubleValue;
        public decimal DecimalValue;
        
        public string StringValue;
        public DateTime DateTimeValue;

     
    }

    
}
