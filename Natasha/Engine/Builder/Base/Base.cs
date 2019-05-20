using System.Text;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal LINK _link;
        internal StringBuilder _script = new StringBuilder();

        public string Script
        {
            get { return _script.ToString(); }
        }
    }
}
