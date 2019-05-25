using System;
using System.Collections.Generic;
using System.Text;
using Natasha.Engine.Template;

namespace Natasha
{
    public class FastMethod : MethodBuilder<FastMethod>
    {
        public static FastMethod New
        {
            get { return new FastMethod(); }
        }
        public FastMethod()
        {
            _link = this;
            ClassTemplate
                .Access(AccessTypes.Public)
                .Modifier(Modifiers.Static);
            MethodTemplate
                .Access(AccessTypes.Public)
                .Modifier(Modifiers.Static);
        }

        public override FastMethod UseClassTemplate(Action<ClassTemplate> template)
        {
            template(ClassTemplate);
            return this;
        }
        public override FastMethod UseBodyTemplate(Action<MethodTemplate> template)
        {
            template(MethodTemplate);
            return this;
        }
    }
}
