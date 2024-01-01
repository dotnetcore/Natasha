using System.Text;

namespace Natasha.CSharp.Template
{
    public class NamespaceBodyTemplate<T> : OopBodyTemplate<T> where T : NamespaceTemplate<T>, new()
    {
        public StringBuilder NamespaceBodyScript;
        public StringBuilder NamespaceOnceBodyScript;
        public NamespaceBodyTemplate()
        {
            NamespaceBodyScript = new StringBuilder();
            NamespaceOnceBodyScript = new StringBuilder();
        }


        public T NamespaceBodyAppend(string body)
        {

            NamespaceBodyScript.Append(body);
            return Link;

        }


        public T NamespaceBodyAppendLine(string body)
        {

            NamespaceBodyScript.AppendLine(body);
            return Link;

        }


        public T NamespaceBody(string body)
        {

            NamespaceBodyScript.Clear();
            NamespaceBodyScript.Append(body);
            return Link;

        }


        public T NamespaceOnceBodyAppend(string body)
        {

            NamespaceOnceBodyScript.AppendLine(body);
            return Link;

        }


        public T NamespaceOnceBody(string body)
        {

            NamespaceOnceBodyScript.Clear();
            NamespaceOnceBodyScript.Append(body);
            return Link;

        }


        public override T BuilderScript()
        {
            // [comment]
            // [attribute]
            // [access] [modifier] [name] [:interface] 
            // {
            //      [{body}]
            // }
            // [{this}]
            base.BuilderScript();
            _script.Append(NamespaceBodyScript);
            _script.Append(NamespaceOnceBodyScript);
            NamespaceOnceBodyScript.Clear();
            return Link;
        }
    }
}
