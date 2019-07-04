namespace Natasha
{
    public class MethodBodyTemplate<T> : MethodParametersTemplate<T>
    {
        public string MethodBodyScript;
        public T Body(string text)
        {
            MethodBodyScript = text;
            return Link;
        }


        public override T Builder()
        {
            base.Builder();
            _script.Append(MethodBodyScript);
            _script.Append("}");
            return Link;
        }
    }
}
