namespace Natasha
{
    public class ClassContentTemplate<T>:FieldTemplate<T>
    {
        public string ContentScript;
        public T ClassBody(string text)
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
