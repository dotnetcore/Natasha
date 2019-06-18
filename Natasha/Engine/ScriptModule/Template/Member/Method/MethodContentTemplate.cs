namespace Natasha
{
    public class MethodContentTemplate<T> : MethodParametersTemplate<T>
    {
        public string ContentScript;
        public T Body(string text)
        {
            ContentScript = text;
            return Link;
        }


        public override string Builder()
        {
            Script.Append(ContentScript);
            return base.Builder();
        }
    }
}
