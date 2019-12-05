using System.Collections.Generic;
using System.Text;

namespace Natasha.Template
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
            get { _script.Clear(); Builder();  return _script.ToString(); }
        }

        public virtual HashSet<string> Usings => default;




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
