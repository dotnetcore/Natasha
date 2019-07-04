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

        public readonly ClassComplier Complier;
        public CtorTemplate CtorBuilder;
        public ClassBuilder() {
            Complier = new ClassComplier();
            Link = this;
        } 




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
        public override ClassBuilder Builder()
        {
            if (CtorBuilder!=null)
            {
               ClassBody(CtorBuilder.Builder()._script);
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
            return Complier.GetType(Builder().Script, classIndex, namespaceIndex);
        }
    }
}
