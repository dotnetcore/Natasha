using System.Collections.Generic;
using System.Reflection;
namespace Natasha.DynamicLoad.Base
{
    public interface INatashaDynamicLoadContextCreator
    {
        INatashaDynamicLoadContextBase CreateContext(string key);

        INatashaDynamicLoadContextBase CreateDefaultContext();
        INatashaDynamicLoadContextBase? GetDomain(Assembly assembly);
        unsafe bool TryGetRawMetadata(Assembly assembly, out byte* blob, out int length);
        IEnumerable<Assembly>? GetDependencyAssemblies(Assembly assembly);
    }
}