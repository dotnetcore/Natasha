using Natasha.Operator;
using System;

namespace Natasha.MethodExtension
{
    public static class StringExtension
    {

        public static T Delegate<T>(this string instance,params NamespaceConverter[] types) where T : Delegate
        {
            return NDelegateOperator<T>.Delegate(instance, types);
        }
        public static T AsyncDelegate<T>(this string instance, params NamespaceConverter[] types) where T : Delegate
        {
            return NDelegateOperator<T>.AsyncDelegate(instance, types);
        }
        public static T UnsafeDelegate<T>(this string instance, params NamespaceConverter[] types) where T : Delegate
        {
            return NDelegateOperator<T>.UnsafeDelegate(instance, types);
        }
        public static T UnsafeAsyncDelegate<T>(this string instance, params NamespaceConverter[] types) where T : Delegate
        {
            return NDelegateOperator<T>.UnsafeAsyncDelegate(instance, types);
        }
        public static FastMethodOperator FastOperator(this string instance, params NamespaceConverter[] types)
        {
            var builder = FastMethodOperator.Create();
            builder.Using(types);
            return builder.MethodBody(instance);
        }

    }

}
