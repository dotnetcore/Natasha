using Natasha.CSharp.Codecov.Utils;
using System.Reflection;


public static class AssemblyExtension
{
    public static List<(string MethodName, bool[] Usage)>? GetCodecovCollection(this Assembly assembly)
    {
        var recorder = CodecovMonitor.GetRecorderFromAssmebly(assembly);
        return recorder.ToList();
    }

    public static void ResetCodecov(this Assembly assembly)
    {
        var recorder = CodecovMonitor.GetRecorderFromAssmebly(assembly);
        recorder.FlushPayload();
    }   
}

