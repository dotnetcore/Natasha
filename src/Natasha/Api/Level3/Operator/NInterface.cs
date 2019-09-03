using Natasha.Builder;

namespace Natasha
{
    public class NInterface : OopBuilder<NInterface>
    {

        public NInterface()
        {

            OopAccess(AccessTypes.Public);
            ChangeToInterface();
            Link = this;

        }

    }
}
