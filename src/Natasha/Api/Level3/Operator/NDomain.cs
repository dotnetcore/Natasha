using Natasha.Operator;
using System;

namespace Natasha
{
    public class NDomain
    {

        private AssemblyDomain _domain;
        private NDomain() { }

        public static NDomain Create(string domainName = default)
        {
            NDomain instance = new NDomain();
            if (domainName== default)
            {
                instance._domain = DomainManagment.Default;
            }
            else
            {
                instance._domain = DomainManagment.Create(domainName);
            }
           
            return instance;
        }

        public static NDomain Create(AssemblyDomain domain)
        {
            NDomain instance = new NDomain();
            instance._domain = domain;
            return instance;
        }

        public static NDomain Random()
        {
            NDomain instance = new NDomain();
            instance._domain = DomainManagment.Random();
            return instance;
        }




        public static void Delete(string name)
        {

            var domain = DomainManagment.Get(name);
            if (domain!=null)
            {
                domain.Dispose();
            }

        }




        public Func<T> Func<T>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T>>.Delegate(content, _domain, true, usings);
        }




        public Func<T> AsyncFunc<T>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T> UnsafeFunc<T>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T> UnsafeAsyncFunc<T>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2> Func<T1,T2>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2> AsyncFunc<T1,T2>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2> UnsafeFunc<T1,T2>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2> UnsafeAsyncFunc<T1,T2>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3> Func<T1,T2,T3>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3> AsyncFunc<T1,T2,T3>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3> UnsafeFunc<T1,T2,T3>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3> UnsafeAsyncFunc<T1,T2,T3>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Func<T1,T2,T3,T4> Func<T1,T2,T3,T4>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4> AsyncFunc<T1,T2,T3,T4>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4> UnsafeFunc<T1,T2,T3,T4>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4> UnsafeAsyncFunc<T1,T2,T3,T4>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Func<T1,T2,T3,T4,T5> Func<T1,T2,T3,T4,T5>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5> AsyncFunc<T1,T2,T3,T4,T5>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5> UnsafeFunc<T1,T2,T3,T4,T5>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5> UnsafeAsyncFunc<T1,T2,T3,T4,T5>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Func<T1,T2,T3,T4,T5,T6> Func<T1,T2,T3,T4,T5,T6>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6> AsyncFunc<T1,T2,T3,T4,T5,T6>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6> UnsafeFunc<T1,T2,T3,T4,T5,T6>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6> UnsafeAsyncFunc<T1,T2,T3,T4,T5,T6>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Func<T1,T2,T3,T4,T5,T6,T7> Func<T1,T2,T3,T4,T5,T6,T7>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7> AsyncFunc<T1,T2,T3,T4,T5,T6,T7>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7> UnsafeFunc<T1,T2,T3,T4,T5,T6,T7>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7> UnsafeAsyncFunc<T1,T2,T3,T4,T5,T6,T7>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Func<T1,T2,T3,T4,T5,T6,T7,T8> Func<T1,T2,T3,T4,T5,T6,T7,T8>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8> AsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8> UnsafeFunc<T1,T2,T3,T4,T5,T6,T7,T8>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8> UnsafeAsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9> Func<T1,T2,T3,T4,T5,T6,T7,T8,T9>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9> AsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9> UnsafeFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9> UnsafeAsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> AsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> UnsafeFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10> UnsafeAsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> AsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> UnsafeFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11> UnsafeAsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>>.Delegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> AsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> UnsafeFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12> UnsafeAsyncFunc<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }




        public Action Action(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action>.Delegate(content, _domain, true, usings);
        }




        public Action<T> Action<T>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T>>.Delegate(content, _domain, true, usings);
        }




        public Action<T> AsyncAction<T>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T> UnsafeAction<T>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T> UnsafeAsyncAction<T>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2> Action<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2> AsyncAction<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2> UnsafeAction<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2> UnsafeAsyncAction<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3> Action<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3> AsyncAction<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3> UnsafeAction<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3> UnsafeAsyncAction<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Action<T1, T2, T3, T4> Action<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4> AsyncAction<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4> UnsafeAction<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4> UnsafeAsyncAction<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Action<T1, T2, T3, T4, T5> Action<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5> AsyncAction<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5> UnsafeAction<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5> UnsafeAsyncAction<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Action<T1, T2, T3, T4, T5, T6> Action<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6> AsyncAction<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6> UnsafeAction<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Action<T1, T2, T3, T4, T5, T6, T7> Action<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7> AsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAction<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8> Action<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.Delegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.AsyncDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeDelegate(content, _domain, true, usings);
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeAsyncDelegate(content, _domain, true, usings);
        }
    }
}
