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
            CtorDelegate = CtorBuilder.NewDelegate<T>();
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


        /// <summary>
        /// 获取指定类型的字段或者属性
        /// </summary>
        /// <typeparam name="TValue">需要获取的类型</typeparam>
        /// <param name="name">属性/字段名</param>
        /// <returns></returns>
        public override TValue Get<TValue>()
        {
            return InternalEntityCaller<T, TValue>.GetterDelegateMapping[_current_name](_instance);
        }


        /// <summary>
        /// 赋值操作，给指定字段或者属性 赋值
        /// </summary>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="name">属性/字段名</param>
        /// <param name="value">值</param>
        public override void Set<TValue>(TValue value)
        {
            InternalEntityCaller<T, TValue>.SetterDelegateMapping[_current_name](_instance, value);
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




        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<DynamicOperatorBase, object>>> GetDynamicCache;
        private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<DynamicOperatorBase, object>>> SetDynamicCache;
        private static readonly ConcurrentDictionary<Type, Func<object>> CtorMapping;
        private static readonly ConcurrentDictionary<Type, Func<DynamicOperatorBase>> OperatorMapping;
        static DynamicOperator()
        {
            GetDynamicCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<DynamicOperatorBase, object>>>();
            SetDynamicCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Action<DynamicOperatorBase, object>>>();
            CtorMapping = new ConcurrentDictionary<Type, Func<object>>();
            OperatorMapping = new ConcurrentDictionary<Type, Func<DynamicOperatorBase>>();
        }




        private readonly ConcurrentDictionary<string, Action<DynamicOperatorBase, object>> DynamicGet;
        private readonly ConcurrentDictionary<string, Action<DynamicOperatorBase, object>> DynamicSet;
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


        public static DynamicOperatorBase GetOperator(Type type)
        {
            if (!OperatorMapping.ContainsKey(type))
            {
                OperatorMapping[type] = FastMethodOperator.New
                    .Using(type)
                    .MethodBody($@"return new DynamicOperator<{type.GetDevelopName()}>();")
                    .Return<DynamicOperatorBase>()
                    .Complie<Func<DynamicOperatorBase>>();
            }
            return OperatorMapping[type]();
        }


        /// <summary>
        /// 初始化类型
        /// </summary>
        /// <param name="type">类型</param>
        private void InitType(Type type)
        {
            if (!GetDynamicCache.ContainsKey(type))
            {

                GetDynamicCache[type] = new ConcurrentDictionary<string, Action<DynamicOperatorBase, object>>();
                SetDynamicCache[type] = new ConcurrentDictionary<string, Action<DynamicOperatorBase, object>>();


                //动态函数-实例的创建
                CtorMapping[type] = CtorBuilder.NewDelegate<object>(type);


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

    
    public static class InternalEntityCaller<S,T>
    {
        internal readonly static ConcurrentDictionary<string, Action<S, T>> SetterDelegateMapping;
        internal readonly static ConcurrentDictionary<string, Func<S, T>> GetterDelegateMapping;
        static InternalEntityCaller()
        {
            SetterDelegateMapping = new ConcurrentDictionary<string, Action<S, T>>();
            GetterDelegateMapping = new ConcurrentDictionary<string, Func<S, T>>();

            Type type = typeof(S);
            Type memberType = typeof(T);

            var fields = type.GetFields();
            for (int i = 0; i < fields.Length; i+=1)
            {
                if (fields[i].FieldType==memberType)
                {

                    SetterDelegateMapping[fields[i].Name] = FastMethodOperator
                        .New
                        .Param<S>("obj")
                        .Param<T>("value")
                        .MethodBody($"obj.{fields[i].Name} = value;")
                        .Return()
                        .Complie<Action<S, T>>();

                    GetterDelegateMapping[fields[i].Name] = FastMethodOperator
                        .New
                        .Param<S>("obj")
                        .MethodBody($"return obj.{fields[i].Name};")
                        .Return<T>()
                        .Complie<Func<S, T>>();

                }
            }

            var props = type.GetProperties(); 
            for (int i = 0; i < props.Length; i += 1)
            {
                if (props[i].PropertyType == memberType)
                {

                    SetterDelegateMapping[props[i].Name] = FastMethodOperator
                        .New
                        .Param<S>("obj")
                        .Param<T>("value")
                        .MethodBody($"obj.{props[i].Name} = value;")
                        .Return()
                        .Complie<Action<S, T>>();

                    GetterDelegateMapping[props[i].Name] = FastMethodOperator
                       .New
                       .Param<S>("obj")
                       .MethodBody($"return obj.{props[i].Name};")
                       .Return<T>()
                       .Complie<Func<S, T>>();

                }
            }
        }

        
    }


    internal class DynamicMemberHelper
    {
        internal static ConcurrentDictionary<Type, string> TypeMemberMapping;
        static DynamicMemberHelper()
        {
            TypeMemberMapping = new ConcurrentDictionary<Type, string>();
            var infos = typeof(DynamicOperatorBase).GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < infos.Length; i++)
            {
                TypeMemberMapping[infos[i].FieldType] = infos[i].Name;
            }
        }


        public static Action<DynamicOperatorBase, T> SetField<T>(FieldInfo info)
        {
            if (TypeMemberMapping.ContainsKey(info.FieldType))
            {
                string body;
                Type type;
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

            return null;
        }


        public static Action<DynamicOperatorBase, T> GetField<T>(FieldInfo info)
        {
            string body;
            Type type;
            if (typeof(T) == typeof(object))
            {
                type = info.DeclaringType;
                if (TypeMemberMapping.ContainsKey(info.FieldType))
                {
                    body = $@"proxy.{TypeMemberMapping[info.FieldType]}=(({type.Name})instance).{info.Name};";
                }
                else
                {
                    body = $@"DynamicOperator<{info.FieldType.GetDevelopName()}> {info.Name}temp = (({type.Name})instance).{info.Name}; proxy.OperatorValue={info.Name}temp;";
                }

            }
            else
            {
                type = typeof(T);
                if (TypeMemberMapping.ContainsKey(info.FieldType))
                {
                    body = $@"proxy.{TypeMemberMapping[info.FieldType]}=instance.{info.Name};";
                }
                else
                {
                    body = $@"DynamicOperator<{info.FieldType.GetDevelopName()}> {info.Name}temp = instance.{info.Name}; proxy.OperatorValue={info.Name}temp;";
                }
            }


            return GetDelegate<T>(type, info.FieldType, body);
        }


        public static Action<DynamicOperatorBase, T> GetProperty<T>(PropertyInfo info)
        {
            string body;
            Type type;
            if (typeof(T) == typeof(object))
            {
                type = info.DeclaringType;
                if (TypeMemberMapping.ContainsKey(info.PropertyType))
                {
                    body = $@"proxy.{TypeMemberMapping[info.PropertyType]}=(({type.Name})instance).{info.Name};";
                }
                else
                {
                    body = $@"DynamicOperator<{info.PropertyType.GetDevelopName()}> {info.Name}temp = (({type.Name})instance).{info.Name}; proxy.OperatorValue={info.Name}temp;";
                }
            }
            else
            {
                type = typeof(T);
                if (TypeMemberMapping.ContainsKey(info.PropertyType))
                {
                    body = $@"proxy.{TypeMemberMapping[info.PropertyType]}=instance.{info.Name};";
                }
                else
                {
                    body = $@"DynamicOperator<{info.PropertyType.GetDevelopName()}> {info.Name}temp = instance.{info.Name}; proxy.OperatorValue={info.Name}temp;";
                }
            }

            return GetDelegate<T>(type, info.PropertyType, body);
        }


        public static Action<DynamicOperatorBase, T> SetProperty<T>(PropertyInfo info)
        {
            if (TypeMemberMapping.ContainsKey(info.PropertyType))
            {
                string body;
                Type type;
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

                return GetDelegate<T>(type, info.PropertyType, body);
            }
            return null;
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
