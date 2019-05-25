using Natasha.Engine.Builder.Reverser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal readonly StringBuilder _fields = new StringBuilder();
        internal readonly HashSet<string> _fieldsSet = new HashSet<string>();

        public LINK Field(string level,Type type,string name)
        {
            Using(type);
            if (!_fieldsSet.Contains(name))
            {
                _fields.Append($"{level} {NameReverser.GetName(type)} {name};");
            }
            return _link;
        }
    }
}
