using Natasha.Builder;

namespace Natasha
{
    public class NEnum : OopBuilder<NEnum>
    {

        public NEnum()
        {

            Link = this;
            Public.ChangeToEnum();

        }

    }
}
