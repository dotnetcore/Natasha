using System;
using System.Text;

namespace Natasha.Template
{
    public class MemberAttributeTemplate<T> : TemplateRecoder<T>
    {

        public readonly StringBuilder AttributeScript;


        public T Attribute<A>(string ctorInfo=default)
        {

            return Attribute(typeof(A), ctorInfo);

        }




        public T Attribute(Type type,string ctorInfo = default)
        {

            UsingRecoder.Add(type);
            if (ctorInfo!=default)
            {
                AttributeScript.AppendLine($"[{type.GetDevelopName()}({ctorInfo})]");
            }
            else
            {
                AttributeScript.AppendLine($"[{type.GetDevelopName()}]");
            }
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(AttributeScript);
            return Link;

        }

    }

}
