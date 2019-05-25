using Natasha.Engine.Builder.Reverser;
using System;
using System.Text;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        /// <summary>
        /// 设置继承
        /// </summary>
        /// <param name="types">类型</param>
        /// <returns></returns>
        public LINK Inheritance(params string[] types)
        {
            if (types != null && types.Length > 0)
            {
                for (int i = 1; i < types.Length; i++)
                {
                    Inheritance(types[i]);
                }
            }
            return _link;
        }
        public LINK Inheritance(params Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                Inheritance(types[i]);
            }
            return _link;
        }
        public LINK Inheritance<T>()
        {
            return Inheritance(typeof(T));
        }
        public LINK Inheritance(Type type)
        {
            if (type == null)
            {
                return _link;
            }
            Using(type);
            return Inheritance(NameReverser.GetName(type));
        }
    }
}
