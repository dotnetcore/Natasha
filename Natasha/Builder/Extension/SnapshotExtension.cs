using System.Collections.Generic;

namespace Natasha
{
    public static class IEnumerableSnapshotExtension
    {
        public static HashSet<T> SnapshotExtension<T>(this IEnumerable<T> collection, IEnumerable<T> oldInstances)
        {
            HashSet<T> temp = new HashSet<T>(oldInstances);
            foreach (var item1 in oldInstances)
            {
                foreach (var item2 in collection)
                {
                    if (SnapshotOperator.Diff(item1, item2).Count == 0)
                    {
                        temp.Remove(item1);
                    }
                }
            }
            return temp;
        }
    }
}
