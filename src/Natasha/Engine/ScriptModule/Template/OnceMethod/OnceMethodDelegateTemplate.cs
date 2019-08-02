using System;

namespace Natasha.Template
{
    public class OnceMethodDelegateTemplate<T>:OnceMethodContentTemplate<T>
    {
        public Type DelegateType { get { return DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType); } }
    }
}
