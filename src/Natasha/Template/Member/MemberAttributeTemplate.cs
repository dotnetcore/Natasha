using System;
using System.Text;

namespace Natasha.Template
{
    public class MemberAttributeTemplate<T> : TemplateRecoder<T>
    {

        public readonly StringBuilder MemberAttributeScript;

        public MemberAttributeTemplate() => MemberAttributeScript = new StringBuilder();




        public T MemberAttribute(string attrInfo = default)
        {

            MemberAttributeScript.AppendLine(attrInfo);
            return Link;

        }




        public T MemberAttribute<A>(string ctorInfo=default)
        {

            return MemberAttribute(typeof(A), ctorInfo);

        }




        public T MemberAttribute(Type type,string ctorInfo = default)
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
