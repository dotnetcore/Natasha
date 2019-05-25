using Natasha.Engine.Utils;

namespace Natasha.Engine.Template
{
    public class TemplateRecoder<T>
    {
        public readonly TypeRecoder UsingRecoder;
        public T Link;
        public TemplateRecoder()
        {
            UsingRecoder = new TypeRecoder();
        }
    }
}
