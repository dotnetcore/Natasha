using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Metadata;

namespace Natasha.CSharp.Codecov.Utils
{
    public static class CodecovMonitor
    {
        private static readonly ConcurrentDictionary<string, CodecovRecorder> _codecovCache;

        static CodecovMonitor()
        {
            _codecovCache = new();
        }

        public static CodecovRecorder GetRecorderFromType(Type type)
        {
            return GetRecorderFromAssmebly(type.Assembly);
        }

        public static CodecovRecorder GetRecorderFromAssmebly(Assembly assembly)
        {
            if (_codecovCache.TryGetValue(assembly.FullName, out var result))
            {
                return result;
            }
            throw new Exception("请先使用 CodecovMonitor.AnalaysisAssemblyToCache() 分析您的动态程序集。");
        }

        public static unsafe void AnalaysisAssemblyToCache(Assembly assembly)
        {
            var compilerGeneratedType = assembly.GetType("<PrivateImplementationDetails>") ?? throw new EntryPointNotFoundException();
            var fieldInfo = compilerGeneratedType.GetField("MVID", BindingFlags.NonPublic | BindingFlags.Static) ?? throw new MissingMemberException(compilerGeneratedType.FullName, "MVID");
            var mvid = (Guid)fieldInfo.GetValue(null)!;

            if (assembly.TryGetRawMetadata(out var blob, out var length))
            {
                var metaReader = new MetadataReader(blob, length);
                var methodTotal = metaReader.MethodDefinitions.Count;
                var recorder = new CodecovRecorder(assembly.FullName, mvid.ToString(), methodTotal);
                _codecovCache[assembly.FullName] = recorder;
                int index = 0;
                foreach (var methodDefinition in metaReader.MethodDefinitions)
                {
                    var methodDefine = metaReader.GetMethodDefinition(methodDefinition);
                    var typeDefine = metaReader.GetTypeDefinition(methodDefine.GetDeclaringType());
                    var namespaceDefine = metaReader.GetNamespaceDefinition(typeDefine.NamespaceDefinition);
                    recorder.MethodNameCache[index] = string.Empty;
                    if (!namespaceDefine.Name.IsNil)
                    {
                        recorder.MethodNameCache[index] += $"{metaReader.GetString(namespaceDefine.Name)}.";
                    }
                    if (!typeDefine.Name.IsNil)
                    {
                        recorder.MethodNameCache[index] += $"{metaReader.GetString(typeDefine.Name)}.";
                    }
                    if (!methodDefine.Name.IsNil)
                    {
                        recorder.MethodNameCache[index] += $"{metaReader.GetString(methodDefine.Name)}";
                    }
                    index += 1;
                }
            }
        }
    }
}
