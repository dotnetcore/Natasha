using Natasha.CSharp.Builder;
using System;
using System.Collections.Generic;
using System.Reflection;
using Natasha.CSharp.Template.Reverser;

namespace Natasha.CSharp.Template
{

    public class DelegateTemplate<T> : MethodBodyTemplate<T> where T : DelegateTemplate<T>, new()
    {

        private readonly List<Type> _parametersRecoder;
        public DelegateTemplate()
        {
            _parametersRecoder = new List<Type>();
            Return(typeof(void));
        }


        public T ReadonlyReturn<S>()
        {
            return ReadonlyReturn(typeof(S));
        }


        public T ReadonlyReturn(Type type)
        {
            if (type != typeof(void))
            {
                Modifier(ModifierFlags.Readonly);
            }
            return Type(type);
        }


        public T ReadonlyReturn(MethodInfo info)
        {
            ModifierAppend(DeclarationReverser.GetReturnPrefix(info));
            return ReadonlyReturn(info.ReturnType);
        }


        public T Return<S>()
        {
            return Return(typeof(S));
        }


        public T Return(Type type)
        {
            return Type(type);
        }


        public T Return(MethodInfo info)
        {
            ModifierAppend(DeclarationReverser.GetReturnPrefix(info));
            return Return(info.ReturnType);
        }


        public T Return(string returnTypeString)
        {
            return Type(returnTypeString);
        }


        public override T Param(Type type, string paramName, string keywords = "")
        {

            _parametersRecoder.Add(type);
            return base.Param(type, paramName, keywords);

        }


        public Type DelegateType { get { return DelegateBuilder.GetDelegate(_parametersRecoder.ToArray(), _type); } }

    }

}
