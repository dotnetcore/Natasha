using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Natasha
{


    /// <summary>
    /// 动态类的动态调用
    /// </summary>
    public class DynamicStaticOperator : DynamicOperatorBase
    {


        public static implicit operator DynamicStaticOperator(Type instance)
        {
            return new DynamicStaticOperator(instance);
        }




        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<DynamicOperatorBase>>> GetDynamicCache;
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<DynamicOperatorBase>>> SetDynamicCache;
        static DynamicStaticOperator()
        {
            GetDynamicCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<DynamicOperatorBase>>>();
            SetDynamicCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<DynamicOperatorBase>>>();
        }




        private readonly ConcurrentDictionary<string, Action<DynamicOperatorBase>> DynamicGet;
        private readonly ConcurrentDictionary<string, Action<DynamicOperatorBase>> DynamicSet;
        private readonly Type _type;
        public DynamicStaticOperator(Type instance = null)
        {
            _type = instance;
            InitType(_type);
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

                GetDynamicCache[type] = new ConcurrentDictionary<string, Action<DynamicOperatorBase>>();
                SetDynamicCache[type] = new ConcurrentDictionary<string, Action<DynamicOperatorBase>>();


                //动态函数-成员调用
                var members = type.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                for (int i = 0; i < members.Length; i++)
                {
                    if (members[i].MemberType == MemberTypes.Field)
                    {
                        FieldInfo info = (FieldInfo)members[i];
                        if (info.IsStatic)
                        {
                            GetDynamicCache[type][info.Name] = DynamicStaticMemberHelper.GetField(info);
                            SetDynamicCache[type][info.Name] = DynamicStaticMemberHelper.SetField(info);
                        }
                    }
                    else if (members[i].MemberType == MemberTypes.Property)
                    {
                        PropertyInfo info = (PropertyInfo)members[i];
                        if (info.GetGetMethod(true).IsStatic)
                        {
                            GetDynamicCache[type][info.Name] = DynamicStaticMemberHelper.GetProperty(info);
                            SetDynamicCache[type][info.Name] = DynamicStaticMemberHelper.SetProperty(info);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="name">成员名</param>
        /// <returns></returns>
        public override void Get(string name)
        {
            DynamicGet[name](this);
        }




        /// <summary>
        /// 设置成员信息
        /// </summary>
        /// <param name="name">成员名</param>
        /// <returns></returns>
        public override void Set(string name)
        {
            DynamicSet[name](this);
        }




        /// <summary>
        /// 获取指定类型的字段或者属性
        /// </summary>
        /// <typeparam name="TValue">需要获取的类型</typeparam>
        /// <param name="name">属性/字段名</param>
        /// <returns></returns>
        public override TValue Get<TValue>()
        {
            InternalStaticEntityCaller<TValue>.Init(_type);
            return InternalStaticEntityCaller<TValue>.GetterDelegateMapping[_type][_current_name]();
        }




        /// <summary>
        /// 赋值操作，给指定字段或者属性 赋值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="name">属性/字段名</param>
        /// <param name="value">值</param>
        public override void Set<TValue>(TValue value)
        {
            InternalStaticEntityCaller<TValue>.Init(_type);
            InternalStaticEntityCaller<TValue>.SetterDelegateMapping[_type][_current_name](value);
        }
    }

    public static class InternalStaticEntityCaller<T>
    {
        internal readonly static ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<T>>> SetterDelegateMapping;
        internal readonly static ConcurrentDictionary<Type, ConcurrentDictionary<string, Func<T>>> GetterDelegateMapping;
        static InternalStaticEntityCaller()
        {
            SetterDelegateMapping = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<T>>>();
            GetterDelegateMapping = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Func<T>>>();
        }

        public static void Init(Type type)
        {
            if (!GetterDelegateMapping.ContainsKey(type))
            {
                Type memberType = typeof(T);

                var fields = type.GetFields();
                for (int i = 0; i < fields.Length; i += 1)
                {
                    if (fields[i].FieldType == memberType)
                    {

                        SetterDelegateMapping[type][fields[i].Name] = FastMethodOperator
                            .New
                            .Param<T>("value")
                            .MethodBody($"{type.GetDevelopName()}.{fields[i].Name} = value;")
                            .Return()
                            .Complie<Action<T>>();

                        GetterDelegateMapping[type][fields[i].Name] = FastMethodOperator
                            .New
                            .MethodBody($"return {type.GetDevelopName()}.{fields[i].Name};")
                            .Return<T>()
                            .Complie<Func<T>>();

                    }
                }

                var props = type.GetProperties();
                for (int i = 0; i < props.Length; i += 1)
                {
                    if (props[i].PropertyType == memberType)
                    {

                        SetterDelegateMapping[type][props[i].Name] = FastMethodOperator
                            .New
                            .Param<T>("value")
                            .MethodBody($"{type.GetDevelopName()}.{props[i].Name} = value;")
                            .Return()
                            .Complie<Action<T>>();

                        GetterDelegateMapping[type][props[i].Name] = FastMethodOperator
                           .New
                           .MethodBody($"return {type.GetDevelopName()}.{props[i].Name};")
                           .Return<T>()
                           .Complie<Func<T>>();

                    }
                }
            }
        }

    }

    internal class DynamicStaticMemberHelper
    {


        internal static ConcurrentDictionary<Type, string> TypeMemberMapping;
        static DynamicStaticMemberHelper()
        {
            TypeMemberMapping = new ConcurrentDictionary<Type, string>();
            var infos = typeof(DynamicOperatorBase).GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < infos.Length; i++)
            {
                TypeMemberMapping[infos[i].FieldType] = infos[i].Name;
            }
        }


        public static Action<DynamicOperatorBase> SetField(FieldInfo info)
        {
            if (TypeMemberMapping.ContainsKey(info.FieldType))
            {
                Type type = info.DeclaringType;
                string body = $@"{type.GetDevelopName()}.{info.Name}=proxy.{TypeMemberMapping[info.FieldType]};";
                return GetStaticDelegate(type, info.FieldType, body);
            }
            return null;
        }


        public static Action<DynamicOperatorBase> GetField(FieldInfo info)
        {
            string body;
            if (TypeMemberMapping.ContainsKey(info.FieldType))
            {
                body = $@"proxy.{TypeMemberMapping[info.FieldType]}={info.DeclaringType.GetDevelopName()}.{info.Name};";
            }
            else
            {
                body = $@"DynamicOperator<{info.FieldType.GetDevelopName()}> {info.Name}temp = {info.DeclaringType.GetDevelopName()}.{info.Name}; proxy.OperatorValue={info.Name}temp;";
            }
            return GetStaticDelegate(info.DeclaringType, info.FieldType, body);
        }


        public static Action<DynamicOperatorBase> GetProperty(PropertyInfo info)
        {
            string body;
            if (TypeMemberMapping.ContainsKey(info.PropertyType))
            {
                body = $@"proxy.{TypeMemberMapping[info.PropertyType]}={info.DeclaringType.GetDevelopName()}.{info.Name};";
            }
            else
            {
                body = $@"DynamicOperator<{info.PropertyType.GetDevelopName()}> {info.Name}temp = {info.DeclaringType.GetDevelopName()}.{info.Name}; proxy.OperatorValue={info.Name}temp;";
            }
            return GetStaticDelegate(info.DeclaringType, info.PropertyType, body);
        }


        public static Action<DynamicOperatorBase> SetProperty(PropertyInfo info)
        {

            if (TypeMemberMapping.ContainsKey(info.PropertyType))
            {
                Type type = info.DeclaringType;
                string body = $@"{type.GetDevelopName()}.{info.Name}=proxy.{TypeMemberMapping[info.PropertyType]};";
                return GetStaticDelegate(type, info.PropertyType, body);
            }
            return null;

        }


        private static Action<DynamicOperatorBase> GetStaticDelegate(Type type, Type operatorType, string body)
        {
            return FastMethodOperator.New
                  .Using(type)
                  .Using(operatorType)
                  .Param<DynamicOperatorBase>("proxy")
                  .MethodBody(body)
                  .Return()
              .Complie<Action<DynamicOperatorBase>>();
        }
    }
}
