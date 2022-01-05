using Microsoft.CodeAnalysis;
using Natasha.CSharp.Extension.Inner;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Natasha.CSharp.Component.Domain.Core
{
    //与元数据相关
    //数据值与程序集及内存相关
    internal class NatashaReferenceCache
    {
        /// <summary>
        /// 存放内存流编译过来的程序集与引用
        /// </summary>
        private readonly ConcurrentDictionary<AssemblyName, PortableExecutableReference> _referenceCache;
        private readonly ConcurrentDictionary<string, AssemblyName> _referenceNameCache;
        internal NatashaReferenceCache()
        {
            _referenceCache = new ConcurrentDictionary<AssemblyName, PortableExecutableReference>();
            _referenceNameCache = new ConcurrentDictionary<string, AssemblyName>();
        }
        internal int Count { get { return _referenceCache.Count; } }
        private void AddReference(AssemblyName assemblyName, PortableExecutableReference reference, LoadBehaviorEnum loadReferenceBehavior)
        {
            var name = assemblyName.GetUniqueName();
            if (_referenceNameCache.TryGetValue(name, out var oldAssemblyName))
            {
                if (assemblyName.CompareWithDefault(oldAssemblyName, loadReferenceBehavior) == LoadVersionResultEnum.UseCustomer)
                {
                    _referenceCache!.Remove(oldAssemblyName);
                    _referenceNameCache[name] = assemblyName;
                    _referenceCache[assemblyName] = reference;
                }
            }
            else
            {
                _referenceNameCache[name] = assemblyName;
                _referenceCache[assemblyName] = reference;
            }

        }
        internal void AddReference(AssemblyName assemblyName, Stream stream, LoadBehaviorEnum loadReferenceBehavior = LoadBehaviorEnum.None)
        {
            AddReference(assemblyName, MetadataReference.CreateFromStream(stream), loadReferenceBehavior);
        }
        internal void AddReference(AssemblyName assemblyName, string path, LoadBehaviorEnum loadReferenceBehavior = LoadBehaviorEnum.None)
        {
            AddReference(assemblyName, MetadataReference.CreateFromFile(path), loadReferenceBehavior);
        }
        internal void AddReference(Assembly assembly, LoadBehaviorEnum loadReferenceBehavior = LoadBehaviorEnum.None)
        {
            if (!assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location))
            {
                AddReference(assembly.GetName(), MetadataReference.CreateFromFile(assembly.Location), loadReferenceBehavior);
            }
        }
        internal void RemoveReference(AssemblyName assemblyName)
        {
            _referenceCache!.Remove(assemblyName);
            var name = assemblyName.Name;
            if (name != null)
            {
                _referenceNameCache!.Remove(name);
            }
        }
        internal void RemoveReference(string name)
        {
            if (_referenceNameCache.TryGetValue(name, out var assemblyName))
            {
                RemoveReference(assemblyName);
            }
        }
        internal void Clear()
        {
            _referenceCache.Clear();
            _referenceNameCache.Clear();
        }
        internal IEnumerable<PortableExecutableReference> GetReferences()
        {
            return _referenceCache.Values;
        }
        internal IEnumerable<PortableExecutableReference> CombineWithDefaultReferences(LoadBehaviorEnum loadBehavior = LoadBehaviorEnum.None, Func<AssemblyName, AssemblyName, LoadVersionResultEnum>? useAssemblyNameFunc = null)
        {
            var sets = new HashSet<PortableExecutableReference>(_referenceCache.Values);
            var excludeNods = new HashSet<PortableExecutableReference>();
            var defaultReferences = NatashaDomain.DefaultDomain._referenceCache._referenceCache;
            var defaultNameReferences = NatashaDomain.DefaultDomain._referenceCache._referenceNameCache; ;
            if (loadBehavior != LoadBehaviorEnum.None || useAssemblyNameFunc != null)
            {
                foreach (var item in _referenceNameCache)
                {
                    if (defaultNameReferences.TryGetValue(item.Key, out var defaultAssemblyName))
                    {
                        LoadVersionResultEnum funcResult;
                        if (useAssemblyNameFunc != null)
                        {
                            funcResult = useAssemblyNameFunc(defaultAssemblyName, item.Value);
                            if (funcResult == LoadVersionResultEnum.PassToNextHandler)
                            {
                                funcResult = item.Value.CompareWithDefault(defaultAssemblyName, loadBehavior);
                            }
                        }
                        else
                        {
                            funcResult = item.Value.CompareWithDefault(defaultAssemblyName, loadBehavior);
                        }

                        if (funcResult == LoadVersionResultEnum.UseDefault)
                        {
                            excludeNods.Add(_referenceCache[item.Value]);
                        }
                        else if (funcResult == LoadVersionResultEnum.UseCustomer)
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
    }
}
