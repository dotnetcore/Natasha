using Natasha.Template;
using System;

namespace Natasha
{
    /// <summary>
    /// 类构建器
    /// </summary>
    public class OopBuilder : OopContentTemplate<OopBuilder>
    {

        public readonly OopComplier Complier;
        public CtorTemplate CtorBuilder;


        public OopBuilder()
        {

            Complier = new OopComplier();
            Link = this;

        }




        /// <summary>
        /// 初始化器构建
        /// </summary>
        /// <param name="action">构建委托</param>
        /// <returns></returns>
        public OopBuilder Ctor(Action<CtorTemplate> action)
        {

            action(CtorBuilder = new CtorTemplate());


            return this;

        }




        /// <summary>
        /// 构建脚本
        /// </summary>
        /// <returns></returns>
        public override OopBuilder Builder()
        {

            _script.Clear();

            if (CtorBuilder != null)
            {
                CtorBuilder.Name(OopNameScript);
                OopBody(CtorBuilder.Builder()._script);

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
            switch (OopTypeEnum)
            {

                case OopType.Class:

                    return Complier.GetClassType(Builder().Script, classIndex, namespaceIndex);

                case OopType.Struct:

                    return Complier.GetStructType(Builder().Script, classIndex, namespaceIndex);

                case OopType.Interface:

                    return Complier.GetInterfaceType(Builder().Script, classIndex, namespaceIndex);
            }

            return null;
        }

    }

}
