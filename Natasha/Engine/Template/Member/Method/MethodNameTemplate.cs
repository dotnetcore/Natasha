using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class MethodNameTemplate<T>:MethodReturnTemplate<T>
    {
        public string NameScript;

        public MethodNameTemplate()
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
