using Natasha.Builder;
using System;

namespace Natasha.Template
{
    public class MethodDelegateTemplate<T> : MethodBodyTemplate<T> where T : MethodDelegateTemplate<T>, new()
    {
        public Type DelegateType { get { return DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType); } }

    }

}
