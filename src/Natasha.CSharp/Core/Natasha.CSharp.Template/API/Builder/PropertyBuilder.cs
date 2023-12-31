using Natasha.CSharp.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.Builder
{

    public class PropertyBuilder : PropertyTemplate<PropertyBuilder>
    {

        public PropertyBuilder()
        {
            Link = this;
        }

    }

}
