using System.Reflection;

namespace Natasha.Template
{
    public class MethodUnsafeTemplate<T> : MemberModifierTemplate<T>
    {

        public string MethodUnsafeScript;

        public T UseUnsafe()
        {

            MethodUnsafeScript = "unsafe ";
            return Link;

        }




        public override T Builder()
        {

            _script.Insert(0, MethodUnsafeScript);
            return base.Builder();

        }

    }

}
