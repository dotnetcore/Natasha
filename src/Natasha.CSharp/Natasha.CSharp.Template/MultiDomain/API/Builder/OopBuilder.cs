#if NETCOREAPP3_0_OR_GREATER
using Natasha.CSharp.Template;

namespace Natasha.CSharp.Builder
{

    public partial class OopBuilder<T> : UsingTemplate<T> where T : OopBuilder<T>, new()
    {
        protected internal void BuildTree()
        {
            LoadCurrentDomainUsing();
            AssemblyBuilder.Add(this.GetScript());
        }
    }

}
#endif
