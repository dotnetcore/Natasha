using System.Reflection;

namespace Natasha.Template
{
    public class OnceMethodAsyncTemplate<T> : OnceMethodModifierTemplate<T>
    {

        public string OnceAsyncScript;

        public T UseAsync()
        {

            OnceAsyncScript = "async ";
            return Link;

        }




        public T Async(MethodInfo info)
        {

            OnceAsyncScript = AsyncReverser.GetAsync(info);
            return Link;

        }




        public override T Builder()
        {

            OnceBuilder.Insert(0, OnceAsyncScript);
            return base.Builder();

        }

    }
}
