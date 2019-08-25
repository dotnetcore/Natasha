using Natasha.Builder;

namespace Natasha
{
    public class NEnum : OopBuilder<NEnum>
    {

        public NEnum()
        {

            OopAccess(AccessTypes.Public);
            ChangeToEnum();
            Link = this;

        }

    }
}
