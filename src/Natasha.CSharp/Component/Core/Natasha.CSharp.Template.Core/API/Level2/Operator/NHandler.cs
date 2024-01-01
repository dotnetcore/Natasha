using Natasha.CSharp.Builder;
using System;

namespace Natasha.CSharp
{
    public class NHandler<T> : OopBuilder<T> where T : OopBuilder<T> , new()
    {

        public NDelegate DelegateHandler
        {
            get { return NDelegate.UseDomain(AssemblyBuilder.LoadContext); }
        }


        public NClass ClassHandler
        {
            get { return NClass.UseDomain(AssemblyBuilder.LoadContext).Using(NamespaceScript); }
        }


        public NInterface InterfaceHandler
        {
            get { return NInterface.UseDomain(AssemblyBuilder.LoadContext).Using(NamespaceScript); }
        }


        public NEnum EnumHandler
        {
            get { return NEnum.UseDomain(AssemblyBuilder.LoadContext).Using(NamespaceScript); }
        }


        public Func<T> Creator()
        {
            return DelegateHandler.Func<T>($"return new {NameScript}();");
        }

    }

}