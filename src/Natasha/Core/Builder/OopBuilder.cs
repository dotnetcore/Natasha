using Natasha.Template;
using System;

namespace Natasha.Builder
{

    public class OopBuilder<T> : OopContentTemplate<T> where T: OopBuilder<T>,new()
    {

        public readonly AssemblyComplier Complier;
        public CtorBuilder CtorBuilder;


        public OopBuilder()
        {

            Complier = new AssemblyComplier();

        }


        /// <summary>
        /// 如果参数为空，则使用默认域
        /// 如果参数不为空，则创建以参数为名字的独立域
        /// </summary>
        /// <param name="domainName">域名</param>
        /// <returns></returns>
        public static T Create(string domainName = default)
        {
            T instance = new T();
            if (domainName == default)
            {
                instance.Complier.Domain = DomainManagment.Default;
            }
            else
            {
                instance.Complier.Domain = DomainManagment.Create(domainName);
            }

            return instance;
        }
        /// <summary>
        /// 使用一个现成的域
        /// </summary>
        /// <param name="domain">域</param>
        /// <returns></returns>
        public static T Create(AssemblyDomain domain)
        {
            T instance = new T();
            instance.Complier.Domain = domain;
            return instance;
        }
        /// <summary>
        /// 创建一个随机的域
        /// </summary>
        /// <returns></returns>
        public static T Random()
        {
            T instance = new T();
            instance.Complier.Domain = DomainManagment.Random();
            return instance;
        }





        /// <summary>
        /// 使用随机域
        /// </summary>
        /// <returns></returns>
        public T UseRandomDomain()
        {

            Complier.Domain = DomainManagment.Create("N" + Guid.NewGuid().ToString("N"));
            return Link;

        }




        /// <summary>
        /// 使用随机域
        /// </summary>
        /// <returns></returns>
        public T UseDomain(string domainName)
        {

            Complier.Domain = DomainManagment.Create(domainName);
            return Link;

        }
        public T UseDomain(AssemblyDomain domain)
        {

            Complier.Domain = domain;
            return Link;

        }



        /// <summary>
        /// 初始化器构建
        /// </summary>
        /// <param name="action">构建委托</param>
        /// <returns></returns>
        public T Ctor(Action<CtorBuilder> action)
        {

            action(CtorBuilder = new CtorBuilder());
            return Link;

        }



        public T Method(Action<MethodBuilder> action)
        {
            var handler = new MethodBuilder();
            action?.Invoke(handler);
            OopBody(handler.Script);
            return Link;
        }




        /// <summary>
        /// 构建脚本
        /// </summary>
        /// <returns></returns>
        public override T Builder()
        {

            if (CtorBuilder != null)
            {
                CtorBuilder.Name(OopNameScript);
                OopBody(CtorBuilder.Script);

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

            Complier.Add(this, Usings);
            string name=default;
            switch (OopTypeEnum)
            {

                case OopType.Class:

                    name = ScriptHelper.GetClassName(Script, classIndex, namespaceIndex);
                    break;

                case OopType.Struct:

                    name = ScriptHelper.GetStructName(Script, classIndex, namespaceIndex);
                    break;

                case OopType.Interface:

                    name = ScriptHelper.GetInterfaceName(Script, classIndex, namespaceIndex);
                    break;

                case OopType.Enum:

                    name = ScriptHelper.GetEnumName(Script, classIndex, namespaceIndex);
                    break;
            }

            return Complier.GetType(name);
        }

    }

}
