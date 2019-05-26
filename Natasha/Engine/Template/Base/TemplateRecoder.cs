using Natasha.Engine.Utils;
using System.Text;

namespace Natasha
{
    public class TemplateRecoder<T>:IScriptBuilder
    {
        public StringBuilder Script;
        public readonly TypeRecoder UsingRecoder;
        public T Link;
        public TemplateRecoder()
        {
            UsingRecoder = new TypeRecoder();
            Script = new StringBuilder();
        }

        public virtual string Builder()
        {
            return Script.ToString();
        }
    }
}
