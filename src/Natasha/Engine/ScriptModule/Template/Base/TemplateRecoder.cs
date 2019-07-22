using Natasha.Utils;
using System.Text;

namespace Natasha
{
    /// <summary>
    /// 记录模板
    /// </summary>
    /// <typeparam name="T">LINK返回的类型</typeparam>
    public class TemplateRecoder<T>:IScriptBuilder<T>
    {

        public StringBuilder _script;
        public readonly TypeRecoder UsingRecoder;
        public T Link;
        public TemplateRecoder()
        {
            UsingRecoder = new TypeRecoder();
            _script = new StringBuilder(200);
        }

        public string Script
        {
            get { return _script.ToString(); }
        }


        /// <summary>
        /// 脚本构建
        /// </summary>
        /// <returns></returns>
        public virtual T Builder()
        {
            return Link;
        }
    }
}
