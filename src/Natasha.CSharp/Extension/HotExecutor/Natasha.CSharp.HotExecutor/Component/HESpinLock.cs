using System.Runtime.CompilerServices;
namespace Natasha.CSharp.Extension.HotExecutor
{
    internal class HESpinLock
    {

        private int _lockCount = 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetLock()
        {
            return Interlocked.CompareExchange(ref _lockCount, 1, 0) == 0;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void GetAndWaitLock()
        {
            while (Interlocked.CompareExchange(ref _lockCount, 1, 0) != 0)
            {
                Thread.Sleep(20);
            }
        }

        public bool IsLocking()
        {
            return _lockCount == 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseLock()
        {

            _lockCount = 0;

        }
    }
}
