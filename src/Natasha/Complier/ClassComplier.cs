using Natasha.Complier;
using System;
using System.Reflection;

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

            //获取类型
            return RuntimeComplier.GetClassType(content,classIndex,namespaceIndex);

        }
        public Type GetStructType(string content, int classIndex = 1, int namespaceIndex = 1)
        {

            //获取类型
            return RuntimeComplier.GetStructType(content, classIndex, namespaceIndex);

        }
    }

}
