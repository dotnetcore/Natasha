#if NETCOREAPP3_0_OR_GREATER
using System;
public static class NatashaTypeDomainExtension
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
#endif