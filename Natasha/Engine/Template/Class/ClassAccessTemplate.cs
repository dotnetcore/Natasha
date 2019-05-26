using Natasha.Engine.Reverser;
using System.Reflection;

namespace Natasha
{
    public class ClassAccessTemplate<T> : NamespaceTemplate<T>
    {
        public string AccessScript;
        public T Access(MethodInfo access)
        {
            AccessScript = AccessReverser.GetAccess(access);
            return Link;
        }
        public T Access(AccessTypes access)
        {
            AccessScript = AccessReverser.GetAccess(access);
            return Link;
        }
        public T Access(string access)
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
