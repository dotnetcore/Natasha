using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha
{
    /// <summary>
    /// 构建信息
    /// </summary>
    public class NBuildInfo
    {

        public Type FatherType;
        public string FatherTypeName;
        public string FatherAvailableName;


        public Type CurrentType;
        public string CurrentTypeName;
        public string CurrentTypeAvailableName;


        public Type MemberType;
        public string MemberTypeName;
        public string MemberTypeAvailableName;
        public string MemberName;


        public Type ElementType;
        public string ElementTypeName;
        public string ElementTypeAvailableName;


        public Type ArrayBaseType;
        public string ArrayBaseTypeName;
        public string ArrayBaseTypeAvaliableName;


        public int ArrayLayer;
        public int ArrayDimensions;


        public bool IsStatic;
        public bool IsNew;
        public bool CanRead;
        public bool CanWrite;


        public string StaticName;


        public static implicit operator NBuildInfo(MemberInfo info)
        {
            if (info.MemberType == MemberTypes.Field)
            {

                var tempInfo = (FieldInfo)(info);
                var instance = new NBuildInfo
                {

                    MemberName = tempInfo.Name,
                    MemberType = tempInfo.FieldType,
                    MemberTypeName = tempInfo.FieldType.GetDevelopName(),
                    MemberTypeAvailableName = tempInfo.FieldType.GetAvailableName(),
                    IsNew = tempInfo.DeclaringType != tempInfo.ReflectedType,
                    CanRead = true,
                    CanWrite = !tempInfo.IsInitOnly

                };


                HandlerDeclaringType(instance, tempInfo.ReflectedType);
                HandlerArrayType(instance, tempInfo.FieldType);


                if (tempInfo.IsStatic)
                {

                    instance.IsStatic = true;
                    instance.StaticName = $"{instance.CurrentTypeName}";

                }
                return instance;

            }
            else if (info.MemberType == MemberTypes.Property)
            {

                var tempInfo = (PropertyInfo)(info);
                var instance = new NBuildInfo
                {

                    MemberName = tempInfo.Name,
                    MemberType = tempInfo.PropertyType,
                    MemberTypeName = tempInfo.PropertyType.GetDevelopName(),
                    MemberTypeAvailableName = tempInfo.PropertyType.GetAvailableName(),
                    IsNew = tempInfo.DeclaringType != tempInfo.ReflectedType,
                    CanRead = tempInfo.CanRead,
                    CanWrite = tempInfo.CanWrite
                    
                };


                HandlerDeclaringType(instance, tempInfo.ReflectedType);
                HandlerArrayType(instance, tempInfo.PropertyType);


                if (tempInfo.GetGetMethod(true).IsStatic)
                {

                    instance.IsStatic = true;
                    instance.StaticName = $"{instance.CurrentTypeName}";

                }
                return instance;

            }

            return null;
        }




        public static implicit operator NBuildInfo(Type type)
        {

            var instance = new NBuildInfo();
            HandlerDeclaringType(instance, type);
            HandlerArrayType(instance, type);
            return instance;

        }




        public static NBuildInfo HandlerArrayType(NBuildInfo instance, Type type)
        {

            if (type.IsArray)
            {

                Type temp = type;
                instance.ElementType = type.GetElementType();
                instance.ElementTypeName = instance.ElementType.GetDevelopName();
                instance.ElementTypeAvailableName = instance.ElementType.GetAvailableName();


                int count = 0;
                while (temp.HasElementType)
                {

                    count++;
                    temp = temp.GetElementType();

                }
                instance.ArrayLayer = count;
                instance.ArrayBaseType = temp;
                instance.ArrayBaseTypeName = instance.ArrayBaseType.GetDevelopName();
                instance.ArrayBaseTypeAvaliableName = instance.ArrayBaseType.GetAvailableName();


                var ctor = type.GetConstructors()[0];
                instance.ArrayDimensions = ctor.GetParameters().Length;


            }

            return instance;

        }




        public static NBuildInfo HandlerDeclaringType(NBuildInfo instance, Type type)
        {

            instance.CurrentType = type;
            instance.CurrentTypeName = type.GetDevelopName();
            instance.CurrentTypeAvailableName = type.GetAvailableName();

            instance.FatherType = type.BaseType;
            instance.FatherTypeName = type.BaseType.GetDevelopName();
            instance.FatherAvailableName = type.BaseType.GetAvailableName();
            return instance;

        }




        public static IDictionary<string,NBuildInfo> GetInfos<T>()
        {

            return GetInfos(typeof(T));

        }




        public static IDictionary<string, NBuildInfo> GetInfos(Type type)
        {

            if (type==default)
            {
                return default;
            }


            Dictionary<string, NBuildInfo> cache = new Dictionary<string, NBuildInfo>();
            var members = type.GetMembers();
            for (int i = 0; i < members.Length; i+=1)
            {

                var member = members[i];
                if (
                        !cache.ContainsKey(member.Name) 
                        || 
                        member.ReflectedType == member.DeclaringType
                    )
                {

                    var tempBuilder = member;
                    if (tempBuilder!=null)
                    {
                        cache[member.Name] = member;
                    }
                    
                }

            }
            return cache;

        }

    }

}
