using Natasha.Core;
using Natasha.Template;
using System;

namespace Natasha.Builder
{

    public class OopBuilder<T> : OopContentTemplate<T> where T : OopBuilder<T>, new()
    {

        public readonly AssemblyComplier Complier;
        public CtorBuilder CtorBuilder;


        public OopBuilder()
        {

            Complier = new AssemblyComplier();

        }



        public static T Default
        {

            get { return Create(); }

        }



        /// <summary>
        /// 如果参数为空，则使用默认域
        /// 如果参数不为空，则创建以参数为名字的独立域
        /// </summary>
        /// <param name="domainName">域名</param>
        /// <returns></returns>
        public static T Create(string domainName = default, bool complieInFile = default)
        {

            T instance = new T();
            instance.Complier.ComplieInFile = complieInFile;


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
        public static T Create(AssemblyDomain domain, bool complieInFile = default)
        {

            T instance = new T();
            instance.Complier.ComplieInFile = complieInFile;
            instance.Complier.Domain = domain;
            return instance;

        }
        /// <summary>
        /// 创建一个随机的域
        /// </summary>
        /// <returns></returns>
        public static T Random(bool complieInFile = default)
        {

            T instance = new T();
            instance.Complier.ComplieInFile = complieInFile;
            instance.Complier.Domain = DomainManagment.Random;
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



        public T UseType(Type type, string TypeName)
        {

            if (type.IsEnum)
            {

                ChangeToEnum();

            }
            else if (type.IsInterface)
            {

                ChangeToInterface();

            }
            else if (type.IsValueType)
            {

                ChangeToStruct();

            }
            else
            {

                ChangeToClass();

            }

            OopName(TypeName!=default? TypeName:type.GetDevelopName())
            .Inheritance(type.BaseType)
            .Inheritance(type.GetInterfaces())
            .Namespace(type.Namespace)
           . OopAccess(type)
           . OopModifier(type);
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
        /// 将结果编译进文件
        /// </summary>
        /// <returns></returns>
        public T UseFileComplie()
        {
            Complier.ComplieInFile = true;
            return Link;
        }




        /// <summary>
        /// 精准获取动态类
        /// </summary>
        /// <param name="classIndex">类索引，1开始</param>
        /// <param name="namespaceIndex">命名空间索引，1开始</param>
        /// <returns></returns>
        public Type GetType(int classIndex = 1, int namespaceIndex = 1)
        {

            Complier.Add(this);
            string name = default;
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
