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
        public T ClassName(string name)
        {
            NameScript = name;
            return Link;
        }
        public T ClassName(Type type)
        {
            NameScript = NameReverser.GetName(type);
            return Link;
        }
        public T ClassName<S>()
        {
            return ClassName(typeof(S));
        }
        public T ClassName(MethodInfo reflectMethodInfo)
        {
            NameScript = reflectMethodInfo.Name;
            return Link;
        }

        public override string Builder()
        {
            Script.Insert(0, "class "+NameScript);
            return base.Builder();
        }
    }
}
