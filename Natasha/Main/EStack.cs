using Natasha.Cache;
using System.Reflection.Emit;

namespace Natasha
{
    public class EStack
    {
        public static void Push()
        {
            ThreadCache.GetIL().Emit(OpCodes.Dup);
        }
        public static void Pop()
        {
            ThreadCache.GetIL().Emit(OpCodes.Pop);
        }
    }
}
