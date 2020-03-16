using Natasha.Builder;

namespace Natasha
{
    public class NHandler<T> : OopBuilder<T> where T : OopBuilder<T> , new()
    {

        public NHandler() { }
        public NDomain DelegateHandler
        {
            get { return NDomain.Create(Complier.Domain).Using(NamespaceScript); }
        }


        public NClass ClassHandler
        {
            get { return NClass.Create(Complier.Domain).Using(NamespaceScript); }
        }


        public NInterface InterfaceHandler
        {
            get { return NInterface.Create(Complier.Domain).Using(NamespaceScript); }
        }


        public NEnum EnumHandler
        {
            get { return NEnum.Create(Complier.Domain).Using(NamespaceScript); }
        }

    }
}
