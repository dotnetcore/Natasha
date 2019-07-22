using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha
{
    public class MethodDelegateTemplate<T>:MethodBodyTemplate<T>
    {
        public Type DelegateType { get { return DelegateBuilder.GetDelegate(ParametersTypes.ToArray(), ReturnType); } }
    }
}
