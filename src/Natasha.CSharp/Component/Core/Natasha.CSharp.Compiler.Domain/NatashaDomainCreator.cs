using Natasha.DynamicLoad.Base;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.Loader;

public class NatashaDomainCreator : INatashaDynamicLoadContextCreator
{
    public INatashaDynamicLoadContextBase CreateContext(string key)
    {
        return new NatashaCompilerDomain(key);
    }

    public INatashaDynamicLoadContextBase CreateDefaultContext()
    {
        return new NatashaCompilerDomain();
    }

    public IEnumerable<Assembly>? GetDependencyAssemblies(Assembly assembly)
    {
        var assemblyDomain = AssemblyLoadContext.GetLoadContext(assembly);
        if (assemblyDomain != null)
        {
            return assembly.GetReferencedAssemblies().Select(asmName => assemblyDomain.LoadFromAssemblyName(asmName));
        }
        return null;
    }

    public INatashaDynamicLoadContextBase? GetDomain(Assembly assembly)
    {
        var assemblyDomain = AssemblyLoadContext.GetLoadContext(assembly);
        if (assemblyDomain != null)
        {
            return assemblyDomain as NatashaCompilerDomain;
        }
        return null;
    }

    public unsafe bool TryGetRawMetadata(Assembly assembly, out byte* blob, out int length)
    {
        return assembly.TryGetRawMetadata(out blob, out length);
    }
}

