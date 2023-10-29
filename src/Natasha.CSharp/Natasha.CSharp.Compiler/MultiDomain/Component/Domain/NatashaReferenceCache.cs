#if MULTI
using Microsoft.CodeAnalysis;
using Natasha.CSharp.Component.Domain;
using Natasha.Domain.Extension;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace Natasha.CSharp.Component
{
    //与元数据相关
    //数据值与程序集及内存相关
    public sealed class NatashaReferenceCache
    {
        /// <summary>
        /// 存放内存流编译过来的程序集与引用
        /// </summary>
        private readonly ConcurrentDictionary<AssemblyName, MetadataReference> _referenceCache;
        private readonly ConcurrentDictionary<string, AssemblyName> _referenceNameCache;
        public NatashaReferenceCache()
        {
            _referenceCache = new();
            _referenceNameCache = new();
        }

        public int Count { get { return _referenceCache.Count; } }
        public void AddReference(AssemblyName assemblyName, MetadataReference reference, PluginLoadBehavior loadReferenceBehavior)
        {

            var name = assemblyName.GetUniqueName();
            if (loadReferenceBehavior != PluginLoadBehavior.None)
            {
                if (_referenceNameCache.TryGetValue(name, out var oldAssemblyName))
                {
                    if (assemblyName.CompareWithDefault(oldAssemblyName, loadReferenceBehavior) == AssemblyLoadVersionResult.UseCustomer)
                    {
                        _referenceCache!.Remove(oldAssemblyName);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            _referenceNameCache[name] = assemblyName;
            _referenceCache[assemblyName] = reference;

        }
        public void AddReference(AssemblyName assemblyName, Stream stream, PluginLoadBehavior loadReferenceBehavior = PluginLoadBehavior.None)
        {
            AddReference(assemblyName, MetadataReference.CreateFromStream(stream), loadReferenceBehavior);
        }
        public void AddReference(AssemblyName assemblyName, string path, PluginLoadBehavior loadReferenceBehavior = PluginLoadBehavior.None)
        {
            AddReference(assemblyName, CreateMetadataReference(path), loadReferenceBehavior);
        }

        public void AddReference(Assembly assembly, PluginLoadBehavior loadReferenceBehavior = PluginLoadBehavior.None)
        {
            if (!assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location))
            {
                AddReference(assembly.GetName(), CreateMetadataReference(assembly.Location), loadReferenceBehavior);
            }
        }
        public void RemoveReference(AssemblyName assemblyName)
        {
            _referenceCache!.Remove(assemblyName);
            var name = assemblyName.Name;
            if (name != null)
            {
                _referenceNameCache!.Remove(name);
            }
        }
        public void RemoveReference(string name)
        {
            if (_referenceNameCache.TryGetValue(name, out var assemblyName))
            {
                RemoveReference(assemblyName);
            }
        }
        public void Clear()
        {
            _referenceCache.Clear();
            _referenceNameCache.Clear();
        }
        public IEnumerable<MetadataReference> GetReferences()
        {
            return _referenceCache.Values;
        }
        internal HashSet<MetadataReference> CombineWithDefaultReferences(NatashaReferenceCache defaultCache, PluginLoadBehavior loadBehavior = PluginLoadBehavior.None, Func<AssemblyName, AssemblyName, AssemblyLoadVersionResult>? useAssemblyNameFunc = null)
        {
            var sets = new HashSet<MetadataReference>(_referenceCache.Values);
            var excludeNods = new HashSet<MetadataReference>();
            var defaultReferences = defaultCache._referenceCache;
            var defaultNameReferences = defaultCache._referenceNameCache; ;
            if (loadBehavior != PluginLoadBehavior.None || useAssemblyNameFunc != null)
            {
                foreach (var item in _referenceNameCache)
                {
                    if (defaultNameReferences.TryGetValue(item.Key, out var defaultAssemblyName))
                    {
                        AssemblyLoadVersionResult funcResult;
                        if (useAssemblyNameFunc != null)
                        {
                            funcResult = useAssemblyNameFunc(defaultAssemblyName, item.Value);
                            if (funcResult == AssemblyLoadVersionResult.PassToNextHandler)
                            {
                                funcResult = item.Value.CompareWithDefault(defaultAssemblyName, loadBehavior);
                            }
                        }
                        else
                        {
                            funcResult = item.Value.CompareWithDefault(defaultAssemblyName, loadBehavior);
                        }

                        if (funcResult == AssemblyLoadVersionResult.UseDefault)
                        {
                            excludeNods.Add(_referenceCache[item.Value]);
                        }
                        else if (funcResult == AssemblyLoadVersionResult.UseCustomer)
                        {
                            excludeNods.Add(defaultReferences[defaultAssemblyName]);
                        }

                    }
                }
            }
            sets.UnionWith(defaultReferences.Values);
            sets.ExceptWith(excludeNods);
            return sets;
        }

        private static MetadataReference CreateMetadataReference(string path)
        {
            using (var stream = File.OpenRead(path))
            {
                var moduleMetadata = ModuleMetadata.CreateFromStream(stream, PEStreamOptions.PrefetchMetadata);
                var assemblyMetadata = AssemblyMetadata.Create(moduleMetadata);
                return assemblyMetadata.GetReference(filePath: path);
            }
        }
    }
}
#endif