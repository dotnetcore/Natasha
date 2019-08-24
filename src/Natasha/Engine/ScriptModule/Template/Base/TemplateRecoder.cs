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
#if NETCOREAPP3_0
        public AssemblyDomain Domain;
#endif
        public T Link;


        public TemplateRecoder()
        {

            UsingRecoder = new TypeRecoder();
            _script = new StringBuilder(200);

        }



#if NETCOREAPP3_0
        public T InDomain(AssemblyDomain domain)
        {

            Domain = domain;
            return Link;

        }

#endif





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
