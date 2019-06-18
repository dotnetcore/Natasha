using System;
using System.Reflection;

namespace Natasha
{
    public class CtorNameTemplate<T>:MemberModifierTemplate<T>
    {
        public string NameScript;

        public CtorNameTemplate()
        {
            NameScript = "NatashaDynamicMethod";
        }

        public bool HashMethodName()
        {
            return NameScript != "NatashaDynamicMethod";
        }

        public T Name(string name)
        {
            NameScript = name;
            return Link;
        }
        public T Name(Type type)
        {
            NameScript = NameReverser.GetName(type);
            return Link;
        }
        public T Name<S>()
        {
            return Name(typeof(S));
        }
        public T Name(MethodInfo info)
        {
            NameScript =info.Name;
            return Link;
        }

        public override string Builder()
        {
            Script.Insert(0, NameScript);
            return base.Builder();
        }
    }
}
