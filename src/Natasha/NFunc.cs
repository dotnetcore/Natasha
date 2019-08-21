using Natasha.Operator;
using System;

namespace Natasha
{
    public static class NFunc<T>
    {

        public static Func<T> Delegate(string content, params Type[] usings)
        {
            return content==default? null : DelegateOperator<Func<T>>.Delegate(content, usings);
        }




        public static Func<T> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T>>.AsyncDelegate(content, usings);
        }




        public static Func<T> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T>>.UnsafeDelegate(content, usings);
        }




        public static Func<T> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T>>.UnsafeAsyncDelegate(content, usings);
        }

    }



    public static class NFunc<T1,T2>
    {

        public static Func<T1, T2> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2>>.Delegate(content, usings);
        }




        public static Func<T1, T2> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3>
    {

        public static Func<T1, T2, T3> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4>
    {

        public static Func<T1, T2, T3, T4> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5>
    {

        public static Func<T1, T2, T3, T4, T5> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5, T6>
    {

        public static Func<T1, T2, T3, T4, T5, T6> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5, T6, T7>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5, T6, T7, T8>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeAsyncDelegate(content, usings);
        }

    }





    public static class NFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>.UnsafeAsyncDelegate(content, usings);
        }

    }




    public static class NFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>.UnsafeAsyncDelegate(content, usings);
        }

    }





    public static class NFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Delegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>.Delegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> AsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>.AsyncDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> UnsafeDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>.UnsafeDelegate(content, usings);
        }




        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> UnsafeAsyncDelegate(string content, params Type[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>.UnsafeAsyncDelegate(content, usings);
        }

    }
}


