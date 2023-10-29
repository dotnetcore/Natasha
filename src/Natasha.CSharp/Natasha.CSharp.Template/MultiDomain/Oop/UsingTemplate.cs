#if MULTI
namespace Natasha.CSharp.Template
{

    public partial class UsingTemplate<T> : FlagTemplate<T> where T : UsingTemplate<T>, new()
    {

       
        protected T LoadCurrentDomainUsing()
        {
            return Using(AssemblyBuilder.Domain.UsingRecorder._usings);
        }

    }

}
#endif
