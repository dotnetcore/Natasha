using Natasha.Template;

namespace Natasha.Builder
{
    public class CtorBuilder : DelegateTemplate<CtorBuilder>
    {

        public CtorBuilder()
        {

            Link = this;
            NoUseDefinedType();

        }

    }
}
