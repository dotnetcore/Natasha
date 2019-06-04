using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Natasha
{
    public static class SnapshotOperator<T>
    {
        public static readonly ConcurrentDictionary<T, T> SnapshotCache;

        static SnapshotOperator() => SnapshotCache = new ConcurrentDictionary<T, T>();
        public static void MakeSnapshot(T needSnapshot)
        {
            SnapshotCache[needSnapshot] = DeepClone.Clone(needSnapshot);
        }

        public static Func<T, T, Dictionary<string, DiffModel>> CompareFunc;

        public static Dictionary<string, DiffModel> Diff(T newInstance, T oldInstance)
        {
            if (CompareFunc == null)
            {
                CompareFunc = (Func<T, T, Dictionary<string, DiffModel>>)(new SnapshotBuilder(typeof(T)).Create());
            }
            return CompareFunc(newInstance, oldInstance);
        }
        public static bool IsDiffernt(T instance)
        {
            return Compare(instance).Count != 0;
        }

        public static Dictionary<string, DiffModel> Compare(T instance)
        {
            if (CompareFunc == null)
            {
                CompareFunc = (Func<T, T, Dictionary<string, DiffModel>>)(new SnapshotBuilder(typeof(T)).Create());
            }
            return CompareFunc(instance, SnapshotCache[instance]);
        }
    }
    public static class SnapshotOperator
    {
        public static Dictionary<string, DiffModel> Diff<T>(T newInstance, T oldInstance)
        {
            return SnapshotOperator<T>.Diff(newInstance, oldInstance);
        }
        public static bool IsDiffernt<T>(T instance)
        {
            return SnapshotOperator<T>.Compare(instance).Count != 0;
        }
        public static Dictionary<string, DiffModel> Compare<T>(T instance)
        {
            return SnapshotOperator<T>.Compare(instance);
        }
        public static void MakeSnapshot<T>(T needSnapshot)
        {
            SnapshotOperator<T>.MakeSnapshot(needSnapshot);
        }
    }
}
