using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal StringBuilder _using = new StringBuilder();
        internal HashSet<string> _usings = new HashSet<string>();

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
