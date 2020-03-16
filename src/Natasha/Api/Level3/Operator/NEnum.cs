using Natasha.Builder;

namespace Natasha
{
    public class NEnum : NHandler<NEnum>
    {

        public NEnum()
        {

            Link = this;
            Public.ChangeToEnum();

        }

    }
}
