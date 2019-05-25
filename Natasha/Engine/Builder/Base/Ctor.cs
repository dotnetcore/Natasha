using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        //public MethodBuilder<object> CtorBuilder = new MethodBuilder<object>();

        private string _ctor;
        public LINK Ctor(StringBuilder ctor)
        {
            _ctor = ctor.ToString();
            return _link;
        }
        public LINK Ctor(string ctor)
        {
            _ctor = ctor;
            return _link;
        }

    }
}
