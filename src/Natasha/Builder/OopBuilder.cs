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


        #region 指定字符串域创建以及参数
        public static T Create(string domainName, ComplierResultTarget target = ComplierResultTarget.Stream, ComplierResultError error = ComplierResultError.None)
        {

            return Create(domainName, error, target);

        }

        public static T Create(string domainName, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            if (domainName == default || domainName.ToLower() == "default")
            {
                return Create(DomainManagment.Default, target, error);
            }
            else
            {
                return Create(DomainManagment.Create(domainName), target, error);
            }

        }
        #endregion
        #region 指定域创建以及参数
        public static T Create(AssemblyDomain domain, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(domain, target, error);

        }

        public static T Create(AssemblyDomain domain, ComplierResultTarget target = ComplierResultTarget.Stream, ComplierResultError error = ComplierResultError.None)
        {

            T instance = new T();
            instance.Complier.EnumCRError = error;
            instance.Complier.EnumCRTarget = target;
            instance.Complier.Domain = domain;
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static T Default()
        {

            return Create(DomainManagment.Default, ComplierResultTarget.Stream);

        }

        public static T Default(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Default, target, error);

        }

        public static T Default(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Default, target, error);

        }
        #endregion
        #region 随机域创建以及参数
        public static T Random()
        {

            return Create(DomainManagment.Random, ComplierResultTarget.Stream);

        }


        public static T Random(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Random, target, error);

        }


        public static T Random(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Random, target, error);

        }
        #endregion





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
            Complier.EnumCRTarget = ComplierResultTarget.File;
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
