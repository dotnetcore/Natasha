using System;
namespace Natasha.Engine.Builder
{
    public abstract partial class BuilderStandard<LINK>
    {
        internal string _class_name = "N" + Guid.NewGuid().ToString("N");

        /// <summary>
        /// 设置类名
        /// </summary>
        /// <param name="class">类名</param>
        /// <returns></returns>
        public LINK ClassName(string @class)
        {
            _class_name = @class;
            return _link;
        }
    }
}
