using System;
using System.Text;

namespace Natasha.Template
{

    public class OopAttributeTemplate<T> : NamespaceTemplate<T>
    {

        public readonly StringBuilder OopAttributeScript;


        public OopAttributeTemplate() => OopAttributeScript = new StringBuilder();



        public T OopAttribute(string attrInfo = default)
        {

            OopAttributeScript.AppendLine(attrInfo);
            return Link;

        }




        public T OopAttribute<A>(string ctorInfo = default)
        {

            return OopAttribute(typeof(A), ctorInfo);

        }





        public T OopAttribute(Type type, string ctorInfo = default)
        {

            UsingRecoder.Add(type);
            if (ctorInfo != default)
            {
                OopAttributeScript.AppendLine($"[{type.GetDevelopName()}({ctorInfo})]");
            }
            else
            {
                OopAttributeScript.AppendLine($"[{type.GetDevelopName()}]");
            }


            return Link;

        }




        public override T Builder()
        {

            _script.Insert(0, OopAttributeScript);
            return base.Builder();

        }

    }

}
