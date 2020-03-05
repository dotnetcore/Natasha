using Natasha.Reverser;
using System;
using System.Collections.Generic;
using System.Text;

namespace NatashaUT.Model
{
    public class MethodModel
    {
        public virtual void M() { }
    }

    public abstract class MethodModel2
    {
        public abstract void M();
    }

    public class MethodModel3:MethodModel2
    {
        public override void M()
        {
            throw new NotImplementedException();
        }
    }

    public class MethodModel4 : MethodModel3
    {
        public new void M()
        {
            
        }
    }

}
