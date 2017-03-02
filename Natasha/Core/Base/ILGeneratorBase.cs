using Natasha.Cache;
using System.Reflection.Emit;
using System.Threading;

namespace Natasha.Core.Base
{
    //这个类主要用来获取对应EHandler环境下的IL
    //操作类都要继承它，因此每个操作类的实例都会得到当前EHandler环境下的IL
    public abstract class ILGeneratorBase
    {
        public ILGenerator ilHandler;
        public int ThreadId;
        public ILGeneratorBase()
        {
            //从线程缓存中获取IL
            ilHandler = ThreadCache.GetIL();
            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }
    }
}
