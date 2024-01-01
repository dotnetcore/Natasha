using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyModel;
using Natasha.CSharp.Compiler.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Xunit;

namespace NatashaFunctionUT.Reference
{
    [Trait("基础功能测试", "引用")]
    public class GlobalReferenceTest : ReferencePrepare
    {
        [Fact(DisplayName = "默认引用数量")]
        public void GlobalReference()
        { 
            IEnumerable<string> paths = DependencyContext
                .Default
                .CompileLibraries.SelectMany(cl => cl.ResolveReferencePaths());

            var count = NatashaLoadContext.DefaultContext.References.Count;
            Assert.True(NatashaLoadContext.DefaultContext.UsingRecorder.HasUsing("System.Threading"));
            Assert.False(NatashaLoadContext.DefaultContext.UsingRecorder.HasUsing("System.IO"));
        }
        [Fact(DisplayName = "[默认引用]排重测试")]
        public unsafe void DefaultDistinctReference()
        {

            NatashaMetadataCache referenceCache = new();
            var assembilies = AppDomain.CurrentDomain.GetAssemblies();// AssemblyLoadContext.Default.Assemblies;
            int count = 0;
            foreach (var assembly in assembilies)
            {
                if (assembly.TryGetRawMetadata(out var blob, out var length))
                {
                    var metadata = AssemblyMetadata.Create(ModuleMetadata.CreateFromMetadata((IntPtr)blob, length));
                    var metadataReference = metadata.GetReference();
                    referenceCache.AddReference(assembly.GetName(), metadataReference, AssemblyCompareInfomation.None);
                    count += 1;
                }
                //if (!item.IsDynamic && item.Location != string.Empty)
                //{
                    
                //    referenceCache.AddReference(item);
                    
                //}
                if (count == 10)
                {
                    break;
                }
            }
            //Assembly Path='C:\Program Files\dotnet\shared\Microsoft.NETCore.App\5.0.12\System.Private.CoreLib.dll'
            var references = referenceCache.CombineWithDefaultReferences(DefaultReferences, AssemblyCompareInfomation.UseDefault);
           

            if (DefaultReferences.Count != references.Count)
            {
                references.ExceptWith(DefaultReferences.GetReferences());
                //Assert.Contains("System.Private.CoreLib.dll", references.First().FilePath);
            }
            else
            {
                Assert.Equal(DefaultReferences.Count, references.Count);
            }
        }

        [Fact(DisplayName = "[合并版本引用]排重测试")]
        public void DistinctReferenceWithoutCompare()
        {
            var sets = GetPortableExecutableReferences(AssemblyCompareInfomation.None);
            //dapper + json(12.0) + DNDV1 + runtime
            Assert.Equal(4, sets.Count);

        }

        [Fact(DisplayName = "[高版本引用]排重测试")]
        public void DistinctReferenceWithHighVersion()
        {
            var sets = GetPortableExecutableReferences(AssemblyCompareInfomation.UseHighVersion);
#if NETCOREAPP3_1
            //dapper + json(12.0) + DNDV1
            Assert.Equal(3, sets.Count);
#else
            //dapper + json(12.0) + DNDV1
            Assert.Equal(3, sets.Count);
#endif
        }

        [Fact(DisplayName = "[低版本引用]排重测试")]
        public void DistinctReferenceWithLowVersion()
        {
            var sets = GetPortableExecutableReferences(AssemblyCompareInfomation.UseLowVersion);
#if NETCOREAPP3_1
            //dapper + DNDV1
            Assert.Equal(2, sets.Count);
#else
            //dapper + DNDV1 
            Assert.Equal(2, sets.Count);
#endif
        }

        [Fact(DisplayName = "[非同名引用]排重测试")]
        public void DistinctReferenceWithExistVersion()
        {
            var sets = GetPortableExecutableReferences(AssemblyCompareInfomation.UseDefault);
#if NETCOREAPP3_1
            //dapper + DNDV1
            Assert.Equal(2, sets.Count);
#else
            //dapper + DNDV1
            Assert.Equal(2, sets.Count);
#endif

        }
    } 
}
