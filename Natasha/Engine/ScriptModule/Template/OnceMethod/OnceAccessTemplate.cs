using System.Reflection;
using System.Text;

namespace Natasha
{
    public class OnceAccessTemplate<T>:ClassContentTemplate<T>
    {
        public StringBuilder OnceBuilder;
        public string MethodScript;
        public string OnceAccessScript;

        public OnceAccessTemplate()
        {
            OnceBuilder = new StringBuilder();
        }
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
        public override T Builder()
        {
            OnceBuilder.Insert(0,OnceAccessScript);
            MethodScript = OnceBuilder.ToString();
            ClassBody(MethodScript);
            return base.Builder();
        }
    }
}
