using System;
using System.Reflection;

namespace Natasha.CSharp.Compiler.Component
{
    //unsafe delegate bool TryGetRawMetadataDelegate(Assembly assembly, out byte* blob, out int length);
    delegate void ApplyUpdateDelegate(Assembly assembly, ReadOnlySpan<byte> metadataDelta, ReadOnlySpan<byte> ilDelta, ReadOnlySpan<byte> pdbDelta);
    internal unsafe static class RuntimeInnerHelper
    {
        //internal readonly static TryGetRawMetadataDelegate TryGetRawMetadata;
        internal readonly static ApplyUpdateDelegate ApplyUpdate;
        static RuntimeInnerHelper()
        {
            AssemblyCSharpBuilder builder = new AssemblyCSharpBuilder();
            var assembly = builder
                .UseDefaultLoadContext()
                .UseSimpleMode()
                .WithDebugCompile(opt=>opt.ForAssembly())
                .ConfigLoadContext(context => context
                  .AddReferenceAndUsingCode(typeof(System.Reflection.Metadata.AssemblyReference))
                  .AddReferenceAndUsingCode(typeof(System.Runtime.CompilerServices.RuntimeHelpers))
                  .AddReferenceAndUsingCode(typeof(object))
                  .AddReferenceAndUsingCode(typeof(ReadOnlySpan<byte>))
                  .AddReferenceAndUsingCode(typeof(Assembly))
                )
                .Add(@"
public static class NatashaRuntimeInnerHelperDynamicBootstrap
{
    public static void ApplyUpdate(Assembly assembly, ReadOnlySpan<byte> metadataDelta, ReadOnlySpan<byte> ilDelta, ReadOnlySpan<byte> pdbDelta)
    {
        MetadataUpdater.ApplyUpdate(assembly, metadataDelta, ilDelta, pdbDelta);
    }
}")
                .GetAssembly();
        /*public unsafe static bool TryGetRawMetadata(Assembly assembly, out byte* blob, out int length)
        {
            #if NETCOREAPP3_0_OR_GREATER
                        return assembly.TryGetRawMetadata(out blob, out length);
            #else
                        blob = null;
                        length = 0;
                        return false;
            #endif
        }*/
            //TryGetRawMetadata = assembly.GetDelegateFromShortName<TryGetRawMetadataDelegate>("NatashaRuntimeInnerHelperDynamicBootstrap", "TryGetRawMetadata");
            ApplyUpdate = assembly.GetDelegateFromShortName<ApplyUpdateDelegate>("NatashaRuntimeInnerHelperDynamicBootstrap", "ApplyUpdate");


        }
    }
}
