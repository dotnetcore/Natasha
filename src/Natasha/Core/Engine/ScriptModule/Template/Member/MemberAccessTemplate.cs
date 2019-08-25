using System.Reflection;

namespace Natasha.Template
{
    public class MemberAccessTemplate<T> : MemberAttributeTemplate<T>
    {

        public string MemberAccessScript;


        public T MemberAccess(MethodInfo access)
        {

            MemberAccessScript = AccessReverser.GetAccess(access);
            return Link;

        }




        public T MemberAccess(AccessTypes access)
        {

            MemberAccessScript = AccessReverser.GetAccess(access);
            return Link;

        }

        


        public T MemberAccess(string access)
        {

            MemberAccessScript = access;
            if (!MemberAccessScript.EndsWith(" "))
            {
                MemberAccessScript += " ";
            }
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(MemberAccessScript);
            return Link;

        }

    }

}
