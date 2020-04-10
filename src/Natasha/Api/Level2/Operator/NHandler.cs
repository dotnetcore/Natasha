using Natasha.CSharp.Builder;
using System;

namespace Natasha.CSharp
{
    public class NHandler<T> : OopBuilder<T> where T : OopBuilder<T> , new()
    {

        public NDelegate DelegateHandler
        {
            get { return NDelegate.Use(AssemblyBuilder.Compiler.Domain).Namespace(NamespaceScript); }
        }


        public NClass ClassHandler
        {
            get { return NClass.Use(AssemblyBuilder.Compiler.Domain).Namespace(NamespaceScript); }
        }


        public NInterface InterfaceHandler
        {
            get { return NInterface.Use(AssemblyBuilder.Compiler.Domain).Namespace(NamespaceScript); }
        }


        public NEnum EnumHandler
        {
            get { return NEnum.Use(AssemblyBuilder.Compiler.Domain).Namespace(NamespaceScript); }
        }


        public Func<T> Creator()
        {
            return DelegateHandler.Func<T>($"return new {NameScript}();");
        }

    }

}
