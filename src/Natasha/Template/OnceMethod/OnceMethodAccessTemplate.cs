using Natasha.Reverser;
using Natasha.Reverser.Model;
using System;
using System.Reflection;

namespace Natasha.Template
{
    public class OnceMethodAccessTemplate<T> : OnceMethodAttributeTemplate<T> where T : OnceMethodAccessTemplate<T>, new()
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




        public T PublicMember
        {
            get { OnceAccessScript = "public "; return Link; }
        }
        public T PrivateMember
        {
            get { OnceAccessScript = "private "; return Link; }
        }
        public T ProtectedMember
        {
            get { OnceAccessScript = "protected "; return Link; }
        }
        public T InternalMember
        {
            get { OnceAccessScript = "internal "; return Link; }
        }
        public T ProtectedInternalMember
        {
            get { OnceAccessScript = "protected internal "; return Link; }
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

            OnceBuilder.Insert(0, OnceAccessScript);
            return base.Builder();

        }

    }

}
