using System;
using System.Collections.Generic;
using System.Linq;

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


    public static class IEnumerableCloneExtension
    {
        public static IEnumerable<T> NormalCloneExtension<T>(this IEnumerable<T> collection)
        {
            if (collection == null)
            {
                return null;
            }

            return collection.Select(item => item);
        }
        public static IEnumerable<T> CloneExtension<T>(this IEnumerable<T> collection)
        {
            if (collection==null)
            {
                return null;
            }
            
            return collection.Select(item => DeepClone<T>.Clone(item));
        }

        public static IDictionary<TKey,TValue> CloneExtension<TKey, TValue>(this IDictionary<TKey, TValue> collection)
        {
            if (collection == null)
            {
                return null;
            }

            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in collection)
            {
                dictionary[DeepClone<TKey>.Clone(item.Key)] = DeepClone<TValue>.Clone(item.Value);
            }
            return dictionary;
        }

        public static IDictionary<TKey, TValue> OnlyKeyCloneExtension<TKey, TValue>(this IDictionary<TKey, TValue> collection)
        {
            if (collection == null)
            {
                return null;
            }

            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in collection)
            {
                dictionary[DeepClone<TKey>.Clone(item.Key)] = item.Value;
            }
            return dictionary;
        }
        public static IDictionary<TKey, TValue> OnlyValueCloneExtension<TKey, TValue>(this IDictionary<TKey, TValue> collection)
        {
            if (collection == null)
            {
                return null;
            }

            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in collection)
            {
                dictionary[DeepClone<TKey>.Clone(item.Key)] = item.Value;
            }
            return dictionary;
        }
    }


    public static class IEnumerableDiffExtension {
        public static HashSet<T> SnapshotExtension<T>(this IEnumerable<T> collection,IEnumerable<T> oldInstances)
        {
            HashSet<T> temp = new HashSet<T>(oldInstances);
            foreach (var item1 in oldInstances)
            {
                foreach (var item2 in collection)
                {
                    if (SnapshotOperator.Diff(item1, item2).Count==0)
                    {
                        temp.Remove(item1);
                    }
                }
            }
            return temp;
        }
    }
}
