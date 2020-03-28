using Natasha.Template;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.Builder
{

    public class PropertyBuilder : PropertyTemplate<PropertyBuilder>
    {

        public PropertyBuilder()
        {
            Link = this;
        }

    }

}
