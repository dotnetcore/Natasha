using Natasha.Operator;
using System;

namespace Natasha
{ 

    public class DomainOperator
    {

        private OopOperator _operator;
        private AssemblyDomain _domain;
        public DomainOperator()
        {
            _operator = new OopOperator();
        }


        public static DomainOperator Instance
        {
            get { return new DomainOperator(); }
        }


        public static DomainOperator Create(string domainName)
        {
            var result  = new DomainOperator();
            result._domain = DomainManagment.Create(domainName);
            return result;
        }
        public static DomainOperator Create(AssemblyDomain domain)
        {
            var result = new DomainOperator();
            result._domain = domain;
            return result;
        }



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

            @operator._domain = DomainManagment.Create(domainName);
            return @operator;

        }
        public static DomainOperator operator +(string domainName, DomainOperator @operator)
        {

            @operator._domain = DomainManagment.Create(domainName);
            return @operator;

        }
        public static DomainOperator operator +(DomainOperator @operator, AssemblyDomain domain)
        {

            @operator._domain = domain;
            return @operator;

        }
        public static DomainOperator operator +(AssemblyDomain domain, DomainOperator @operator)
        {

            @operator._domain = domain;
            return @operator;

        }




        public Type GetType(string typeName = default)
        {

            AssemblyComplier complier = new AssemblyComplier();
            complier.Domain = _domain;
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
            complier.Add(text);
            return complier.GetType(typeName);
        }

    }

}
