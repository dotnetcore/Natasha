using System;
using System.Reflection;

namespace Natasha
{
    public class ClassBuilder
    {
        static ClassBuilder()
        {
            
        }
        /// <summary>
        /// 根据命名空间和类的位置获取类型
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="index">命名空间里的第index个类</param>
        /// <param name="namespaceIndex">第x个命名空间</param>
        /// <returns></returns>
        public static Type GetType(string content, int index = 1, int namespaceIndex = 1)
        {
            Assembly assembly = null;
            string className = ScriptComplier.GetClassName(content, index, namespaceIndex);
            assembly = ScriptComplier.Complier(content, className);
            return AssemblyOperator.Loader(assembly)[className];
        }
    }
}
