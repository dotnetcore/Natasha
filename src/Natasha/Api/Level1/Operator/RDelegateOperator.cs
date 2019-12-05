using System;

namespace Natasha.Operator
{

    public static class RDelegateOperator<T> where T : Delegate
    {

        public static T Delegate(string content, params NamespaceConverter[] usings)
        {

            var domain = DomainManagment.Create("N" + Guid.NewGuid().ToString("N"));
            return DelegateOperator<T>.Delegate(content, domain, true, usings);

        }




        public static T AsyncDelegate(string content, params NamespaceConverter[] usings)
        {

            var domain = DomainManagment.Create("N" + Guid.NewGuid().ToString("N"));
            return DelegateOperator<T>.AsyncDelegate(content, domain, true, usings);

        }




        public static T UnsafeDelegate(string content, params NamespaceConverter[] usings)
        {

            var domain = DomainManagment.Create("N" + Guid.NewGuid().ToString("N"));
            return DelegateOperator<T>.UnsafeDelegate(content, domain, true, usings);


        }




        public static T UnsafeAsyncDelegate(string content, params NamespaceConverter[] usings)
        {

            var domain = DomainManagment.Create("N" + Guid.NewGuid().ToString("N"));
            return DelegateOperator<T>.UnsafeAsyncDelegate(content, domain, true, usings);

        }

    }

}
