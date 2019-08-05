using System;
using System.Text;

namespace Natasha.Template
{

    public class OnceMethodAttributeTemplate<T> : OopContentTemplate<T>
    {

        public readonly StringBuilder MethodAttributeScript;
        public StringBuilder OnceBuilder;
        public string MethodScript;


        public OnceMethodAttributeTemplate()
        {

            OnceBuilder = new StringBuilder();
            MethodAttributeScript = new StringBuilder();

        }




        public T MethodAttribute<A>(string ctorInfo = default)
        {

            return MethodAttribute(typeof(A), ctorInfo);

        }




        public T MethodAttribute(Type type, string ctorInfo = default)
        {

            UsingRecoder.Add(type);
            if (ctorInfo != default)
            {
                MethodAttributeScript.AppendLine($"[{type.GetDevelopName()}({ctorInfo})]");
            }
            else
            {
                MethodAttributeScript.AppendLine($"[{type.GetDevelopName()}]");
            }
            return Link;

        }




        public override T Builder()
        {

            OnceBuilder.Insert(0, MethodAttributeScript);
            MethodScript = OnceBuilder.ToString();
            OopBody(MethodScript);
            return base.Builder();

        }

    }

}
