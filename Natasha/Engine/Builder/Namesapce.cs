using System;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal string _namespace;

        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespace">命名空间</param>
        /// <returns></returns>
        public LINK Namespace(string @namespace)
        {
            _namespace = @namespace;
            return _link;
        }
        public LINK Namespace<T>()
        {
            return Namespace(typeof(T));
        }
        public LINK Namespace(Type type)
        {
            _namespace = type.Namespace;
            return _link;
        }
    }
}
