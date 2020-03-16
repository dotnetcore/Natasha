using Natasha.Builder;

namespace Natasha
{
    public class NInterface : NHandler<NInterface>
    {

        public NInterface()
        {

            Link = this;
            Public.ChangeToInterface();

        }

    }
}
