using System.Text;

namespace Natasha.Template
{

    public class OnceMethodContentTemplate<T> : OnceMethodParametersTemplate<T> where T : OnceMethodContentTemplate<T>, new()
    {

        public StringBuilder OnceContentScript;

        public OnceMethodContentTemplate()
        {
            OnceContentScript = new StringBuilder();
        }




        public T MethodBody(string text)
        {
            OnceContentScript.AppendLine(text);
            return Link;
        }




        public override T Builder()
        {
            OnceBuilder.Append(OnceContentScript);
            return base.Builder();
        }

    }

}
