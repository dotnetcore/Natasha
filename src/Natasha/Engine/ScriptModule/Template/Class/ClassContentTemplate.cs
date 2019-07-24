using System.Text;

namespace Natasha
{
    public class ClassContentTemplate<T>:FieldTemplate<T>
    {
        public StringBuilder ClassContentScript;
        public ClassContentTemplate()
        {
            ClassContentScript = new StringBuilder();
        }
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
