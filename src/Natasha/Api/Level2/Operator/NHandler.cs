using Natasha.Builder;
using System;

namespace Natasha
{
    public class NHandler<T> : OopBuilder<T> where T : OopBuilder<T> , new()
    {

        public NDelegate DelegateHandler
        {
            get { return NDelegate.Use(Compiler.Domain).Namespace(NamespaceScript); }
        }


        public NClass ClassHandler
        {
            get { return NClass.Use(Compiler.Domain).Namespace(NamespaceScript); }
        }


        public NInterface InterfaceHandler
        {
            get { return NInterface.Use(Compiler.Domain).Namespace(NamespaceScript); }
        }


        public NEnum EnumHandler
        {
            get { return NEnum.Use(Compiler.Domain).Namespace(NamespaceScript); }
        }


        public Func<T> Creator()
        {
            return DelegateHandler.Func<T>($"return new {NameScript}();");
        }

    }

}
