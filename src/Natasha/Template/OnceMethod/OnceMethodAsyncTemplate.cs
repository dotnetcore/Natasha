using Natasha.Reverser;
using System.Reflection;

namespace Natasha.Template
{
    public class OnceMethodAsyncTemplate<T> : OnceMethodUnsafeTemplate<T> where T : OnceMethodAsyncTemplate<T>, new()
    {

        public string OnceAsyncScript;

        public T UseAsync()
        {

            OnceAsyncScript = "async ";
            return Link;

        }




        public T AsyncFrom(MethodInfo info)
        {

            OnceAsyncScript = AsyncReverser.GetAsync(info);
            return Link;

        }




        public T AsyncFrom<S>(string info)
        {

            var method = typeof(S).GetMethod(info);
            OnceAsyncScript = AsyncReverser.GetAsync(method);
            return Link;

        }




        public override T Builder()
        {

            OnceBuilder.Insert(0, OnceAsyncScript);
            return base.Builder();

        }

    }
}
