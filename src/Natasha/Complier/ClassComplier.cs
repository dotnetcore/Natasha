using Natasha.Complier;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Natasha
{
    public class ClassComplier : IComplier
    {
        public ClassComplier()
        {
            UseFileComplie(); 
        }


        /// <summary>
        /// 根据命名空间和类的位置获取类型
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="classIndex">命名空间里的第index个类</param>
        /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
        /// <returns></returns>
        public Type GetType(string content, int classIndex = 1, int namespaceIndex = 1)
        {
            
            //根据索引获取类名
            string className = ScriptComplieEngine.GetTreeAndClassName(content, classIndex, namespaceIndex).ClassName;

            
            //获取程序集
            Assembly assembly = GetAssemblyByScript(content);


            //获取类型
            return AssemblyOperator.Loader(assembly)[className];
        }
    }
}
