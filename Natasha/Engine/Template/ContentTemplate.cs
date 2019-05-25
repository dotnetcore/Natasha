namespace Natasha.Engine.Template
{
    public class ContentTemplate<T>:AccessTemplate<T>
    {
        public string Content;
        public T Body(string text)
        {
            Content = text;
            return Link;
        }
    }
}
