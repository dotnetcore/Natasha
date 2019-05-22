using System.Text;

namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal StringBuilder _text;
        /// <summary>
        /// 写内容
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public LINK Body(string text)
        {
            _text =new StringBuilder(text);
            return _link;
        }
        public LINK Body(StringBuilder text)
        {
            _text = text;
            return _link;
        }
    }
}
