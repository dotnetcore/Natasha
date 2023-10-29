using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Template.Reverser
{
    public static class GenericConstraintReverser
    {


        public static string? GetConstraint(ConstraintFlags type)
        {
            switch (type)
            {
                case ConstraintFlags.Class:
                    return "class";
                case ConstraintFlags.Struct:
                    return "struct";
                case ConstraintFlags.Unmanaged:
                    return "unmanaged";
                case ConstraintFlags.New:
                    return "new()";
                default:
                    break;
            }
            return default;
        }

        /// <summary>
        /// 获取参数泛型约束，例如”out T“
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetVariant<T>()
        {
            return GetVariant(typeof(T));
        }
        public static string GetVariant(Type type)
        {

            GenericParameterAttributes variance =
           type.GenericParameterAttributes
           & GenericParameterAttributes.VarianceMask;

            if (variance == GenericParameterAttributes.None)
            {

                return "";

            }
            if ((variance & GenericParameterAttributes.Covariant) != 0)
            {
                return $"out {type.GetDevelopName()}";
            }
            else
            {
                return $"in {type.GetDevelopName()}";
            }

        }





        /// <summary>
        /// 获取参数泛型约束数组，例如[”out T“, "in S"]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetVariants<T>()
        {
            return GetVariant(typeof(T));
        }
        public static string[]? GetVariants(Type type)
        {

            if (type.IsGenericType)
            {
                var types = type.GetGenericArguments();
                var result = new string[types.Length];
                for (int i = 0; i < types.Length; i++)
                {
                    result[i] = GetVariant(types[i]);
                }
                return result;
            }
            return null;

        }




        /// <summary>
        /// 获取类型约束 例如：class,new()
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string? GetConstraints<T>()
        {
            return GetConstraints(typeof(T));
        }
        public static string? GetConstraints(Type type)
        {

            string? result = default;
            if (type.IsGenericParameter)
            {

                GenericParameterAttributes constraints =
            type.GenericParameterAttributes
            & GenericParameterAttributes.SpecialConstraintMask;


                if ((constraints & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
                {

                    result = "class";

                }
                else if ((constraints & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
                {

                    var temp = type.GetCustomAttributes().Any(item => item.GetType().Name == "IsUnmanagedAttribute");
                    if (temp)
                    {
                        return "unmanaged";
                    }
                    else
                    {
                        return "struct";
                    }
                }


                if ((constraints & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
                {
                    if (result != default)
                    {
                        result += ", ";
                    }
                    result += "new()";
                }

            }
            return result;

        }




        /// <summary>
        /// 获取完整泛型约束，例如:where T : class, new(), interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTypeConstraints<T>()
        {
            return GetTypeConstraints(typeof(T));
        }
        public static string GetTypeConstraints(Type type)
        {

            StringBuilder builder = new StringBuilder();
            var types = type.GetGenericArguments();
            foreach (var item in types)
            {

                StringBuilder typeBuilder = new StringBuilder();
                var attrs = item.GetCustomAttributes();
                if (item.GetCustomAttributes().Any(attr => attr.GetType().Name == "NullableAttribute"))
                {

                    typeBuilder.Append("notnull, ");

                }

                Type[] tpConstraints = item.GetGenericParameterConstraints();
                foreach (Type tpc in tpConstraints)
                {

                    if (tpc != typeof(ValueType))
                    {

                        typeBuilder.Append(tpc.GetDevelopName() + ", ");

                    }

                }


                typeBuilder.Append(GetConstraints(item));
                if (typeBuilder.Length > 2)
                {
                    if (typeBuilder[typeBuilder.Length - 2] == ',')
                    {
                        typeBuilder.Length -= 2;
                    }
                }


                if (typeBuilder.Length != 0)
                {
                    builder.Append($"where {item.GetDevelopName()} : {typeBuilder} ");
                }

            }

            return builder.ToString();

        }

    }

}
