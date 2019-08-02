using System.Reflection;

namespace Natasha.Template
{
    public class MethodAsyncTemplate<T> : MemberModifierTemplate<T>
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
