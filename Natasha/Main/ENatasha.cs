using Natasha.Cache;

namespace Natasha
{
    public static class ENatasha
    {
        static ENatasha()
        {

        }
        public static void Initialize()
        {
            ThreadCache.Initialize();
            ClassCache.Initialize();
        }
    }
}
