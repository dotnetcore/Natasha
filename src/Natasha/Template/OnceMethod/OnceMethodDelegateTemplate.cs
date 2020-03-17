using Natasha.Builder;
using System;

namespace Natasha.Template
{
    public class OnceMethodDelegateTemplate<T> : OnceMethodContentTemplate<T> where T : OnceMethodDelegateTemplate<T>, new()
    {
        public Type DelegateType { get { return DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType); } }
    }
}
