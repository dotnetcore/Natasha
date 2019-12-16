using Natasha.Operator;
using System;
using System.Collections.Concurrent;

namespace Natasha
{
    public static class DelegateExtension
    {

        public static Delegate Create(this Type instance, string content, params NamespaceConverter[] usings)
        {
            var method = instance.GetMethod("Invoke");
            return FakeMethodOperator
                .Create()
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie();
        }




        public static ConcurrentDictionary<Delegate, AssemblyDomain> _delegate_cache;
        static DelegateExtension()
        {
            _delegate_cache = new ConcurrentDictionary<Delegate, AssemblyDomain>();
        }




        public static bool DisposeDomain(this Delegate @delegate)
        {
            if (_delegate_cache.ContainsKey(@delegate))
            {
                while (!_delegate_cache.TryRemove(@delegate, out var domain))
                {
                    if (domain != default)
                    {
                        domain.Dispose();
                        return true;
                    }
                }
            }
            return false;
        }




        internal static void AddInCache(this Delegate @delegate, AssemblyDomain domain)
        {
            _delegate_cache[@delegate] = domain;
        }
    }
}
