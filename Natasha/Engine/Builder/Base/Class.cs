using System;
using System.Text;

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

        private StringBuilder _modifier = new StringBuilder();

        public LINK Static()
        {
            _modifier.Append(" static");
            return _link;
        }
        public LINK Abstract()
        {
            _modifier.Append(" abstract");
            return _link;
        }
        public LINK Partial()
        {
            _modifier.Append(" partial");
            return _link;
        }

        private string _level;
        public LINK Public()
        {
            _level="public";
            return _link;
        }
        public LINK Private()
        {
            _level="private";
            return _link;
        }
        public  LINK Protected()
        {
            _level="protected";
            return _link;
        }
        
        public LINK Internal()
        {
            _level="internal";
            return _link;
        }
    }
}
