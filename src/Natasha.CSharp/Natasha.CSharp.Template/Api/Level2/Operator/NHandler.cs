using Natasha.CSharp.Builder;
using System;

namespace Natasha.CSharp
{
    public class NHandler<T> : OopBuilder<T> where T : OopBuilder<T> , new()
    {

        public NDelegate DelegateHandler
        {
            get { return NDelegate.UseDomain(AssemblyBuilder.Compiler.Domain).SetClass(item=>item.Using(NamespaceScript)); }
        }


        public NClass ClassHandler
        {
            get { return NClass.UseDomain(AssemblyBuilder.Compiler.Domain).Using(NamespaceScript); }
        }


        public NInterface InterfaceHandler
        {
            get { return NInterface.UseDomain(AssemblyBuilder.Compiler.Domain).Using(NamespaceScript); }
        }


        public NEnum EnumHandler
        {
            get { return NEnum.UseDomain(AssemblyBuilder.Compiler.Domain).Using(NamespaceScript); }
        }


        public Func<T> Creator()
        {
            return DelegateHandler.Func<T>($"return new {NameScript}();");
        }

    }

}
