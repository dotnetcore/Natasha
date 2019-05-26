using Natasha.Engine.Builder.Reverser;
using System;
using System.Text;

namespace Natasha
{
    public class CtorTemplate: MethodContentTemplate<CtorTemplate>,IScriptBuilder
    {
        public CtorTemplate()
        {
            Link = this;
        }
    }
}
