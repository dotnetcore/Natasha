using Microsoft.CodeAnalysis.Emit;
using Natasha.CSharp.Codecov.Utils;
using System.Collections.Immutable;

public static class AssenblyCSharpBuilder
{
    public static AssemblyCSharpBuilder WithCodecov(this AssemblyCSharpBuilder builder)
    {
        builder.ConfigEmitOptions(opt => opt.WithInstrumentationKinds(ImmutableArray.Create(InstrumentationKind.TestCoverage)));
        builder.Add(@"
namespace Microsoft.CodeAnalysis.Runtime
{
    public static class Instrumentation
    {
        private static Natasha.CSharp.Codecov.Utils.CodecovRecorder _recorder = default!;

        public static bool[] CreatePayload(System.Guid mvid, int methodToken, int fileIndex, ref bool[] payload, int payloadLength)
        { 
            if(_recorder == null)
            {
                _recorder = CodecovMonitor.GetRecorderFromType(typeof(Instrumentation));
            }
            return _recorder.CreatePayload(mvid, methodToken, fileIndex, ref payload, payloadLength);
        }

        public static bool[] CreatePayload(System.Guid mvid, int methodToken, int[] fileIndices, ref bool[] payload, int payloadLength)
        {
            if(_recorder == null)
            {
                _recorder = CodecovMonitor.GetRecorderFromType(typeof(Instrumentation));
            }
            return _recorder.CreatePayload(mvid, methodToken, fileIndices, ref payload, payloadLength);
        }

        public static void FlushPayload()
        {
            _recorder.FlushPayload();
        }
    }
}
");
        builder.CompileSucceedEvent += Builder_CompileSucceedEvent;
        return builder;
    }

    private static void Builder_CompileSucceedEvent(Microsoft.CodeAnalysis.CSharp.CSharpCompilation arg1, System.Reflection.Assembly arg2)
    {
        CodecovMonitor.AnalaysisAssemblyToCache(arg2);
    }
}

