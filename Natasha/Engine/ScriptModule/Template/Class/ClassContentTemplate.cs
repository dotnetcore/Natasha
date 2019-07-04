using System.Text;

namespace Natasha
{
    public class ClassContentTemplate<T>:FieldTemplate<T>
    {
        public StringBuilder ContentScript;
        public ClassContentTemplate()
        {
            ContentScript = new StringBuilder();
        }
        public T ClassBody(StringBuilder text)
        {
            ContentScript.Append(text);
            return Link;
        }
        public T ClassBody(string text)
        {
            ContentScript.Append(text);
            return Link;
        }
        public override T Builder()
        {
            base.Builder();
            _script.Append(ContentScript);
            _script.Append("}");
            if (!HiddenNamesapce)
            {
                _script.Append("}");
            }
            return Link;
        }
    }
}
