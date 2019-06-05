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
        static CloneOperator()
        {

        }
        public static Func<T, T> CloneDelegate;
        public static T Clone(T instance)
        {
            if (CloneDelegate==null)
            {
                CloneBuilder<T>.CreateCloneDelegate();
            }
            return CloneDelegate(instance);
        }
    }
}
