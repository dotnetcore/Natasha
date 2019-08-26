using Natasha.Complier;
using System.IO;

namespace Natasha
{
    public class AssemblyComplier : IComplier
    {
        public AssemblyComplier() { }
        public AssemblyComplier(string name) { Name = name; }




        public AssemblyDomain Domain
        {

            get { return ComplierInfos.Domain; }
            set { ComplierInfos.Domain = value; }

        }



        /// <summary>
        /// 添加一段代码
        /// </summary>
        /// <param name="text">代码</param>
        /// <returns></returns>
        public CompilationException Add(string text)
        {

            var info = ComplierInfos.Add(text);
            Exception.ErrorFlag = info.ErrorFlag;
            return info;

        }




        /// <summary>
        /// 使用构建模板
        /// </summary>
        /// <param name="template">构建模板</param>
        /// <returns></returns>
        public CompilationException Add(IScript template)
        {

            var info = ComplierInfos.Add(template.Script);
            Exception.ErrorFlag = info.ErrorFlag;
            return info;

        }




        /// <summary>
        /// 加载文件中的代码
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public CompilationException AddFile(string path)
        {

            if (File.Exists(path))
            {

                return Add(File.ReadAllText(path));

            }
            return default;

        }

    }

}
