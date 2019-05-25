using Natasha.Engine.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class ClassTemplate<T> : BuilderStandard<T>
    {
    }
    public class ClassTemplate: BuilderStandard<ClassTemplate>
    {
        public ClassTemplate():base()
        {
            _link = this;
        }
    }
}
