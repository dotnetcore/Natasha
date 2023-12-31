using Natasha.CSharp;
using Natasha.CSharp.Builder;
using Natasha.CSharp.Template;
using System;


public class NInterface : NHandler<NInterface>
{

    public NInterface()
    {

        Link = this;
        this.Interface();

    }



    public override NInterface Property(Action<PropertyBuilder> action)
    {

        return base.Property(item => { item.NoUseAccess(); action(item); });

    }


    public override NInterface Method(Action<MethodBuilder> action)
    {

        return base.Method(item => { item.NoUseAccess().NoBody(";"); action(item); });

    }

}
