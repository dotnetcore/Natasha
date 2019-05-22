using System;

namespace Natasha
{
    public static class DeepClone<T>
    {
        public static Func<T,T> Clone;
    }
}
