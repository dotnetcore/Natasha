using System.Reflection;

namespace Natasha.Template
{
    public class OnceMethodAccessTemplate<T>: OnceMethodAttributeTemplate<T>
    {
        public string OnceAccessScript;


        public T MethodAccess(MethodInfo access)
        {

            OnceAccessScript = AccessReverser.GetAccess(access);
            return Link;

        }




        public T MethodAccess(AccessTypes access)
        {

            OnceAccessScript = AccessReverser.GetAccess(access);
            return Link;

        }




        public T MethodAccess(string access)
        {

            OnceAccessScript = access;
            if (OnceAccessScript.EndsWith(" "))
            {
                OnceAccessScript += " ";
            }
            return Link;

        }




        public override T Builder()
        {

            OnceBuilder.Insert(0,OnceAccessScript);
            return base.Builder();

        }

    }

}
