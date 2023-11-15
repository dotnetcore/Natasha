#if NETCOREAPP3_0_OR_GREATER
using System;


public static class NatashaDelegateExtension
{

    public static NatashaReferenceDomain GetDomain(this Delegate @delegate)
    {

        return @delegate.Method.Module.Assembly.GetDomain();

    }



    public static void DisposeDomain(this Delegate @delegate)
    {

        @delegate.Method.Module.Assembly.DisposeDomain();

    }
}

#endif