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

            var handler = new PropertyBuilder();
            handler.NoUseAccess();
            action(handler);
            RecoderType(handler.UsingRecoder.Types);
            Body(handler.Script);
            return this;

        }



        public override NInterface Method(Action<MethodBuilder> action)
        {

            var handler = new MethodBuilder();
            handler.NoUseAccess().NoBody(";");
            action(handler);
            RecoderType(handler.UsingRecoder.Types);
            Body(handler.Script);
            return this;

        }

    }

}
