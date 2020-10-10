using System.Text;

namespace Natasha.CSharp.Template
{
    public class OopBodyTemplate<T> : OopConstraintTemplate<T> where T : OopBodyTemplate<T>, new()
    {

        public StringBuilder BodyScript;
        public StringBuilder OnceBodyScript;
        public OopBodyTemplate()
        {
            BodyScript = new StringBuilder();
            OnceBodyScript = new StringBuilder();
        }

        public T BodyAppend(string body)
        {

            BodyScript.Append(body);
            return Link;

        }
        public T BodyAppendLine(string body)
        {

            BodyScript.AppendLine(body);
            return Link;

        }
        public T Body(string body)
        {

            BodyScript.Clear();
            BodyScript.Append(body);
            return Link;

        }
        public T OnceBodyAppend(string body)
        {

            OnceBodyScript.AppendLine(body);
            return Link;

        }
        public T OnceBody(string body)
        {

            OnceBodyScript.Clear();
            OnceBodyScript.Append(body);
            return Link;

        }




        public override T BuilderScript()
        {
            // [Attribute]
            // [access] [modifier] [Name] [:Interface] 
            // [{this}]
            base.BuilderScript();
            _script.AppendLine("{");
            _script.Append(BodyScript);
            _script.Append(OnceBodyScript);
            _script.Append('}');
            OnceBodyScript.Clear();
            return Link;
        }


    }

}
