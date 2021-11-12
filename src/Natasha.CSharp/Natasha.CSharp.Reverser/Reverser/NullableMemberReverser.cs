using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Reverser
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

            if (nullInfo.ReadState != NullabilityState.Nullable)
            {
                if (nullInfo.ElementType != null)
                {
                    return $"{GetMemberNullableDevelopName(nullInfo.ElementType)}[]";
                }
                else if (nullInfo.GenericTypeArguments.Length > 0)
                {
                    StringBuilder typeStringBuilder = new StringBuilder();
                    var type = nullInfo.Type;
                    var typeString = (!string.IsNullOrEmpty(type.Namespace) && !string.IsNullOrEmpty(type.FullName)) ? $"{type.Namespace}.{type.Name.Split('`')[0]}" : type.Name.Split('`')[0];
                    typeStringBuilder.Append($"{typeString}<{GetMemberNullableDevelopName(nullInfo.GenericTypeArguments[0])}");
                    for (int i = 1; i < nullInfo.GenericTypeArguments.Length; i++)
                    {
                        typeStringBuilder.Append($",{GetMemberNullableDevelopName(nullInfo.GenericTypeArguments[i])}");
                    }
                    typeStringBuilder.Append('>');
                    return typeStringBuilder.ToString();
                }
                return nullInfo.Type.GetDevelopName();
            }
            return nullInfo.Type.GetDevelopName()+"?";

        }

        public static string GetMemberNullableDevelopName(this ParameterInfo parameterInfo)
        {

            NullabilityInfo nullInfo = _nullableContextHandler.Create(parameterInfo);
            return GetMemberNullableDevelopName(nullInfo);

        }
        public static string GetMemberNullableDevelopName(this FieldInfo fieldInfo)
        {

            NullabilityInfo nullInfo = _nullableContextHandler.Create(fieldInfo);
            return GetMemberNullableDevelopName(nullInfo);

        }

        public static string GetMemberNullableDevelopName(this PropertyInfo propertyInfo)
        {

            NullabilityInfo nullInfo = _nullableContextHandler.Create(propertyInfo);
            return GetMemberNullableDevelopName(nullInfo);

        }

        public static string GetMemberNullableDevelopName(this EventInfo eventInfo)
        {

            NullabilityInfo nullInfo = _nullableContextHandler.Create(eventInfo);
            return GetMemberNullableDevelopName(nullInfo);

        }
    }
}
