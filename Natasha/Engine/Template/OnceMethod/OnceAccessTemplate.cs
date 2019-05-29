using System.Reflection;

namespace Natasha
{
    public class OnceAccessTemplate<T>:ClassContentTemplate<T>
    {
        public string MethodScript;
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
            return Link;
        }
        public override string Builder()
        {
            Script.Insert(0, OnceAccessScript);
            MethodScript = Script.ToString();
            return base.Builder();
        }
    }
}
