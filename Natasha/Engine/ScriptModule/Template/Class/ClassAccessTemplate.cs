using System.Reflection;

namespace Natasha
{
    /// <summary>
    /// 类型模板
    /// </summary>
    /// <typeparam name="T">LINK类型</typeparam>
    public class ClassAccessTemplate<T> : NamespaceTemplate<T>
    {
        public string AccessScript;
        public T ClassAccess(MethodInfo reflectMethodInfo)
        {
            AccessScript = AccessReverser.GetAccess(reflectMethodInfo);
            return Link;
        }
        public T ClassAccess(AccessTypes enumAccess)
        {
            AccessScript = AccessReverser.GetAccess(enumAccess);
            return Link;
        }
        public T ClassAccess(string access)
        {
            AccessScript = access;
            return Link;
        }

        public override T Builder()
        {
            base.Builder();
            _script.Append(AccessScript);
            return Link;
        }
    }
}
