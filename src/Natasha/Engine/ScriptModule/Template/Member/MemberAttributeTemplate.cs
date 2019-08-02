using System;
using System.Text;

namespace Natasha.Template
{
    public class MemberAttributeTemplate<T> : TemplateRecoder<T>
    {

        public readonly StringBuilder MemberAttributeScript;


        public T Attribute<A>(string ctorInfo=default)
        {

            return Attribute(typeof(A), ctorInfo);

        }




        public T Attribute(Type type,string ctorInfo = default)
        {

            UsingRecoder.Add(type);
            if (ctorInfo!=default)
            {
                MemberAttributeScript.AppendLine($"[{type.GetDevelopName()}({ctorInfo})]");
            }
            else
            {
                MemberAttributeScript.AppendLine($"[{type.GetDevelopName()}]");
            }
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(MemberAttributeScript);
            return Link;

        }

    }

}
