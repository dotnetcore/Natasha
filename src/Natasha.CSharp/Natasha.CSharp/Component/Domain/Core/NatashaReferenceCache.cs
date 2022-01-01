using Microsoft.CodeAnalysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Natasha.CSharp.Component.Domain.Core
{
    public class NatashaReferenceCache
    {
        /// <summary>
        /// 存放内存流编译过来的程序集与引用
        /// </summary>
        private readonly ConcurrentDictionary<AssemblyName, PortableExecutableReference> _referenceCache;
        private readonly ConcurrentDictionary<string, AssemblyName> _referenceNameCache;
        public NatashaReferenceCache()
        {
            _referenceCache = new ConcurrentDictionary<AssemblyName, PortableExecutableReference>();
            _referenceNameCache = new ConcurrentDictionary<string, AssemblyName>();
        }
        public int Count { get { return _referenceCache.Count; } }
        public void AddReference(AssemblyName assemblyName,string path)
        {
            _referenceCache[assemblyName] = MetadataReference.CreateFromFile(path);
            if (assemblyName.Name!=null)
            {
                _referenceNameCache[assemblyName.Name] = assemblyName;
            }
        }
        public void AddReference(AssemblyName assemblyName, Stream stream)
        {
            _referenceCache[assemblyName] = MetadataReference.CreateFromStream(stream);
            if (assemblyName.Name != null)
            {
                _referenceNameCache[assemblyName.Name] = assemblyName;
            }
        }
        public void AddReference(Assembly assembly)
        {
            if (!assembly.IsDynamic && !string.IsNullOrEmpty(assembly.Location))
            {
                AddReference(assembly.GetName(), assembly.Location);
            }
        }
        public void RemoveReference(AssemblyName assemblyName)
        {
            _referenceCache!.Remove(assemblyName);
            var name = assemblyName.Name;
            if (name!=null)
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
        public IEnumerable<PortableExecutableReference> GetReferences()
        {
            return _referenceCache.Values;
        }
        public IEnumerable<PortableExecutableReference> CombineReferences(NatashaReferenceCache cache2, LoadBehaviorEnum loadBehavior = LoadBehaviorEnum.None)
        {
            var sets = new HashSet<PortableExecutableReference>(_referenceCache.Values);
            if (loadBehavior != LoadBehaviorEnum.None)
            {
                foreach (var item in _referenceNameCache)
                {

                    if (cache2._referenceNameCache.TryGetValue(item.Key, out var assemblyName))
                    {
                        switch (loadBehavior)
                        {
                            case LoadBehaviorEnum.UseHighVersion:
                                if (item.Value.Version <= assemblyName.Version)
                                {
                                    sets.Remove(_referenceCache[item.Value]);
                                }
                                break;
                            case LoadBehaviorEnum.UseLowVersion:
                                if (item.Value.Version >= assemblyName.Version)
                                {
                                    sets.Remove(_referenceCache[item.Value]);
                                }
                                break;
                            case LoadBehaviorEnum.UseBeforeIfExist:
                                sets.Remove(_referenceCache[item.Value]);
                                break;
                            default:
                                break;
                        }
                    }

                }
            }
            sets.UnionWith(cache2._referenceCache.Values);
            return sets;
        }
    }
}
