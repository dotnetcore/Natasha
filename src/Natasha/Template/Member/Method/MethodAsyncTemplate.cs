using System.Reflection;
using Natasha.Reverser;

namespace Natasha.Template
{
    public class MethodAsyncTemplate<T> : MethodUnsafeTemplate<T>
    {

        public string MethodAsyncScript;

        public T UseAsync()
        {

            MethodAsyncScript = "async ";
            return Link;

        }




        public T Async(MethodInfo info)
        {

            MethodAsyncScript = AsyncReverser.GetAsync(info);
            return Link;

        }




        public override T Builder()
        {

            _script.Insert(0, MethodAsyncScript);
            return base.Builder();

        }

    }

}
