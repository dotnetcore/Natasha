using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.Loader.AssemblyLoadContext;

public static class NatashaDomainExtension
{
    public static ContextualReflectionScope CreateScope(this NatashaDomain domain)
    {
        return domain.EnterContextualReflection();
    }

    public static ContextualReflectionScope NatashaDomainScope(this string domain)
    {
        return DomainManagement.Create(domain).EnterContextualReflection();
    }
}

