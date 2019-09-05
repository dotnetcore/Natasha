using Natasha.Builder;

namespace Natasha
{
    public class NInterface : OopBuilder<NInterface>
    {

        public NInterface()
        {

            Link = this;
            Public.ChangeToInterface();

        }

    }
}
