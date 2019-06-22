using System;
using System.Reflection;

namespace Natasha
{
    /// <summary>
    /// 构建信息
    /// </summary>
    public class BuilderInfo
    {
        public Type DeclaringType;
        public string DeclaringTypeName;
        public string DeclaringAvailableName;

        public Type MemberType;
        public string MemberTypeName;
        public string MemberTypeAvailableName;
        public string MemberName;
       
        public Type ElementType;
        public string ElementTypeName;
        public string ElementTypeAvailableName;
        public bool IsStatic;


        public string StaticName;

        public static implicit operator BuilderInfo(MemberInfo info)
        {
            if (info.MemberType == MemberTypes.Field)
            {
                var tempInfo = (FieldInfo)(info);

                var instance = new BuilderInfo
                {
                    DeclaringType = tempInfo.DeclaringType,
                    DeclaringTypeName = tempInfo.DeclaringType.GetDevelopName(),
                    DeclaringAvailableName = tempInfo.DeclaringType.GetAvailableName(),


                    MemberName = tempInfo.Name,
                    MemberType = tempInfo.FieldType,
                    MemberTypeName = tempInfo.FieldType.GetDevelopName(),
                    MemberTypeAvailableName = tempInfo.FieldType.GetAvailableName(),


                    ElementType = tempInfo.FieldType.HasElementType ? tempInfo.FieldType.GetElementType() : tempInfo.FieldType
                };
                instance.ElementTypeName = instance.ElementType.GetDevelopName();
                instance.ElementTypeAvailableName = instance.ElementType.GetAvailableName();

                instance.IsStatic = tempInfo.IsStatic;

                if (instance.IsStatic)
                {
                    instance.StaticName = $"{instance.DeclaringTypeName}.{tempInfo.Name}";
                }
                return instance;
            }
            else if (info.MemberType == MemberTypes.Property)
            {
                var tempInfo = (PropertyInfo)(info);

                var instance = new BuilderInfo
                {
                    DeclaringType = tempInfo.DeclaringType,
                    DeclaringTypeName = tempInfo.DeclaringType.GetDevelopName(),
                    DeclaringAvailableName = tempInfo.DeclaringType.GetAvailableName(),


                    MemberName = tempInfo.Name,
                    MemberType = tempInfo.PropertyType,
                    MemberTypeName = tempInfo.PropertyType.GetDevelopName(),
                    MemberTypeAvailableName = tempInfo.PropertyType.GetAvailableName(),


                    ElementType = tempInfo.PropertyType.HasElementType ? tempInfo.PropertyType.GetElementType() : tempInfo.PropertyType
                };
                instance.ElementTypeName = instance.ElementType.GetDevelopName();
                instance.ElementTypeAvailableName = instance.ElementType.GetAvailableName();

                instance.IsStatic = tempInfo.GetGetMethod(true).IsStatic;

                if (instance.IsStatic)
                {
                    instance.StaticName = $"{instance.DeclaringTypeName}.{tempInfo.Name}";
                }
                return instance;
            }
            return null;
        }

        public static implicit operator BuilderInfo(Type type)
        {
            var instance = new BuilderInfo
            {
                DeclaringType = type,
                DeclaringTypeName = type.GetDevelopName(),
                DeclaringAvailableName = type.GetAvailableName(),
                ElementType = type.HasElementType ? type.GetElementType() : type
            };
            instance.ElementTypeName = instance.ElementType.GetDevelopName();
            instance.ElementTypeAvailableName = instance.ElementType.GetAvailableName();
            return instance;
        }
    }
}
