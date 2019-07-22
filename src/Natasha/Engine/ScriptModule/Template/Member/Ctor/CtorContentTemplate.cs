namespace Natasha
{
    public class CtorContentTemplate<T> : CtorParametersTemplate<T>
    {
        public string ContentScript;
        public T Body(string text)
        {
            ContentScript = text;
            return Link;
        }


        public override T Builder()
        {
            base.Builder();
            _script.Append(ContentScript);
            _script.Append("}");
            return Link;
        }
    }
}
