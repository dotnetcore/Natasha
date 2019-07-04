using System;
using System.Reflection;

namespace Natasha
{
    public class OnceMethodNameTemplate<T>: OnceMethodReturnTemplate<T>
    {
        public string MethodNameScript;

        public OnceMethodNameTemplate()
        {
            MethodNameScript = "NatashaDynamicMethod";
        }

        public bool HashMethodName()
        {
            return MethodNameScript != "NatashaDynamicMethod";
        }

        public T MethodName(string name)
        {
            MethodNameScript = name;
            return Link;
        }
        public T MethodName(Type type)
        {
            MethodNameScript = type.GetDevelopName();
            return Link;
        }
        public T MethodName<S>()
        {
            return MethodName(typeof(S));
        }
        public T MethodName(MethodInfo info)
        {
            MethodNameScript =info.Name;
            return Link;
        }

        public override T Builder()
        {
            OnceBuilder.Insert(0, MethodNameScript);
            return base.Builder();
        }
    }
}
