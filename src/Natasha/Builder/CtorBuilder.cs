using Natasha.CSharp.Template;

namespace Natasha.CSharp.Builder
{
    public class CtorBuilder : DelegateTemplate<CtorBuilder>
    {

        public CtorBuilder()
        {

            Link = this;
            NoUseType();

        }

    }
}
