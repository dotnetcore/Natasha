using System;

namespace Natasha
{
    /// <summary>
    /// 深度克隆
    /// </summary>
    public static class CloneOperator {
        public static T Clone<T>(T instance)
        {
            return CloneOperator<T>.Clone(instance);
        }
    }

    /// <summary>
    /// 深度克隆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class CloneOperator<T>
    {
        public readonly static Func<T, T> Clone;
        static CloneOperator() => Clone = (Func<T, T>)CloneBuilder<T>.Create();
    }
}
