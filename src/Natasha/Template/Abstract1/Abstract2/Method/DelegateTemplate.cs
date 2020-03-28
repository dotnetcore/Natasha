using Natasha.Builder;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Natasha.Template
{

    public class DelegateTemplate<T> : MethodBodyTemplate<T> where T : DelegateTemplate<T>, new()
    {

        private readonly List<Type> _parametersRecoder;
        public DelegateTemplate()
        {
            _parametersRecoder = new List<Type>();
            this.Return(typeof(void));
        }




        public T Return<S>()
        {
            return this.Return(typeof(S));
        }
        public T Return(MethodInfo info)
        {
            return this.Return(info.ReturnType);
        }




        public override T Param(Type type, string paramName, string keywords = default)
        {

            _parametersRecoder.Add(type);
            return base.Param(type, paramName, keywords);

        }




        public Type DelegateType { get { return DelegateBuilder.GetDelegate(_parametersRecoder.ToArray(), _type); } }

    }

}
