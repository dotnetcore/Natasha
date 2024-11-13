using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Natasha.CSharp.Compiler.Component
{
    public static class MetadataHelper
    {

        #region metadata & namespace in memroy
        public static unsafe (AssemblyName asmName, MetadataReference metadata, HashSet<string> namespaces)? GetMetadataAndNamespaceFromMemory(Assembly assembly, Func<AssemblyName?, string?, bool>? filter = null)
        {
#if DEBUG
            Debug.WriteLine($"~~提取元数据:{assembly.FullName}");
#endif
            var asmName = assembly.GetName();
            if (NatashaLoadContext.Creator.TryGetRawMetadata(assembly, out var blob, out var length))
            {
                if (filter == null || !filter(asmName, asmName.Name))
                {
                    return (
                        asmName, 
                        AssemblyMetadata.Create(ModuleMetadata.CreateFromMetadata((IntPtr)blob, length)).GetReference(),
                        GetNamespaceFromMemroy(assembly, filter)
                        );
                }
            }
            return null;
        }
        public static unsafe (AssemblyName asmName, MetadataReference metadata)? GetMetadataFromMemory(Assembly assembly, Func<AssemblyName?, string?, bool>? filter = null)
        {
#if DEBUG
            Debug.WriteLine($"~~提取元数据:{assembly.FullName}");
#endif
            var asmName = assembly.GetName();
            if (NatashaLoadContext.Creator.TryGetRawMetadata(assembly, out var blob, out var length))
            {
                if (filter == null || !filter(asmName, asmName.Name))
                {
                    return (asmName, AssemblyMetadata.Create(ModuleMetadata.CreateFromMetadata((IntPtr)blob, length)).GetReference());
                }
            }
            return null;
        }

        public static HashSet<string> GetNamespaceFromMemroyWithoutInternal(Assembly assembly, Func<AssemblyName?, string?, bool>? filter = null)
        {
            HashSet<string> tempSets = [];
            try
            {
                var types = assembly.ExportedTypes;
                if (types.Count() > 16)
                {
                    var result = Parallel.ForEach(types, type =>
                    {

                        if ((type.IsNested && !type.IsNestedPublic) || !type.IsPublic)
                        {
                            return;
                        }

                        var name = type.Namespace;

                        lock (tempSets)
                        {
                            if (tempSets.Contains(name))
                            {
                                return;
                            }
                        }

                        if (!string.IsNullOrEmpty(name)
                        && !name.StartsWith("Internal")
                        && name.IndexOf('<') == -1)
                        {

                            if (filter == null || !filter(null, name))
                            {
                                lock (tempSets)
                                {
                                    tempSets.Add(name);
                                }
                            }
#if DEBUG
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("[排除程序集]:" + name);
                            }
#endif
                        }
                    });
                    while (!result.IsCompleted)
                    {
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    foreach (var type in types)
                    {

                        if ((type.IsNested && !type.IsNestedPublic) || !type.IsPublic)
                        {
                            continue;
                        }

                        var name = type.Namespace;
                        if (!string.IsNullOrEmpty(name)
                            && !name.StartsWith("Internal")
                            && !tempSets.Contains(name)
                            && name.IndexOf('<') == -1)
                        {

                            if (filter == null || !filter(null, name))
                            {
                                tempSets.Add(name);
                            }
#if DEBUG
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("[排除程序集]:" + name);
                            }
#endif
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(assembly.FullName + ex.Message);
#endif
            }
            return tempSets;
        }
        public static HashSet<string> GetNamespaceFromMemroy(Assembly assembly, Func<AssemblyName?, string?, bool>? filter = null)
        {
            if (assembly.FullName.StartsWith("System.Private.CoreLib"))
            {
                return GetNamespaceFromMemroyWithoutInternal(assembly, filter);
            }
            HashSet<string> tempSets = [];
            try
            {
                var types = assembly.ExportedTypes;
                if (types.Count() > 16)
                {
                    var result = Parallel.ForEach(types, type =>
                    {

                        if ((type.IsNested && !type.IsNestedPublic) || !type.IsPublic)
                        {
                            return;
                        }

                        var name = type.Namespace;

                        lock (tempSets)
                        {
                            if (tempSets.Contains(name))
                            {
                                return;
                            }
                        }

                        if (!string.IsNullOrEmpty(name)
                        && name.IndexOf('<') == -1)
                        {

                            if (filter == null || !filter(null, name))
                            {
                                lock (tempSets)
                                {
                                    tempSets.Add(name);
                                }
                            }
#if DEBUG
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("[排除程序集]:" + name);
                            }
#endif
                        }
                    });
                    while (!result.IsCompleted)
                    {
                        Thread.Sleep(50);
                    }
                }
                else
                {
                    foreach (var type in types)
                    {

                        if ((type.IsNested && !type.IsNestedPublic) || !type.IsPublic)
                        {
                            continue;
                        }

                        var name = type.Namespace;
                        if (!string.IsNullOrEmpty(name)
                            && !tempSets.Contains(name)
                            && name.IndexOf('<') == -1)
                        {

                            if (filter == null || !filter(null, name))
                            {
                                tempSets.Add(name);
                            }
#if DEBUG
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("[排除程序集]:" + name);
                            }
#endif
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(assembly.FullName + ex.Message);
#endif
            }
            return tempSets;
        }
#endregion

        #region metadata & namespace in file
        internal static (AssemblyName asmName, MetadataReference metadata, HashSet<string> namespaces)? GetMetadataAndNamespaceFromFile(string path, Func<AssemblyName?, string?, bool>? filter = null)
        {
            try
            {
                using var peStream = File.OpenRead(path);
                PEReader peReader = new(peStream);
                var metadataReader = peReader.GetMetadataReader();
                if (peReader.HasMetadata && metadataReader.IsAssembly)
                {
                    var asmName = AssemblyName.GetAssemblyName(path);
                    if (filter == null || !filter(asmName, asmName.Name))
                    {
                        peStream.Seek(0, SeekOrigin.Begin);
                        var tempUsingCache = GetNamespacesFromMetadata(metadataReader, filter);
                        return (asmName, CreateMetadataReference(peStream), tempUsingCache);
                    }
                }

            }
            catch
            {
            }
            return null;
        }
        internal static (AssemblyName asmName, MetadataReference metadata)? GetMetadataFromFile(string path, Func<AssemblyName?, string?, bool>? filter = null)
        {
            try
            {
                using var peStream = File.OpenRead(path);
                PEReader peReader = new(peStream);
                var metadataReader = peReader.GetMetadataReader();
                if (peReader.HasMetadata && metadataReader.IsAssembly)
                {
                    var asmName = AssemblyName.GetAssemblyName(path);
                    if (filter == null || !filter(asmName, asmName.Name))
                    {
                        peStream.Seek(0, SeekOrigin.Begin);
                        return (asmName, CreateMetadataReference(peStream));
                    }
                }

            }
            catch
            {
            }
            return null;
        }
        internal static HashSet<string>? GetNamespaceFromFile(string path, Func<AssemblyName?, string?, bool>? filter = null)
        {
            try
            {
                using var peStream = File.OpenRead(path);
                PEReader peReader = new(peStream);
                var metadataReader = peReader.GetMetadataReader();
                if (peReader.HasMetadata && metadataReader.IsAssembly)
                {
                    var asmName = AssemblyName.GetAssemblyName(path);
                    if (filter == null || !filter(asmName, asmName.Name))
                    {
                        return GetNamespacesFromMetadata(metadataReader, filter);
                    }
                }

            }
            catch
            {
            }
            return null;
        }
        internal static HashSet<string> GetNamespacesFromMetadata(MetadataReader metadataReader, Func<AssemblyName?, string?, bool>? filter = null)
        {
            HashSet<string> tempUsingCache = [];
            foreach (var typeDefinitionHandle in metadataReader.TypeDefinitions)
            {
                var typeDefinition = metadataReader.GetTypeDefinition(typeDefinitionHandle);
                if (!typeDefinition.Namespace.IsNil)
                {
                    var name = metadataReader.GetString(typeDefinition.Namespace);
                    if (!string.IsNullOrEmpty(name) && name.IndexOf('<') == -1)
                    {
                        if (filter == null || !filter(null, name))
                        {
                            tempUsingCache.Add(name);
                        }
                    }
                }
            }
            return tempUsingCache;
        }
        #endregion


        internal static MetadataReference CreateMetadataReference(string path)
        {
            using var stream = File.OpenRead(path);
            var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
            var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
            return assemblyMetadata.GetReference(filePath: path);
        }
        internal static MetadataReference CreateMetadataReference(Stream peStream)
        {
            var moduleMetadata = ModuleMetadata.CreateFromStream(peStream, PEStreamOptions.PrefetchMetadata);
            var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
            return assemblyMetadata.GetReference();
        }
    }
}