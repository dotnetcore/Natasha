using System.Reflection;

namespace Natasha
{
    public class MemberAccessTemplate<T> : TemplateRecoder<T>
    {
        public string AccessScript;
        public T MemberAccess(MethodInfo access)
        {
            AccessScript = AccessReverser.GetAccess(access);
            return Link;
        }
        public T MemberAccess(AccessTypes access)
        {
            AccessScript = AccessReverser.GetAccess(access);
            return Link;
        }
        public T MemberAccess(string access)
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
