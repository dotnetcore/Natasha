using Natasha.Builder;

namespace Natasha
{
    public class NStruct : OopBuilder<NStruct>
    {

        public NStruct()
        {

            OopAccess(AccessTypes.Public);
            ChangeToStruct();
            Link = this;

        }

    }
}
