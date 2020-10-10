using Natasha.CSharp;
using Natasha.Framework;

namespace System
{
    public static class TypeExtension
    {


        public static DomainBase GetDomain(this Type type)
        {

            return type.Assembly.GetDomain();

        }


        public static void RemoveReferences(this Type type)
        {

            type.Assembly.RemoveReferences();

        }



        public static void DisposeDomain(this Type type)
        {

            type.Assembly.DisposeDomain();

        }

    }
}
