namespace Natasha.Template
{
    public class MethodBodyTemplate<T> : MethodParametersTemplate<T> where T : MethodBodyTemplate<T>, new()
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
