using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace System.Runtime.Loader
{
#if !NETCOREAPP3_0_OR_GREATER
    public class AssemblyDependencyResolver
    {
        private readonly ConcurrentQueue<string> _queue;
        private readonly string _directory;
        public AssemblyDependencyResolver(string componentAssemblyPath)
        {
            if (File.Exists(componentAssemblyPath))
            {
                _directory = Path.GetDirectoryName(componentAssemblyPath);
                var hashSet = new HashSet<string>(Directory.GetFiles(_directory, "*.dll", SearchOption.AllDirectories));
                hashSet.Remove(componentAssemblyPath);
                _queue = new ConcurrentQueue<string>(hashSet);
            }
        }


        public IEnumerable<string> GetFilePath()
        {
            return _queue;
        }


        public string ResolveAssemblyToPath(AssemblyName assemblyName)
        {
            if (_queue != null)
            {
                if (_queue.TryDequeue(out var path))
                {
                    return path;
                }
            }
            return null;
        }


        public string ResolveUnmanagedDllToPath(string unmanagedDllName)
        {
            if (_queue.TryDequeue(out var path))
            {
                return path;
            }
            return null;
        }
    }

#endif

}
