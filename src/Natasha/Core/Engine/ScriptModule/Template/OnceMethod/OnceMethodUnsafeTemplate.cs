using System.Reflection;

namespace Natasha.Template
{
    public class OnceMethodUnsafeTemplate<T> : OnceMethodModifierTemplate<T>
    {

        public string OnceUnsafeScript;

        public T UseUnsafe()
        {

            OnceUnsafeScript = "unsafe ";
            return Link;

        }




        public override T Builder()
        {

            OnceBuilder.Insert(0, OnceUnsafeScript);
            return base.Builder();

        }

    }
}
