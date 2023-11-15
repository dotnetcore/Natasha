#if !NETCOREAPP3_0_OR_GREATER
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Natasha.CSharp.Component
{
    //与元数据相关
    //数据值与程序集及内存相关
    public static class NatashaReferenceCache
    {
        /// <summary>
        /// 存放内存流编译过来的程序集与引用
        /// </summary>
        private static readonly HashSet<MetadataReference> _referenceCache;
        static NatashaReferenceCache()
        {
            _referenceCache = new();
        }

        public static int Count { get { return _referenceCache.Count; } }

        public static void AddReference(string path)
        {
            DefaultUsing.AddUsing(Assembly.ReflectionOnlyLoadFrom(path));
            AddReference(MetadataReference.CreateFromFile(path));
        }
        public static void AddReference(Assembly assembly)
        {
            DefaultUsing.AddUsing(assembly);
            if (!string.IsNullOrEmpty(assembly.Location))
            {
                AddReference(MetadataReference.CreateFromFile(assembly.Location));
            }
        }
        public static void RemoveReference(string path)
        {
            RemoveReference(MetadataReference.CreateFromFile(path));
        } 
        public static void RemoveReference(Assembly assembly)
        {
            if (!string.IsNullOrEmpty(assembly.Location))
            {
                RemoveReference(MetadataReference.CreateFromFile(assembly.Location));
            }
        }

        private static void RemoveReference(MetadataReference reference)
        {
            lock (_referenceCache)
            {
                _referenceCache.Remove(reference);
            }
        }

        private static void AddReference(MetadataReference reference)
        {
            lock (_referenceCache)
            {
                _referenceCache.Add(reference);
            }
        }
        
        public static IEnumerable<MetadataReference> GetReferences()
        {
            return _referenceCache;
        }
        
    }
}
#endif