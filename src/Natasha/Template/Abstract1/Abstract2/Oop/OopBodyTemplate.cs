using System.Text;

namespace Natasha.Template
{
    public class OopBodyTemplate<T> : InheritanceTemplate_TEMP<T> where T : OopBodyTemplate<T>, new()
    {

        public StringBuilder BodyScript;

        public OopBodyTemplate()
        {

            BodyScript = new StringBuilder();

        }




        public T Body(string body)
        {

            BodyScript.AppendLine(body);
            return Link;

        }




        public override T Builder()
        {
            // [Attribute]
            // [access] [modifier] [Name] [:Interface] 
            // [{this}]
            base.Builder();
            _script.AppendLine("{");
            _script.Append(BodyScript);
            _script.Append('}');
            return Link;
        }


    }

}
