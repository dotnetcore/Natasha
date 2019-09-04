using Microsoft.CodeAnalysis;
using System.Collections.Generic;

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


                if( _domain != DomainManagment.Default)
                {

                    References.AddRange(_domain.ReferencesCache);

                }
#else
                if (_domain != DomainManagment.Default)
                {

                    References.AddRange(_domain.ReferencesCache);

                }
#endif
                return _domain;

            }
            set
            {

                _domain = value;

            }

        }

    }
}
