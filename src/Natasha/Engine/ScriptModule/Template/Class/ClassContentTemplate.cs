using System.Text;

namespace Natasha.Template
{
    public class ClassContentTemplate<T>: ClassMethodTemplate<T>
    {

        public StringBuilder ClassContentScript;


        public ClassContentTemplate() => ClassContentScript = new StringBuilder();




        public T ClassBody(StringBuilder text)
        {

            ClassContentScript.Append(text);
            return Link;

        }




        public T ClassBody(string text)
        {

            ClassContentScript.Append(text);
            return Link;

        }




        public override T Builder()
        {

            _script.Clear();
            base.Builder();
            _script.Append(ClassContentScript);
            _script.Append("}");

            if (!HiddenNamesapce)
            {
                _script.Append("}");
            }


            return Link;

        }

    }

}
