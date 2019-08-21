using Natasha.Builder;

namespace Natasha
{
    public class NInterface : OopBuilder<NInterface>
    {

        public NInterface()
        {

            ChangeToInterface();
            Link = this;

        }

    }
}
