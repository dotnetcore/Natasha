using Microsoft.CodeAnalysis;
using Natasha.Core;
using Natasha.Core.Complier;
using System.IO;

namespace Natasha
{
    public class AssemblyComplier : IComplier
    {

        public AssemblyComplier() {}
        public AssemblyComplier(string name) { AssemblyName = name; }


        

        /// <summary>
        /// 添加一段代码
        /// </summary>
        /// <param name="text">代码</param>
        /// <returns></returns>
        public CompilationException Add(string text)
        {

            var info = SyntaxInfos.Add(text);
            ComplieException.ErrorFlag = info.ErrorFlag;
            return info;

        }



        /// <summary>
        /// 增加语法树
        /// </summary>
        /// <param name="node">语法树</param>
        /// <returns></returns>
        public CompilationException Add(SyntaxTree node)
        {

            var info = SyntaxInfos.Add(node);
            ComplieException.ErrorFlag = info.ErrorFlag;
            return info;

        }




        /// <summary>
        /// 使用构建模板
        /// </summary>
        /// <param name="template">构建模板</param>
        /// <returns></returns>
        public CompilationException Add(IScript template)
        {

            var info = SyntaxInfos.Add(template.Script, template.Usings);
            ComplieException.ErrorFlag = info.ErrorFlag;
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
