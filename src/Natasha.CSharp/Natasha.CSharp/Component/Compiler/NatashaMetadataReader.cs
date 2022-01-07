using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Natasha.CSharp.Component.Compiler
{
    internal static class NatashaMetadataReader
    {

        internal static void ResolverMetadata(IEnumerable<string> paths)
        {
            var resolver = new PathAssemblyResolver(paths);
            using (var mlc = new MetadataLoadContext(resolver))
            {
                Parallel.ForEach(paths, (path) =>
                {

                    Assembly assembly = mlc.LoadFromAssemblyPath(path);
                    NatashaReferenceDomain.DefaultDomain.References.AddReference(assembly.GetName(),path);
                    DefaultUsing.AddUsingWithoutCheck(assembly);
                    NatashaDomain.AddAssemblyToDefaultCache(assembly);
               
                });
            }
        }

    }
}
