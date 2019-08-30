using Natasha.Operator;
using System;

namespace Natasha.MethodExtension
{
    public static class StringExtension
    {
        public static T CreateUsingStrings<T>(this string instance,params string[] usings) where T : Delegate
        {
            return DelegateOperator<T>.CreateUsingStrings(instance, usings);
        }
        public static T Delegate<T>(this string instance,params Type[] types) where T : Delegate
        {
            return DelegateOperator<T>.Delegate(instance, types);
        }
        public static T AsyncDelegate<T>(this string instance, params Type[] types) where T : Delegate
        {
            return DelegateOperator<T>.AsyncDelegate(instance, types);
        }
        public static T UnsafeDelegate<T>(this string instance, params Type[] types) where T : Delegate
        {
            return DelegateOperator<T>.UnsafeDelegate(instance, types);
        }
        public static T UnsafeAsyncDelegate<T>(this string instance, params Type[] types) where T : Delegate
        {
            return DelegateOperator<T>.UnsafeAsyncDelegate(instance, types);
        }




        public static FastMethodOperator FastOperator(this string instance, params Type[] types)
        {
            var builder = FastMethodOperator.New;
            builder.UsingRecoder.Add(types);
            return builder.MethodBody(instance);
        }
    }
}
