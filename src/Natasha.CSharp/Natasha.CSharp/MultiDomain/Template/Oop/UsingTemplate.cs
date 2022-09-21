using System;
#if MULTI
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Template
{

    public partial class UsingTemplate<T> : FlagTemplate<T> where T : UsingTemplate<T>, new()
    {

       
        protected T LoadCurrentDomainUsing()
        {
            return Using(AssemblyBuilder.Domain.UsingRecorder._usings);
        }

    }

}
#endif
