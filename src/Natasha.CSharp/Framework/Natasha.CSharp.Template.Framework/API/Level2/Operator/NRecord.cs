
using Natasha.CSharp;
using Natasha.CSharp.Template;

public class NRecord : NHandler<NRecord>
{
    public NRecord()
    {

        Link = this;
        this.Record();

    }
}

