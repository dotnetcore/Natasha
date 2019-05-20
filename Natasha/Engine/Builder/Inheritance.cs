using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal StringBuilder _inheritance = new StringBuilder();

        /// <summary>
        /// 设置继承
        /// </summary>
        /// <param name="types">类型</param>
        /// <returns></returns>
        public LINK Inheritance(params string[] types)
        {
            if (types != null && types.Length > 0)
            {
                if (_inheritance.Length == 0)
                {
                    _inheritance.Append(":");
                    _inheritance.Append(types[0]);
                    for (int i = 1; i < types.Length; i++)
                    {
                        _inheritance.Append($",{types[i]}");
                    }
                }
                else
                {
                    for (int i = 0; i < types.Length; i++)
                    {
                        _inheritance.Append($",{types[i]}");
                    }
                }
            }
            Using(types);
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
            if (_inheritance.Length == 0)
            {
                _inheritance.Append(":");
                _inheritance.Append(TypeReverser.Get(type));
            }
            else
            {
                _inheritance.Append($",{TypeReverser.Get(type)}");
            }
            Using(type);
            return _link;
        }
    }
}
