using System;

namespace Natasha
{
    public static class StringExtension
    {
        public static T Create<T>(this string instance) where T : Delegate
        {
            return DelegateOperator<T>.Create(instance);
        }
    }
}
