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

                var instance = new BuilderInfo();
                instance.DeclaringType = tempInfo.DeclaringType;
                instance.DeclaringTypeName = tempInfo.DeclaringType.GetDevelopName();
                instance.DeclaringAvailableName = tempInfo.DeclaringType.GetAvailableName();

                instance.MemberName = tempInfo.Name;
                instance.MemberType = tempInfo.FieldType;
                instance.MemberTypeName = tempInfo.FieldType.GetDevelopName();
                instance.MemberTypeAvailableName = tempInfo.FieldType.GetAvailableName();

                instance.ElementType = tempInfo.FieldType.HasElementType ? tempInfo.FieldType.GetElementType() : tempInfo.FieldType;
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

                var instance = new BuilderInfo();
                instance.DeclaringType = tempInfo.DeclaringType;
                instance.DeclaringTypeName = tempInfo.DeclaringType.GetDevelopName();
                instance.DeclaringAvailableName = tempInfo.DeclaringType.GetAvailableName();

                instance.MemberName = tempInfo.Name;
                instance.MemberType = tempInfo.PropertyType;
                instance.MemberTypeName = tempInfo.PropertyType.GetDevelopName();
                instance.MemberTypeAvailableName = tempInfo.PropertyType.GetAvailableName();

                instance.ElementType = tempInfo.PropertyType.HasElementType ? tempInfo.PropertyType.GetElementType() : tempInfo.PropertyType;
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
            var instance = new BuilderInfo();
            instance.DeclaringType = type;
            instance.DeclaringTypeName = type.GetDevelopName();
            instance.DeclaringAvailableName = type.GetAvailableName();
            instance.ElementType = type.HasElementType ? type.GetElementType() : type;
            instance.ElementTypeName = instance.ElementType.GetDevelopName();
            instance.ElementTypeAvailableName = instance.ElementType.GetAvailableName();
            return instance;
        }
    }
}
