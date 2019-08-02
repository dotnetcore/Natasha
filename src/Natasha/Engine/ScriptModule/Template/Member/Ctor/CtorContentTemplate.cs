namespace Natasha.Template
{
    public class CtorContentTemplate<T> : CtorParametersTemplate<T>
    {

        public string CtorContentScript;
        public T Body(string text)
        {
            CtorContentScript = text;
            return Link;
        }




        public override T Builder()
        {

            base.Builder();
            _script.Append(CtorContentScript);
            _script.Append("}");
            return Link;

        }

    }

}
