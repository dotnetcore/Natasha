using Natasha.Builder;

namespace Natasha
{
    public class NClass : OopBuilder<NClass>
    {

        public NClass()
        {

            Link = this;
            Public.ChangeToClass();

        }

    }
}
