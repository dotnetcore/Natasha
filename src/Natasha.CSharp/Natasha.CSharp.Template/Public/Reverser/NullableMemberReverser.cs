using System;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Template.Reverser
{
    public static class NullableMemberReverser
    {
        private static readonly NullabilityInfoContext _nullableContextHandler;
        static NullableMemberReverser()
        {
            _nullableContextHandler = new();
        }
        public static string GetMemberNullableDevelopName(NullabilityInfo nullInfo)
        {
            var type = nullInfo.Type;
            if (nullInfo.ReadState != NullabilityState.Nullable)
            {
                if (nullInfo.ElementType != null)
                {
                    return $"{GetMemberNullableDevelopName(nullInfo.ElementType)}[]";
                }
                else if (nullInfo.GenericTypeArguments.Length > 0)
                {
                    StringBuilder typeStringBuilder = new StringBuilder();
                    var typeString = !string.IsNullOrEmpty(type.Namespace) && !string.IsNullOrEmpty(type.FullName) ? $"{type.Namespace}.{type.Name.Split('`')[0]}" : type.Name.Split('`')[0];
                    typeStringBuilder.Append($"{typeString}<{GetMemberNullableDevelopName(nullInfo.GenericTypeArguments[0])}");
                    for (int i = 1; i < nullInfo.GenericTypeArguments.Length; i++)
                    {
                        typeStringBuilder.Append($",{GetMemberNullableDevelopName(nullInfo.GenericTypeArguments[i])}");
                    }
                    typeStringBuilder.Append('>');
                    return typeStringBuilder.ToString();
                }
                return type.GetDevelopName();
            }
            if (type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GetDevelopName();
            }
            return type.GetDevelopName() + "?";

        }

        public static string GetMemberNullableDevelopName(this ParameterInfo parameterInfo)
        {

            NullabilityInfo nullInfo = new NullabilityInfoContext().Create(parameterInfo);
            return GetMemberNullableDevelopName(nullInfo);

        }
        public static string GetMemberNullableDevelopName(this FieldInfo fieldInfo)
        {

            NullabilityInfo nullInfo = new NullabilityInfoContext().Create(fieldInfo);
            return GetMemberNullableDevelopName(nullInfo);

        }

        public static string GetMemberNullableDevelopName(this PropertyInfo propertyInfo)
        {

            NullabilityInfo nullInfo = new NullabilityInfoContext().Create(propertyInfo);
            return GetMemberNullableDevelopName(nullInfo);

        }

        public static string GetMemberNullableDevelopName(this EventInfo eventInfo)
        {

            NullabilityInfo nullInfo = new NullabilityInfoContext().Create(eventInfo);
            return GetMemberNullableDevelopName(nullInfo);

        }
    }
}
