using System;

namespace Natasha.Operator
{
    public static class NDelegateOperator<T> where T : Delegate
    {

        public static T Delegate(string content, params NamespaceConverter[] usings)
        {

            return DelegateOperator<T>.Delegate(content, default, false, usings);

        }




        public static T AsyncDelegate(string content, params NamespaceConverter[] usings)
        {

            return DelegateOperator<T>.AsyncDelegate(content, default, false, usings);

        }




        public static T UnsafeDelegate(string content, params NamespaceConverter[] usings)
        {

            return DelegateOperator<T>.UnsafeDelegate(content, default, false, usings);

        }




        public static T UnsafeAsyncDelegate(string content, params NamespaceConverter[] usings)
        {

            return DelegateOperator<T>.UnsafeAsyncDelegate(content, default, false, usings);

        }

    }

}
