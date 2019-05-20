namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal string _text;
        /// <summary>
        /// 写内容
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public LINK Body(string text)
        {
            _text = text;
            return _link;
        }
    }
}
