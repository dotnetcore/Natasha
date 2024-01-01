using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

namespace Natasha.DynamicLoad.Base
{
    public interface INatashaDynamicLoadContextBase : IDisposable
    {
        string? Name { get; }

        object? GetCallerReference();

        void SetCallerReference(object instance);

        Assembly LoadPlugin(string path, Func<AssemblyName, bool>? excludeAssembliesFunc = null);

        void SetAssemblyLoadBehavior(AssemblyCompareInfomation loadBehavior);

        IEnumerable<Assembly> Assemblies { get; }

        Assembly LoadAssemblyFromFile(string path);

        Assembly LoadAssemblyFromStream(Stream dllStream,  Stream? pdbStream);
    }
}