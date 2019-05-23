using Natasha.Engine.Builder.Reverser;
using System;
using System.Text;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal readonly StringBuilder _inheritance = new StringBuilder();

        public LINK Inheritance(string type)
        {
            if (_inheritance.Length > 0)
            {
                _inheritance.Append($",{type}");
            }
            else
            {
                _inheritance.Append(":");
                _inheritance.Append(type);
            }
            return _link;
        }

    }
}
