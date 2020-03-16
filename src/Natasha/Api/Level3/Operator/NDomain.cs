using Natasha.Core;
using Natasha.Operator;
using System;

namespace Natasha
{
    public class NDomain : NUsing
    {

        private AssemblyDomain _domain;
        private ComplierResultTarget _target;
        private ComplierResultError _error;

        private NDomain() { }




        #region 指定字符串域创建以及参数
        public static NDomain Create(string domainName, ComplierResultTarget target = ComplierResultTarget.Stream, ComplierResultError error = ComplierResultError.None)
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

        public static NDomain Create(string domainName, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(domainName, target, error);

        }
        #endregion
        #region 指定域创建以及参数
        public static NDomain Create(AssemblyDomain domain, ComplierResultTarget target = ComplierResultTarget.Stream, ComplierResultError error = ComplierResultError.None)
        {

            NDomain instance = new NDomain
            {
                _target = target,
                _error = error,
                _domain = domain
            };
            return instance;

        }




        public static NDomain Create(AssemblyDomain domain, ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(domain, target, error);

        }
        #endregion
        #region  Default 默认域创建以及参数
        public static NDomain Default()
        {

            return Create(DomainManagment.Default, ComplierResultTarget.Stream);

        }

        public static NDomain Default(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Default, target, error);

        }

        public static NDomain Default(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Default, target, error);

        }
        #endregion
        #region 随机域创建以及参数
        public static NDomain Random()
        {

            return Create(DomainManagment.Random, ComplierResultTarget.Stream);

        }


        public static NDomain Random(ComplierResultError error, ComplierResultTarget target = ComplierResultTarget.Stream)
        {

            return Create(DomainManagment.Random, target, error);

        }


        public static NDomain Random(ComplierResultTarget target, ComplierResultError error = ComplierResultError.None)
        {

            return Create(DomainManagment.Random, target, error);

        }
        #endregion







        public Type GetType(string code, string typeName = default)
        {

            OopOperator oopOperator = OopOperator.Create(_domain, _target, _error);
            string result = oopOperator
                .GetUsingBuilder()
                .Append(code).ToString();

            oopOperator.Complier.SyntaxInfos.Add(result, oopOperator.Usings);

            var text = result;
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

            return oopOperator.Complier.GetType(typeName);
        }




        public T Delegate<T>(string content, params NamespaceConverter[] usings) where T : Delegate
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<T>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public T AsyncDelegate<T>(string content, params NamespaceConverter[] usings) where T : Delegate
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<T>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public T UnsafeDelegate<T>(string content, params NamespaceConverter[] usings) where T : Delegate
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<T>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public T UnsafeAsyncDelegate<T>(string content, params NamespaceConverter[] usings) where T : Delegate
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<T>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T> Func<T>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T> AsyncFunc<T>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T> UnsafeFunc<T>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T> UnsafeAsyncFunc<T>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2> Func<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2> AsyncFunc<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2> UnsafeFunc<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2> UnsafeAsyncFunc<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3> Func<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3> AsyncFunc<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3> UnsafeFunc<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3> UnsafeAsyncFunc<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Func<T1, T2, T3, T4> Func<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4> AsyncFunc<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4> UnsafeFunc<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4> UnsafeAsyncFunc<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Func<T1, T2, T3, T4, T5> Func<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5> AsyncFunc<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5> UnsafeFunc<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5> UnsafeAsyncFunc<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6> Func<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6> AsyncFunc<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6> UnsafeFunc<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7> Func<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7> AsyncFunc<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8> Func<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action Action(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action AsyncAction(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action UnsafeAction(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action UnsafeAsyncAction(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T> Action<T>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T> AsyncAction<T>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T> UnsafeAction<T>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T> UnsafeAsyncAction<T>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2> Action<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2> AsyncAction<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2> UnsafeAction<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2> UnsafeAsyncAction<T1, T2>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3> Action<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3> AsyncAction<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3> UnsafeAction<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3> UnsafeAsyncAction<T1, T2, T3>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action<T1, T2, T3, T4> Action<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4> AsyncAction<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4> UnsafeAction<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4> UnsafeAsyncAction<T1, T2, T3, T4>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action<T1, T2, T3, T4, T5> Action<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5> AsyncAction<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5> UnsafeAction<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5> UnsafeAsyncAction<T1, T2, T3, T4, T5>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6> Action<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6> AsyncAction<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6> UnsafeAction<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7> Action<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7> AsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAction<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8> Action<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }



        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.Delegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> AsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.AsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }




        public Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> UnsafeAsyncAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string content, params NamespaceConverter[] usings)
        {
            if (usings != default) { _using_cache.AddRange(usings); }
            return content == default ? null : DelegateOperator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>.UnsafeAsyncDelegate(content, _domain, _target, _error, _using_cache.ToArray());
        }
    }
}
