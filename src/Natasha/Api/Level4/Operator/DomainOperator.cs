using Natasha.Core;
using Natasha.Operator;
using System;

namespace Natasha
{ 

    public class DomainOperator
    {

        private OopOperator _operator;
        private AssemblyDomain _domain;
        private bool _complieInFile;
        public DomainOperator()
        {
            _operator = new OopOperator();
        }


        public static DomainOperator Instance
        {
            get { return new DomainOperator(); }
        }

        public static DomainOperator Default
        {

            get { return Create(); }

        }

        public static DomainOperator Create(string domainName = default, bool complieInFile = false)
        {

            DomainOperator instance = new DomainOperator
            {
                _complieInFile = complieInFile
            };


            if (domainName == default)
            {
                instance._domain = DomainManagment.Default;
            }
            else
            {
                instance._domain = DomainManagment.Create(domainName);
            }

            return instance;

        }

        public static DomainOperator Create(AssemblyDomain domain, bool complieInFile = false)
        {

            DomainOperator instance = new DomainOperator
            {
                _complieInFile = complieInFile,
                _domain = domain
            };

            return instance;
        }




        public static DomainOperator Random(bool complieInFile = false)
        {

            DomainOperator instance = new DomainOperator
            {
                _complieInFile = complieInFile,
                _domain = DomainManagment.Random
            };
            return instance;

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
            complier.ComplieInFile = _complieInFile;
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
