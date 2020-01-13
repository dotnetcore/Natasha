using System;
using System.Reflection;

namespace Natasha.Core
{
    public delegate void DomainAssemblyEvent(string path, Assembly assembly);
    public delegate void DomainNativeAssemblyEvent(string path, IntPtr ptr);

}
