using Natasha.Engine.Reverser;
using System.Reflection;

namespace Natasha.Engine.Template
{
    public class AccessTemplate<T> : TemplateRecoder<T>
    {
        public string AccessLevel;
        public T Access(MethodInfo access)
        {
            AccessLevel = AccessReverser.GetAccess(access);
            return Link;
        }
        public T Access(AccessTypes access)
        {
            AccessLevel = AccessReverser.GetAccess(access);
            return Link;
        }
        public T Access(string access)
        {
            AccessLevel = access;
            return Link;
        }
    }
}
