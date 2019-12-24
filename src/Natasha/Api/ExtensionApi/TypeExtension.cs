using Natasha;
using Natasha.Core;

namespace System
{
    public static class TypeExtension
    {


        public static AssemblyDomain GetDomain(this Type type)
        {

            return DomainCache.GetDomain(type);

        }


        public static void RemoveReferences(this Type type)
        {

            DomainCache.RemoveReferences(type);

        }



        public static void DisposeDomain(this Type type)
        {

            DomainCache.DisposeDomain(type);

        }

    }
}
