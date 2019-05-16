using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class DynamicInstance
    {
        public static ConcurrentDictionary<Type, Dictionary<string, Action<DynamicInstance, object>>> GetDynamicCache;
        public static ConcurrentDictionary<Type, Dictionary<string, Action<DynamicInstance, object>>> SetDynamicCache;
        private static ConcurrentDictionary<Type, string> TypeMemberMapping;
        public static ConcurrentDictionary<Type, Func<object>> CtorMapping;
        public Dictionary<string, Action<DynamicInstance, object>> DynamicGet;
        public Dictionary<string, Action<DynamicInstance, object>> DynamicSet;


        static DynamicInstance()
        {
            GetDynamicCache = new ConcurrentDictionary<Type, Dictionary<string, Action<DynamicInstance, object>>>();
            SetDynamicCache = new ConcurrentDictionary<Type, Dictionary<string, Action<DynamicInstance, object>>>();
            TypeMemberMapping = new ConcurrentDictionary<Type, string>();
            CtorMapping = new ConcurrentDictionary<Type, Func<object>>();
            FieldInfo[] infos = typeof(DynamicInstance).GetFields();
            for (int i = 0; i < infos.Length; i++)
            {
                TypeMemberMapping[infos[i].FieldType] = infos[i].Name;
            }
        }
        public DynamicInstance(Type type)
        {
            if (!GetDynamicCache.ContainsKey(type))
            {
                GetDynamicCache[type] = new Dictionary<string, Action<DynamicInstance, object>>();
                SetDynamicCache[type] = new Dictionary<string, Action<DynamicInstance, object>>();
                InitType(type);
            }
            _instance = CtorMapping[type]();
            DynamicGet = GetDynamicCache[type];
            DynamicSet = SetDynamicCache[type];
        }


        private object _instance;

        public DynamicInstance Load(object instance)
        {
            _instance = instance;
            return this;
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

        public static void InitType(Type type)
        {
            ScriptBuilder ctor = new ScriptBuilder();
            CtorMapping[type] = ctor
                .Namespace(type)
                .Namespace<object>()
                .Body($@"return new {type}();")
                .Return<object>()
                .Create<Func<object>>();


            var members = type.GetMembers();
            
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].MemberType == MemberTypes.Field)
                {
                    FieldInfo info = (FieldInfo)members[i];
                    ScriptBuilder getBuilder = new ScriptBuilder();
                    var delegateGetAction = getBuilder
                        .Namespace(type)
                        .Namespace(info.FieldType)
                        .Namespace<DynamicInstance>()
                        .Param<DynamicInstance>("proxy")
                        .Param<object>("instance")
                        .Body($@"proxy.{TypeMemberMapping[info.FieldType]}=(({type.Name})instance).{info.Name};")
                        .Return()
                        .Create<Action<DynamicInstance,object>>();
                    GetDynamicCache[type][info.Name] = delegateGetAction;


                    ScriptBuilder setBuilder = new ScriptBuilder();
                    var delegateSetAction = setBuilder
                        .Namespace(type)
                        .Namespace(info.FieldType)
                        .Namespace<DynamicInstance>()
                        .Param<DynamicInstance>("proxy")
                        .Param<object>("instance")
                        .Body($@"(({type.Name})instance).{info.Name}=proxy.{TypeMemberMapping[info.FieldType]};")
                        .Return()
                        .Create<Action<DynamicInstance, object>>();
                    SetDynamicCache[type][info.Name] = delegateSetAction;

                }
                else if (members[i].MemberType == MemberTypes.Property)
                {
                    PropertyInfo info = (PropertyInfo)members[i];
                    ScriptBuilder getBuilder = new ScriptBuilder();
                    var delegateGetAction = getBuilder
                        .Namespace(type)
                        .Namespace(info.PropertyType)
                        .Namespace<DynamicInstance>()
                        .Param<DynamicInstance>("proxy")
                        .Param<object>("instance")
                        .Body($@"proxy.{TypeMemberMapping[info.PropertyType]}=(({type.Name})instance).{info.Name};")
                        .Return()
                        .Create<Action<DynamicInstance, object>>();
                    GetDynamicCache[type][info.Name] = delegateGetAction;


                    ScriptBuilder setBuilder = new ScriptBuilder();
                    var delegateSetAction = setBuilder
                        .Namespace(type)
                        .Namespace(info.PropertyType)
                        .Namespace<DynamicInstance>()
                        .Param<DynamicInstance>("proxy")
                        .Param<object>("instance")
                        .Body($@"(({type.Name})instance).{info.Name}=proxy.{TypeMemberMapping[info.PropertyType]};")
                        .Return()
                        .Create<Action<DynamicInstance, object>>();
                    SetDynamicCache[type][info.Name] = delegateSetAction;
                }
            }
        }

        public DynamicInstance Get(string name)
        {
            DynamicGet[name](this,_instance);
            return this;
        }

        public DynamicInstance Set(string name)
        {
            DynamicSet[name](this, _instance);
            return this;
        }

    }

    
}
