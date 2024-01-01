using System.Text;

namespace Natasha.CSharp.Template
{
    public class OopBodyTemplate<T> : OopConstraintTemplate<T> where T : OopBodyTemplate<T>, new()
    {

        public readonly StringBuilder BodyScript;
        public readonly StringBuilder OnceBodyScript;
        private bool _bracket;
        public OopBodyTemplate()
        {
            _bracket = true;
            BodyScript = new StringBuilder();
            OnceBodyScript = new StringBuilder();
        }
        public T NoBodyBracket()
        {
            _bracket = false;
            return Link;
        }
        public T HasBodyBracket()
        {
            _bracket = true;
            return Link;
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
            // [comment]
            // [attribute]
            // [access] [modifier] [name] [:interface] 
            // [{this}]
            base.BuilderScript();
            if (_bracket)
            {
                _script.AppendLine("{");
                _script.Append(BodyScript);
                _script.Append(OnceBodyScript);
                _script.Append('}');
            }
            else
            {
                _script.Append(BodyScript);
                _script.Append(OnceBodyScript);
            }

            OnceBodyScript.Clear();
            return Link;
        }


    }

}
