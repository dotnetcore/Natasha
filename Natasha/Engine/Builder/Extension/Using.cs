using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {

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
            if (type == null)
            {
                return _link;
            }
            return Using(type.Namespace);
        }

        public LINK Using(MethodInfo info)
        {
            if (info != null)
            {
                Using(info.ReturnType);
                Using(info.GetParameters().Select(item => item.ParameterType));
            }
            return _link;
        }
    }
}
