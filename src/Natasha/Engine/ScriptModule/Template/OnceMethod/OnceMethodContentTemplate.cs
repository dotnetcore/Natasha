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


        public override T Builder()
        {
            OnceBuilder.Append(OnceContentScript);
            return base.Builder();
        }
    }
}
