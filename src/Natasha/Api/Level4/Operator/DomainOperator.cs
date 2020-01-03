using Natasha.Core;
using Natasha.Operator;
using System;

namespace Natasha
{ 

    public class DomainOperator
    {

        private OopOperator _operator;
        private readonly AssemblyComplier Complier;
        public DomainOperator()
        {
            _operator = new OopOperator();
            Complier = new AssemblyComplier();
        }


        #region 指定字符串域创建以及参数
        public static DomainOperator Create(string domainName, ComplierResultTarget target = ComplierResultTarget.Stream, ComplierResultError error = ComplierResultError.None)
        {

            return Create(domainName, error, target);

        }

        public static DomainOperator Create(string domainName, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
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
        public static DomainOperator Create(AssemblyDomain domain, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(domain, target, error);

        }

        public static DomainOperator Create(AssemblyDomain domain, ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            DomainOperator instance = new DomainOperator();
            instance.Complier.EnumCRError = error;
            instance.Complier.EnumCRTarget = target;
            instance.Complier.Domain = domain;
            return instance;

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static DomainOperator Default()
        {

            return Create(DomainManagment.Default, ComplierResultTarget.Stream);

        }

        public static DomainOperator Default(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Default, target, error);

        }

        public static DomainOperator Default(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Default, target, error);

        }
        #endregion
        #region 随机域创建以及参数
        public static DomainOperator Random()
        {

            return Create(DomainManagment.Random, ComplierResultTarget.Stream);

        }


        public static DomainOperator Random(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Random, target, error);

        }


        public static DomainOperator Random(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Random, target, error);

        }
        #endregion



        public static DomainOperator operator &(string code, DomainOperator @operator)
        {

            @operator._operator.OopBody(code);
            return @operator;

        }
        public static DomainOperator operator &(DomainOperator @operator, string code)
        {

            @operator._operator.OopBody(code);
            return @operator;

        }
        



        public static DomainOperator operator |(DomainOperator @operator, NamespaceConverter @using)
        {

            @operator._operator.Using(@using);
            return @operator;

        }
        public static DomainOperator operator |(NamespaceConverter @using, DomainOperator @operator)
        {

            @operator._operator.Using(@using);
            return @operator;

        }


        public static DomainOperator operator +(DomainOperator @operator, string domainName)
        {

            @operator.Complier.Domain = DomainManagment.Create(domainName);
            return @operator;

        }
        public static DomainOperator operator +(string domainName, DomainOperator @operator)
        {

            @operator.Complier.Domain = DomainManagment.Create(domainName);
            return @operator;

        }
        public static DomainOperator operator +(DomainOperator @operator, AssemblyDomain domain)
        {

            @operator.Complier.Domain = domain;
            return @operator;

        }
        public static DomainOperator operator +(AssemblyDomain domain, DomainOperator @operator)
        {

            @operator.Complier.Domain = domain;
            return @operator;

        }




        public Type GetType(string typeName = default)
        {

            var text = _operator
                .GetUsingBuilder()
                .Append(_operator.OopContentScript)
                .ToString();
            if (typeName == default)
            {
                typeName = ScriptHelper.GetClassName(text);
                if (typeName == default)
                {
                    typeName = ScriptHelper.GetInterfaceName(text);
                    if (typeName == default)
                    {
                        typeName = ScriptHelper.GetStructName(text);
                        if (typeName == default)
                        {
                            typeName = ScriptHelper.GetEnumName(text);
                        }
                    }
                }
            }
            Complier.Add(text);
            return Complier.GetType(typeName);
        }

    }

}
