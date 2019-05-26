using Natasha.Engine.Builder.Reverser;
using System;
using System.Reflection;

namespace Natasha
{
    public class ClassNameTemplate<T> : ClassModifierTemplate<T>
    {
        public string NameScript;
        public ClassNameTemplate()
        {
            NameScript = "N" + Guid.NewGuid().ToString("N");
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
            NameScript = info.Name;
            return Link;
        }

        public override string Builder()
        {
            Script.Insert(0, "class "+NameScript);
            return base.Builder();
        }
    }
}
