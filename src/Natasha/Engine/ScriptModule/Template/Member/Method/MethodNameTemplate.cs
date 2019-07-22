using System;
using System.Reflection;

namespace Natasha
{
    public class MethodNameTemplate<T>:MethodReturnTemplate<T>
    {
        public string MethodNameScript;

        public MethodNameTemplate()
        {
            MethodNameScript = "NatashaDynamicMethod";
        }

        public bool HashMethodName()
        {
            return MethodNameScript != "NatashaDynamicMethod";
        }

        public T Name(string name)
        {
            MethodNameScript = name;
            return Link;
        }
        public T Name(Type type)
        {
            MethodNameScript = type.GetDevelopName();
            return Link;
        }
        public T Name<S>()
        {
            return Name(typeof(S));
        }
        public T Name(MethodInfo info)
        {
            MethodNameScript =info.Name;
            return Link;
        }

        public override T Builder()
        {
            base.Builder();
            _script.Append(MethodNameScript);
            return Link;
        }
    }
}
