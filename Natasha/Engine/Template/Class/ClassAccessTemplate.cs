using Natasha.Engine.Reverser;
using System.Reflection;

namespace Natasha
{
    public class ClassAccessTemplate<T> : NamespaceTemplate<T>
    {
        public string AccessScript;
        public T ClassAccess(MethodInfo access)
        {
            AccessScript = AccessReverser.GetAccess(access);
            return Link;
        }
        public T ClassAccess(AccessTypes access)
        {
            AccessScript = AccessReverser.GetAccess(access);
            return Link;
        }
        public T ClassAccess(string access)
        {
            AccessScript = access;
            return Link;
        }

        public override string Builder()
        {
            Script.Insert(0, AccessScript);
            return base.Builder();
        }
    }
}
