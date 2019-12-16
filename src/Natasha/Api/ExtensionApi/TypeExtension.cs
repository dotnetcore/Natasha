using Natasha;
using Natasha.Core;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace System
{
    public static class TypeExtension
    {

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
