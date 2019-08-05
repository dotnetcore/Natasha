using System.Text;

namespace Natasha.Template
{
    public class OopContentTemplate<T>: OopMethodTemplate<T>
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

            _script.Clear();
            base.Builder();
            _script.Append(OopContentScript);
            _script.Append("}");

            if (!HiddenNamesapce)
            {
                _script.Append("}");
            }


            return Link;

        }

    }

}
