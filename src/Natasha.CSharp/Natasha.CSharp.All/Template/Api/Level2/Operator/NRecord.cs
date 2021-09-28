using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp
{
    public class NRecord : NHandler<NRecord>
    {
        public NRecord()
        {

            Link = this;
            this.Record();

        }
    }
}
