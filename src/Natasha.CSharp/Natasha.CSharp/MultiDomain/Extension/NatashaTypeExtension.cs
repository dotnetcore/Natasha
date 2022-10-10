#if MULTI
using System;
using Natasha.CSharp.MultiDomain.Extension;

namespace Natasha.CSharp.MultiDomain.Extension
{
    public static class NatashaTypeExtension
    {


        public static NatashaReferenceDomain GetDomain(this Type type)
        {

            return type.Assembly.GetDomain();

        }



        public static void DisposeDomain(this Type type)
        {

            type.Assembly.DisposeDomain();

        }

    }
}
#endif