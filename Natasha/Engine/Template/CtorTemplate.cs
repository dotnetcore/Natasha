using Natasha.Engine.Builder.Reverser;
using System;
using System.Text;

namespace Natasha.Engine.Template
{
    public class CtorTemplate:ParametersTemplate<CtorTemplate>
    {
        private string _class_name;
        public CtorTemplate()
        {
            Link = this;
        }

        public CtorTemplate ClassName(string name)
        {
            _class_name = name;
            return this;
        }
        public CtorTemplate ClassName(Type type)
        {
            _class_name = NameReverser.GetName(type);
            return this;
        }
        public CtorTemplate ClassName<T>()
        {
            return ClassName(typeof(T));
        }

        public string Builder()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{AccessLevel}{_class_name}{Parameters}{{{Content}}}");
            return sb.ToString();
        }
    }
}
