using System;
using System.Collections.Generic;
using System.Text;

namespace NatashaFunctionUT
{
    public class DomainPrepareBase
    {
        static DomainPrepareBase()
        {
            NatashaManagement.Preheating<NatashaDomainCreator>();
        }
    }
}
