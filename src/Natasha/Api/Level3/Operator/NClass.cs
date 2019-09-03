using Natasha.Builder;

namespace Natasha
{
    public class NClass : OopBuilder<NClass>
    {

        public NClass()
        {

            OopAccess(AccessTypes.Public);
            ChangeToClass();
            Link = this;

        }

    }
}
