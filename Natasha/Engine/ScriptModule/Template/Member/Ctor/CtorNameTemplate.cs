using System;
using System.Reflection;

namespace Natasha
{
    public class CtorNameTemplate<T>:MemberModifierTemplate<T>
    {
        public string CtorNameScript;

        public CtorNameTemplate()
        {
            CtorNameScript = "NatashaDynamicMethod";
        }

        public bool HashMethodName()
        {
            return CtorNameScript != "NatashaDynamicMethod";
        }

        public T Name(string name)
        {
            CtorNameScript = name;
            return Link;
        }
        public T Name(Type type)
        {
            CtorNameScript = type.GetDevelopName();
            return Link;
        }
        public T Name<S>()
        {
            return Name(typeof(S));
        }
        public T Name(MethodInfo info)
        {
            CtorNameScript =info.Name;
            return Link;
        }

        public override T Builder()
        {
            base.Builder();
            _script.Append(CtorNameScript);
            return Link;
        }
    }
}
