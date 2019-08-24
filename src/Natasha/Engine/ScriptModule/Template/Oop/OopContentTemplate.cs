using System.Text;

namespace Natasha.Template
{
    public class OopContentTemplate<T>: OopFieldTemplate<T>
    {

        public StringBuilder OopContentScript;


        public OopContentTemplate() => OopContentScript = new StringBuilder();




        public T OopBody(StringBuilder text)
        {

            OopContentScript.Append(text);
            return Link;

        }




        public T OopBody(string text)
        {

            OopContentScript.Append(text);
            return Link;

        }




        public override T Builder()
        {

            base.Builder();
            _script.AppendLine(OopContentScript.ToString());
            _script.Append("}");

            if (!HiddenNamesapce)
            {
                _script.Append("}");
            }


            return Link;

        }

    }

}
