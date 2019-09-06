using Natasha.Builder;

namespace Natasha
{
    public class NStruct : OopBuilder<NStruct>
    {

        public NStruct()
        {

            Link = this;
            Public.ChangeToStruct();

        }

    }
}
