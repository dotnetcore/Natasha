#if NETCOREAPP3_0_OR_GREATER
using Microsoft.CodeAnalysis;
using Natasha.CSharp.Component.Domain;
using Natasha.Domain.Extension;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("PluginFunctionUT, PublicKey=002400000480000094000000060200000024000052534131000400000100010069acb31dd0d9918441d6ed2b49cd67ae17d15fd6ded4ccd2f99b4a88df8cddacbf72d5897bb54f406b037688d99f482ff1c3088638b95364ef614f01c3f3f2a2a75889aa53286865463fb1803876056c8b98ec57f0b3cf2b1185de63d37041ba08f81ddba0dccf81efcdbdc912032e8d2b0efa21accc96206c386b574b9d9cb8")]
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

        /// <summary>
        /// 根据程序集名获取引用
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MetadataReference? GetSingleReference(AssemblyName name)
        {
            if (_referenceNameCache.TryGetValue(name.GetUniqueName(),out var realName))
            {
                return _referenceCache[realName];
            }
            return null;
        }

        public int Count { get { return _referenceCache.Count; } }

        public unsafe void AddReference(Assembly assembly, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
        {
            if (assembly.TryGetRawMetadata(out var blob, out var length))
            {
                var metadataReference = AssemblyMetadata.Create(ModuleMetadata.CreateFromMetadata((IntPtr)blob, length)).GetReference();
                AddReference(assembly.GetName(), metadataReference, loadReferenceBehavior);
            }
        }
        public void AddReference(AssemblyName assemblyName, MetadataReference reference, AssemblyCompareInfomation loadReferenceBehavior)
        {

            var name = assemblyName.GetUniqueName();
            if (loadReferenceBehavior != AssemblyCompareInfomation.None)
            {
                if (_referenceNameCache.TryGetValue(name, out var oldAssemblyName))
                {
                    if (assemblyName.CompareWithDefault(oldAssemblyName, loadReferenceBehavior) == AssemblyLoadVersionResult.UseCustomer)
                    {
                        _referenceCache!.Remove(oldAssemblyName);
                        _referenceNameCache!.Remove(name);
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
        public void AddReference(AssemblyName assemblyName, Stream stream, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
        {
            AddReference(assemblyName, MetadataReference.CreateFromStream(stream), loadReferenceBehavior);
        }
        public void AddReference(AssemblyName assemblyName, string path, AssemblyCompareInfomation loadReferenceBehavior = AssemblyCompareInfomation.None)
        {
            AddReference(assemblyName, CreateMetadataReference(path), loadReferenceBehavior);
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
        internal HashSet<MetadataReference> CombineWithDefaultReferences(NatashaReferenceCache defaultCache, AssemblyCompareInfomation loadBehavior = AssemblyCompareInfomation.None, Func<AssemblyName, AssemblyName, AssemblyLoadVersionResult>? useAssemblyNameFunc = null)
        {
            var sets = new HashSet<MetadataReference>(_referenceCache.Values);
            var excludeNods = new HashSet<MetadataReference>();
            var defaultReferences = defaultCache._referenceCache;
            var defaultNameReferences = defaultCache._referenceNameCache;
            if (loadBehavior != AssemblyCompareInfomation.None || useAssemblyNameFunc != null)
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
            //全部引用
            sets.UnionWith(defaultReferences.Values);
            //排除不符合的引用
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