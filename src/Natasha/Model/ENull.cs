using Natasha.Cache;
using System.Reflection.Emit;

namespace Natasha
{
    //空操作
    public partial class ENull {

        static ENull()
        {
            instance = new ENull();
        }

        private readonly static ENull instance;
        public static ENull Value { get { return instance; } }
        public static void LoadNull()
        {
            ThreadCache.GetIL().Emit(OpCodes.Ldnull);
        }
    }
}
