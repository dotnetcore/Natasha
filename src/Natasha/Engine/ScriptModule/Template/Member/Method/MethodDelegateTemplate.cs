using System;

namespace Natasha.Template
{
    public class MethodDelegateTemplate<T>:MethodBodyTemplate<T>
    {
        public Type DelegateType { get { return DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType); } }

    }

}
