using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha
{
    /// <summary>
    /// 运行时动态操作类
    /// </summary>
    /// <typeparam name="T">运行时类型</typeparam>
    public class DynamicOperator<T> : DynamicOperatorBase where T : class
    {


        public static implicit operator DynamicOperator<T>(T instance)
        {
            return new DynamicOperator<T>(instance);
        }


        private static readonly Func<T> CtorDelegate;
        private static readonly ConcurrentDictionary<string, Action<DynamicOperatorBase, T>> DynamicGet;
        private static readonly ConcurrentDictionary<string, Action<DynamicOperatorBase, T>> DynamicSet;


        static DynamicOperator()
        {
            DynamicGet = new ConcurrentDictionary<string, Action<DynamicOperatorBase, T>>();
            DynamicSet = new ConcurrentDictionary<string, Action<DynamicOperatorBase, T>>();
            CtorDelegate = CtorOperator.NewDelegate<T>();
            InitType(typeof(T));
        }




        /// <summary>
        /// 初始化方法
        /// </summary>
        /// <param name="type"></param>
        public static void InitType(Type type)
        {
            //动态函数-成员调用
            var members = type.GetMembers();
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].MemberType == MemberTypes.Field)
                {

                    FieldInfo info = (FieldInfo)members[i];
                    DynamicGet[info.Name] = DynamicMemberHelper.GetField<T>(info);
                    DynamicSet[info.Name] = DynamicMemberHelper.SetField<T>(info);

                }
                else if (members[i].MemberType == MemberTypes.Property)
                {

                    PropertyInfo info = (PropertyInfo)members[i];
                    DynamicGet[info.Name] = DynamicMemberHelper.GetProperty<T>(info);
                    DynamicSet[info.Name] = DynamicMemberHelper.SetProperty<T>(info);

                }
            }

        }





        private T _instance;
        public DynamicOperator(T instance = null)
        {
            if (instance == null)
            {
                _instance = CtorDelegate();
            }
            else
            {
                _instance = instance;
            }
        }




        /// <summary>
        /// 更换当前实例对象
        /// </summary>
        /// <param name="instance">新实例</param>
        /// <returns></returns>
        public DynamicOperator<T> Change(T newInstance)
        {
            _instance = newInstance;
            return this;
        }




        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="name">成员名</param>
        /// <returns></returns>
        public override void Get(string name)
        {
            DynamicGet[name](this, _instance);
        }





        /// <summary>
        /// 设置成员信息
        /// </summary>
        /// <param name="name">成员名</param>
        /// <returns></returns>
        public override void Set(string name)
        {
            DynamicSet[name](this, _instance);
        }
    }




    /// <summary>
    /// 动态类的动态调用
    /// </summary>
    public class DynamicOperator : DynamicOperatorBase
    {


        public static implicit operator DynamicOperator(Type instance)
        {
            return new DynamicOperator(instance);
        }




        private static readonly ConcurrentDictionary<Type, Dictionary<string, Action<DynamicOperatorBase, object>>> GetDynamicCache;
        private static readonly ConcurrentDictionary<Type, Dictionary<string, Action<DynamicOperatorBase, object>>> SetDynamicCache;
        private static readonly ConcurrentDictionary<Type, Func<object>> CtorMapping;
        static DynamicOperator()
        {
            GetDynamicCache = new ConcurrentDictionary<Type, Dictionary<string, Action<DynamicOperatorBase, object>>>();
            SetDynamicCache = new ConcurrentDictionary<Type, Dictionary<string, Action<DynamicOperatorBase, object>>>();
            CtorMapping = new ConcurrentDictionary<Type, Func<object>>();
        }




        private readonly Dictionary<string, Action<DynamicOperatorBase, object>> DynamicGet;
        private readonly Dictionary<string, Action<DynamicOperatorBase, object>> DynamicSet;
        private readonly Type _type;
        public DynamicOperator(object instance = null)
        {
            if (instance is Type)
            {
                _type = (Type)instance;
                InitType(_type);
                _instance = CtorMapping[_type]();
            }
            else
            {
                _instance = instance;
                _type = instance.GetType();
                InitType(_type);
            }
            DynamicGet = GetDynamicCache[_type];
            DynamicSet = SetDynamicCache[_type];
        }




        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <param name="type">类型</param>
        private void InitType(Type type)
        {
            if (!GetDynamicCache.ContainsKey(type))
            {

                GetDynamicCache[type] = new Dictionary<string, Action<DynamicOperatorBase, object>>();
                SetDynamicCache[type] = new Dictionary<string, Action<DynamicOperatorBase, object>>();


                //动态函数-实例的创建
                CtorMapping[type] = CtorOperator.NewDelegate<object>(type);


                //动态函数-成员调用
                var members = type.GetMembers();
                for (int i = 0; i < members.Length; i++)
                {
                    if (members[i].MemberType == MemberTypes.Field)
                    {
                        FieldInfo info = (FieldInfo)members[i];

                        GetDynamicCache[type][info.Name] = DynamicMemberHelper.GetField<object>(info);
                        SetDynamicCache[type][info.Name] = DynamicMemberHelper.SetField<object>(info);

                    }
                    else if (members[i].MemberType == MemberTypes.Property)
                    {
                        PropertyInfo info = (PropertyInfo)members[i];

                        GetDynamicCache[type][info.Name] = DynamicMemberHelper.GetProperty<object>(info);
                        SetDynamicCache[type][info.Name] = DynamicMemberHelper.SetProperty<object>(info);
                    }
                }
            }
        }




        private object _instance;
        /// <summary>
        /// 更换当前实例对象
        /// </summary>
        /// <param name="instance">新实例</param>
        /// <returns></returns>
        public DynamicOperator Change(object newInstance)
        {
            _instance = newInstance;
            return this;
        }




        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="name">成员名</param>
        /// <returns></returns>
        public override void Get(string name)
        {
            DynamicGet[name](this, _instance);
        }




        /// <summary>
        /// 设置成员信息
        /// </summary>
        /// <param name="name">成员名</param>
        /// <returns></returns>
        public override void Set(string name)
        {
            DynamicSet[name](this, _instance);
        }
    }

    public abstract class DynamicOperatorBase
    {


        private string _current_name;
        public DynamicOperatorBase this[string key]
        {
            get
            {
                _current_name = key;
                return this;
            }
        }


        public bool _bool;
        public bool BoolValue
        {
            get
            {
                Get(_current_name);
                return _bool;
            }
            set
            {
                _bool = value;
                Set(_current_name);
            }
        }


        public byte _byte;
        public byte ByteValue
        {
            get
            {
                Get(_current_name);
                return _byte;
            }
            set
            {
                _byte = value;
                Set(_current_name);
            }
        }


        public sbyte _sbyte;
        public sbyte SByteValue
        {
            get
            {
                Get(_current_name);
                return _sbyte;
            }
            set
            {
                _sbyte = value;
                Set(_current_name);
            }
        }


        public short _short;
        public short ShortValue
        {
            get
            {
                Get(_current_name);
                return _short;
            }
            set
            {
                _short = value;
                Set(_current_name);
            }
        }


        public ushort _ushort;
        public ushort UShortValue
        {
            get
            {
                Get(_current_name);
                return _ushort;
            }
            set
            {
                _ushort = value;
                Set(_current_name);
            }
        }


        public int _int;
        public int IntValue
        {
            get
            {
                Get(_current_name);
                return _int;
            }
            set
            {
                _int = value;
                Set(_current_name);
            }
        }


        public uint _uint;
        public uint UIntValue
        {
            get
            {
                Get(_current_name);
                return _uint;
            }
            set
            {
                _uint = value;
                Set(_current_name);
            }
        }


        public long _long;
        public long LongValue
        {
            get
            {
                Get(_current_name);
                return _long;
            }
            set
            {
                _long = value;
                Set(_current_name);
            }
        }


        public ulong _ulong;
        public ulong ULongValue
        {
            get
            {
                Get(_current_name);
                return _ulong;
            }
            set
            {
                _ulong = value;
                Set(_current_name);
            }
        }


        public float _float;
        public float FloatValue
        {
            get
            {
                Get(_current_name);
                return _float;
            }
            set
            {
                _float = value;
                Set(_current_name);
            }
        }


        public double _double;
        public double DoubleValue
        {
            get
            {
                Get(_current_name);
                return _double;
            }
            set
            {
                _double = value;
                Set(_current_name);
            }
        }


        public decimal _decimal;
        public decimal DecimalValue
        {
            get
            {
                Get(_current_name);
                return _decimal;
            }
            set
            {
                _decimal = value;
                Set(_current_name);
            }
        }


        public string _string;
        public string StringValue
        {
            get
            {
                Get(_current_name);
                return _string;
            }
            set
            {
                _string = value;
                Set(_current_name);
            }
        }


        public DateTime _DateTime;
        public DateTime DateTimeValue
        {
            get
            {
                Get(_current_name);
                return _DateTime;
            }
            set
            {
                _DateTime = value;
                Set(_current_name);
            }
        }


        public virtual void Set(string name)
        {
        }
        public virtual void Get(string name)
        {
        }
    }





    internal class DynamicMemberHelper
    {


        internal static ConcurrentDictionary<Type, string> TypeMemberMapping;
        static DynamicMemberHelper()
        {
            TypeMemberMapping = new ConcurrentDictionary<Type, string>();
            var infos = typeof(DynamicOperatorBase).GetFields();
            for (int i = 0; i < infos.Length; i++)
            {
                TypeMemberMapping[infos[i].FieldType] = infos[i].Name;
            }
        }


        public static Action<DynamicOperatorBase, T> SetField<T>(FieldInfo info)
        {

            string body = "";
            Type type = null;


            if (typeof(T) == typeof(object))
            {
                type = info.DeclaringType;
                body = $@"(({type.Name})instance).{info.Name}=proxy.{TypeMemberMapping[info.FieldType]};";
            }
            else
            {
                type = typeof(T);
                body = $@"instance.{info.Name}=proxy.{TypeMemberMapping[info.FieldType]};";
            }

            return GetDelegate<T>(type, info.FieldType, body);
        }


        public static Action<DynamicOperatorBase, T> GetField<T>(FieldInfo info)
        {

            string body = "";
            Type type = null;


            if (typeof(T) == typeof(object))
            {
                type = info.DeclaringType;
                body = $@"proxy.{TypeMemberMapping[info.FieldType]}=(({type.Name})instance).{info.Name};";
            }
            else
            {
                type = typeof(T);
                body = $@"proxy.{TypeMemberMapping[info.FieldType]}=instance.{info.Name};";
            }


            return GetDelegate<T>(type, info.FieldType, body);
        }


        public static Action<DynamicOperatorBase, T> GetProperty<T>(PropertyInfo info)
        {
            string body = "";
            Type type = null;


            if (typeof(T) == typeof(object))
            {
                type = info.DeclaringType;
                body = $@"proxy.{TypeMemberMapping[info.PropertyType]}=(({type.Name})instance).{info.Name};";
            }
            else
            {
                type = typeof(T);
                body = $@"proxy.{TypeMemberMapping[info.PropertyType]}=instance.{info.Name};";
            }

            return GetDelegate<T>(type, info.PropertyType, body);
        }


        public static Action<DynamicOperatorBase, T> SetProperty<T>(PropertyInfo info)
        {

            string body = "";
            Type type = null;


            if (typeof(T) == typeof(object))
            {
                type = info.DeclaringType;
                body = $@"(({type.Name})instance).{info.Name}=proxy.{TypeMemberMapping[info.PropertyType]};";
            }
            else
            {
                type = typeof(T);
                body = $@"instance.{info.Name}=proxy.{TypeMemberMapping[info.PropertyType]};";
            }


            return GetDelegate<T>(type,info.PropertyType,body);
        }


        private static Action<DynamicOperatorBase, T> GetDelegate<T>(Type type,Type operatorType,string body)
        {
            return FastMethodOperator.New
                  .Using(type)
                  .Using(operatorType)
                  .Param<DynamicOperatorBase>("proxy")
                  .Param<T>("instance")
                  .MethodBody(body)
                  .Return()
              .Complie<Action<DynamicOperatorBase, T>>();
        }
    }
}
