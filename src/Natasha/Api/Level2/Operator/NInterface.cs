using Natasha.CSharp.Builder;
using System;

namespace Natasha.CSharp
{
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
        public override PropertyBuilder GetPropertyBuilder()
        {

            return base.GetPropertyBuilder().NoUseAccess();

        }



        public override NInterface Method(Action<MethodBuilder> action)
        {

            return base.Method(item => { item.NoUseAccess().NoBody(";"); action(item); });

        }
        public override MethodBuilder GetMethodBuilder()
        {

            return base.GetMethodBuilder().NoUseAccess().NoBody(";");
        
        }

    }

}
