using System;

namespace Natasha
{
    public class OnceMethodDelegateTemplate<T>:OnceMethodContentTemplate<T>
    {
        public Type DelegateType { get { return DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType); } }
    }
}
