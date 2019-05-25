using Natasha.Engine.Reverser;
using System;
using System.Reflection;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal string _class_name = "N" + Guid.NewGuid().ToString("N");

        /// <summary>
        /// 设置类名
        /// </summary>
        /// <param name="class">类名</param>
        /// <returns></returns>
        public LINK ClassName(string @class)
        {
            _class_name = @class;
            return _link;
        }
        private string _level;
        public LINK Access(AccessTypes access)
        {
            _level = AccessReverser.GetAccess(access);
            return _link;
        }
        public LINK Access(MethodInfo access)
        {
            _level = AccessReverser.GetAccess(access);
            return _link;
        }
        public LINK Access(Type access)
        {
            _level = AccessReverser.GetAccess(access);
            return _link;
        }
        private string _modifier;
        public LINK Modifier(Modifiers modifier)
        {
            _modifier = ModifierReverser.GetModifier(modifier);
            return _link;
        }
        public LINK Modifier(MethodInfo modifier)
        {
            _modifier = ModifierReverser.GetModifier(modifier);
            return _link;
        }
        public LINK Modifier(Type modifier)
        {
            _modifier = ModifierReverser.GetModifier(modifier);
            return _link;
        }
    }
}
