using System;

namespace Natasha
{
    /// <summary>
    /// 深度克隆
    /// </summary>
    public static class DeepClone {
        public static T Clone<T>(T instance)
        {
            return DeepClone<T>.Clone(instance);
        }
    }

    /// <summary>
    /// 深度克隆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class DeepClone<T>
    {
        static DeepClone()
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
