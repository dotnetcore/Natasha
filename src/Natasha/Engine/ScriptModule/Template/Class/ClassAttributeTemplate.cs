using System;
using System.Text;

namespace Natasha.Template
{

    public class ClassAttributeTemplate<T> : NamespaceTemplate<T>
    {

        public readonly StringBuilder ClassAttributeScript;


        public ClassAttributeTemplate() => ClassAttributeScript = new StringBuilder();




        public T ClassAttribute<A>(string ctorInfo = default)
        {

            return ClassAttribute(typeof(A), ctorInfo);

        }





        public T ClassAttribute(Type type, string ctorInfo = default)
        {

            UsingRecoder.Add(type);
            if (ctorInfo != default)
            {
                ClassAttributeScript.AppendLine($"[{type.GetDevelopName()}({ctorInfo})]");
            }
            else
            {
                ClassAttributeScript.AppendLine($"[{type.GetDevelopName()}]");
            }


            return Link;

        }




        public override T Builder()
        {

            _script.Insert(0, ClassAttributeScript);
            return base.Builder();

        }

    }

}
