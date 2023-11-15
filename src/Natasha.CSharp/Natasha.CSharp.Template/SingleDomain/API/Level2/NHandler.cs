#if !NETCOREAPP3_0_OR_GREATER
using Natasha.CSharp.Builder;
using System;

namespace Natasha.CSharp
{
    public class NHandler<T> : OopBuilder<T> where T : OopBuilder<T> , new()
    {

        public NDelegate DelegateHandler
        {
            get { return NDelegate.DefaultDomain(); }
        }


        public NClass ClassHandler
        {
            get { return NClass.DefaultDomain().Using(NamespaceScript); }
        }


        public NInterface InterfaceHandler
        {
            get { return NInterface.DefaultDomain().Using(NamespaceScript); }
        }


        public NEnum EnumHandler
        {
            get { return NEnum.DefaultDomain().Using(NamespaceScript); }
        }


        public Func<T> Creator()
        {
            return DelegateHandler.Func<T>($"return new {NameScript}();");
        }

    }

}
#endif