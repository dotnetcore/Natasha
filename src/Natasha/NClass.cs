using Natasha.Builder;

namespace Natasha
{
    public class NClass : OopBuilder<NClass>
    {

        public NClass()
        {

            ChangeToClass();
            Link = this;

        }

    }
}
