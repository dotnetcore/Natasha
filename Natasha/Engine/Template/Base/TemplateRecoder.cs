using Natasha.Utils;
using System.Text;

namespace Natasha
{
    /// <summary>
    /// 记录模板
    /// </summary>
    /// <typeparam name="T">LINK返回的类型</typeparam>
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




        /// <summary>
        /// 脚本构建
        /// </summary>
        /// <returns></returns>
        public virtual string Builder()
        {
            return Script.ToString();
        }
    }
}
