namespace Natasha
{
    public class OnceMethodContentTemplate<T> : OnceMethodParametersTemplate<T>
    {
        public string OnceContentScript;
        public T MethodBody(string text)
        {
            OnceContentScript = text;
            return Link;
        }


        public override string Builder()
        {
            Script.Append(OnceContentScript);
            return base.Builder();
        }
    }
}
