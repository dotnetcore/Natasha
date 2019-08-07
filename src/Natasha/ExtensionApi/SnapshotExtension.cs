using Natasha.Operator;
using System.Collections.Generic;

namespace Natasha.SnapshotExtension
{
    public static class SnapshotExtension
    {
        public static void MakeSnapshot<T>(this T instance)
        {
            SnapshotOperator.MakeSnapshot(instance);
        }
        public static Dictionary<string,DiffModel> Compare<T>(this T instance)
        {
            return SnapshotOperator.Compare(instance);
        }
    }
}
