using System.Linq;
using System.Reflection;

namespace Natasha.CSharp.Template.Reverser
{ 
    //#if NET6_0_OR_GREATER
    public static class NullableNatashaExtension
    {
        private static readonly NullabilityInfoContext _nullableContextHandler;
        static NullableNatashaExtension()
        {
            _nullableContextHandler = new();
        }

        public static bool IsContainsNullable(this FieldInfo fieldInfo)
        {
            NullabilityInfo nullInfo = _nullableContextHandler.Create(fieldInfo);
            return IsContainsNullable(nullInfo);

        }

        public static bool IsContainsNullable(NullabilityInfo nullInfo)
        {

            if (nullInfo.ReadState != NullabilityState.Nullable)
            {
                if (nullInfo.ElementType != null)
                {
                    return IsContainsNullable(nullInfo.ElementType);
                }
                else if (nullInfo.GenericTypeArguments.Length > 0)
                {
                    return nullInfo.GenericTypeArguments.Any(item => IsContainsNullable(item));
                }
                return false;
            }
            return true;

        }

        public static bool IsContainsNullable(this PropertyInfo propertyInfo)
        {

            NullabilityInfo nullInfo = _nullableContextHandler.Create(propertyInfo);
            return IsContainsNullable(nullInfo);

        }

        public static bool IsContainsNullable(this ParameterInfo parameterInfo)
        {

            NullabilityInfo nullInfo = _nullableContextHandler.Create(parameterInfo);
            return IsContainsNullable(nullInfo);

        }

        public static bool IsContainsNullable(this EventInfo eventInfo)
        {

            NullabilityInfo nullInfo = _nullableContextHandler.Create(eventInfo);
            return IsContainsNullable(nullInfo);

        }
    }

    //#else
    /*
    public static class NullableNatashaExtension
    {
    public static bool IsContainsNullable(this PropertyInfo property) =>
    IsNullableHelper(property.PropertyType, property.DeclaringType, property.CustomAttributes);

    public static bool IsContainsNullable(this FieldInfo field) =>
    IsNullableHelper(field.FieldType, field.DeclaringType, field.CustomAttributes);

    public static bool IsContainsNullable(this ParameterInfo parameter) =>
    IsNullableHelper(parameter.ParameterType, parameter.Member, parameter.CustomAttributes);

    public static bool IsContainsNullable(this Type type) =>
    IsNullableHelper(type.GetElementType(), type, type.CustomAttributes);

    static bool IsNullableHelper(Type memberType, MemberInfo? declaringType, IEnumerable<CustomAttributeData> customAttributes)
    {
    if (memberType.IsValueType)
        return Nullable.GetUnderlyingType(memberType) != null;

    var nullable = customAttributes
        .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableAttribute");
    if (nullable != null && nullable.ConstructorArguments.Count == 1)
    {
        var attributeArgument = nullable.ConstructorArguments[0];
        if (attributeArgument.ArgumentType == typeof(byte[]))
        {
            var args = (ReadOnlyCollection<CustomAttributeTypedArgument>)attributeArgument.Value!;
            if (args.Count > 0 && args[0].ArgumentType == typeof(byte))
            {
                return (byte)args[0].Value! == 2;
            }
        }
        else if (attributeArgument.ArgumentType == typeof(byte))
        {
            return (byte)attributeArgument.Value! == 2;
        }
    }

    for (var type = declaringType; type != null; type = type.DeclaringType)
    {
        var context = type.CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == "System.Runtime.CompilerServices.NullableContextAttribute");
        if (context != null &&
            context.ConstructorArguments.Count == 1 &&
            context.ConstructorArguments[0].ArgumentType == typeof(byte))
        {
            return (byte)context.ConstructorArguments[0].Value! == 2;
        }
    }

    // Couldn't find a suitable attribute
    return false;
    }

    }*/
    //#endif

}

