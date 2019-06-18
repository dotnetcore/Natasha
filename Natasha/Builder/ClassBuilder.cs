using Natasha.Complier;
using System;
using System.Reflection;

namespace Natasha
{
    /// <summary>
    /// 类构建器
    /// </summary>
    public class ClassBuilder:ClassContentTemplate<ClassBuilder>
    {


        public CtorTemplate CtorBuilder;
        public ClassBuilder() => Link = this;




        /// <summary>
        /// 初始化器构建
        /// </summary>
        /// <param name="action">构建委托</param>
        /// <returns></returns>
        public ClassBuilder Ctor(Action<CtorTemplate> action)
        {
            action(CtorBuilder = new CtorTemplate());
            return this;
        }




        /// <summary>
        /// 构建脚本
        /// </summary>
        /// <returns></returns>
        public override string Builder()
        {
            if (CtorBuilder!=null)
            {
                Script.Append(CtorBuilder.Builder());
            }
            return base.Builder();
        }




        /// <summary>
        /// 精准获取动态类
        /// </summary>
        /// <param name="classIndex">类索引，1开始</param>
        /// <param name="namespaceIndex">命名空间索引，1开始</param>
        /// <returns></returns>
        public Type GetType(int classIndex = 1, int namespaceIndex = 1)
        {
            return GetType(Builder(), classIndex, namespaceIndex);
        }




        /// <summary>
        /// 根据命名空间和类的位置获取类型
        /// </summary>
        /// <param name="content">脚本内容</param>
        /// <param name="classIndex">命名空间里的第index个类</param>
        /// <param name="namespaceIndex">第namespaceIndex个命名空间</param>
        /// <returns></returns>
        public static Type GetType(string content, int classIndex = 1, int namespaceIndex = 1)
        {

            //根据索引获取类名
            string className = ScriptComplieEngine.GetTreeAndClassName(content, classIndex, namespaceIndex).ClassName;


            //获取程序集
            Assembly assembly = ScriptComplieEngine.FileComplier(content);


            //获取类型
            return AssemblyOperator.Loader(assembly)[className];
        }
    }
}
