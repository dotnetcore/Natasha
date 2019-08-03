using System;

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
        public Type GetClassType(string content, int classIndex = 1, int namespaceIndex = 1)
        {

            //根据索引获取类名
            string className = ScriptHelper.GetClassName(content, classIndex, namespaceIndex);
            return GetTypeByScript(content, className);

        }

        public Type GetStructType(string content, int structIndex = 1, int namespaceIndex = 1)
        {

            //根据索引获取类名
            string structName = ScriptHelper.GetStructName(content, structIndex, namespaceIndex);
            return GetTypeByScript(content, structName);

        }

    }

}
