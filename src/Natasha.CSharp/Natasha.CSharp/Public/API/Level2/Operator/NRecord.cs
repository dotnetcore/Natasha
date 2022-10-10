
using Natasha.CSharp;

public class NRecord : NHandler<NRecord>
{
    public NRecord()
    {

        Link = this;
        this.Record();

    }
}

