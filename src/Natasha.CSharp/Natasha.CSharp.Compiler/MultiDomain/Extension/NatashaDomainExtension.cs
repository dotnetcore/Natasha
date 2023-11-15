#if NETCOREAPP3_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static System.Runtime.Loader.AssemblyLoadContext;

public static class NatashaDomainExtension
{


    /// <summary>
    /// 创建一个以该字符串命名的域并锁定
    /// </summary>
    /// <param name="domain"></param>
    /// <returns></returns>
    public static ContextualReflectionScope NatashaDomainScope(this string domain)
    {
        return DomainManagement.Create(domain).EnterContextualReflection();
    }

}

#endif