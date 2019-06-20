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
                var members = type.GetMembers( BindingFlags.Public| BindingFlags.Static| BindingFlags.Instance );
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
            string body = "";
            Type type = info.DeclaringType;
            if (TypeMemberMapping.ContainsKey(info.FieldType))
            {
                body = $@"proxy.{TypeMemberMapping[info.FieldType]}={type.GetDevelopName()}.{info.Name};";
            }
            else
            {
                body = $@"DynamicOperator<{info.FieldType.GetDevelopName()}> {info.Name}temp = {type.GetDevelopName()}.{info.Name}; proxy.OperatorValue={info.Name}temp;";
            }
            return GetStaticDelegate(type, info.FieldType, body);
        }


        public static Action<DynamicOperatorBase> GetProperty(PropertyInfo info)
        {
            string body = "";
            Type type = info.DeclaringType;
            if (TypeMemberMapping.ContainsKey(info.PropertyType))
            {
                body = $@"proxy.{TypeMemberMapping[info.PropertyType]}={type.GetDevelopName()}.{info.Name};";
            }
            else
            {
                body = $@"DynamicOperator<{info.PropertyType.GetDevelopName()}> {info.Name}temp = {type.GetDevelopName()}.{info.Name}; proxy.OperatorValue={info.Name}temp;";
            }
            return GetStaticDelegate(type, info.PropertyType, body);
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
