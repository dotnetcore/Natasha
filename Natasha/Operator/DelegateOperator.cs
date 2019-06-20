using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public static class DelegateOperator<T> where T : Delegate
    {
        public static T Create(string content, params string[] usings)
        {
            var method = typeof(T).GetMethod("Invoke");
            return FakeMethodOperator
                .New
                .UseMethod(method)
                .Using(usings)
                .StaticMethodContent(content)
                .Complie<T>();
        }
    }
}
