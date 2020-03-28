using Natasha.Builder;
using System;

namespace Natasha
{
    public class NStruct : NHandler<NStruct>
    {

        public NStruct()
        {

            Link = this;
            this.Struct();

        }

    }

}
