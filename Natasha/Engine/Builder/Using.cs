using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal StringBuilder _using = new StringBuilder();
        internal HashSet<string> _usings = new HashSet<string>();


        /// <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">命名空间</param>
        /// <returns></returns>
        public LINK Using(params string[] namespaces)
        {
            for (int i = 0; i < namespaces.Length; i++)
            {
                Using(namespaces[i]);
            }
            return _link;
        }
        // <summary>
        /// 设置命名空间
        /// </summary>
        /// <param name="namespaces">类型</param>
        /// <returns></returns>
        public LINK Using(params Type[] namespaces)
        {
            for (int i = 0; i < namespaces.Length; i++)
            {
                Using(namespaces[i]);
            }
            return _link;
        }
        public LINK Using(IEnumerable<Type> namespaces)
        {
            foreach (var item in namespaces)
            {
                Using(item);
            }
            return _link;
        }
        public LINK Using<T>()
        {
            Using(typeof(T));
            return _link;
        }
        public LINK Using(Type type)
        {
            if (type==null)
            {
                return _link;
            }
            return Using(type.Namespace);
        }
        public LINK Using(string type)
        {
            if (type != null)
            {
                if (!_usings.Contains(type))
                {
                    _usings.Add(type);
                    _using.Append($"using {type};");
                }
            }
            return _link;
        }

    }
}
