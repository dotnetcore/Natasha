using Microsoft.CodeAnalysis;
using Natasha.Core.Compiler;
using System.IO;

namespace Natasha
{
    public class AssemblyCompiler : ICompiler
    {

        public AssemblyCompiler() {}
        public AssemblyCompiler(string name) 
        { 
            AssemblyName = name;
            CustomerUsingShut = true;
        }

        public bool CustomerUsingShut;




        public AssemblyCompiler AutoUsing()
        {

            CustomerUsingShut = false;
            return this;

        }
        public AssemblyCompiler CurstomeUsing()
        {
            CustomerUsingShut = true;
            return this;
        }




        /// <summary>
        /// 添加一段代码
        /// </summary>
        /// <param name="text">代码</param>
        /// <returns></returns>
        public CompilationException Add(string text)
        {

            var info = SyntaxInfos.Add(text);
            CompileException.ErrorFlag = info.ErrorFlag;
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
            CompileException.ErrorFlag = info.ErrorFlag;
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
            CompileException.ErrorFlag = info.ErrorFlag;
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
