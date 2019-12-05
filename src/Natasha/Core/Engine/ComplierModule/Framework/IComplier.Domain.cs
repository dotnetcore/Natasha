using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Loader;

namespace Natasha.Complier
{
    public abstract partial class IComplier
    {

        private AssemblyDomain _domain;
        public readonly List<PortableExecutableReference> References;




        public AssemblyDomain Domain
        {

            get
            {

#if !NETSTANDARD2_0

                if (AssemblyLoadContext.CurrentContextualReflectionContext != default) 
                {

                    _domain = (AssemblyDomain)AssemblyLoadContext.CurrentContextualReflectionContext;
                
                }
#endif

               
                return _domain;

            }
            set
            {

                if (value==default)
                {
                    value = DomainManagment.Default;
                }
                _domain = value;

            }

        }

    }
}
