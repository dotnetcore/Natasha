using Microsoft.CodeAnalysis;
using Natasha.CSharp.Compiler.Component;
using System.Reflection;


public sealed partial class NatashaLoadContext
{
    public NatashaLoadContext AddReferences(AssemblyName name, MetadataReference metadataReference, AssemblyCompareInformation compareInfomation = AssemblyCompareInformation.None)
    {
        ReferenceRecorder.AddReference(name, metadataReference, compareInfomation);
        return this;
    }
    public NatashaLoadContext AddReferences(Assembly assembly, AssemblyCompareInformation compareInfomation = AssemblyCompareInformation.None)
    {
        var result = MetadataHelper.GetMetadataFromMemory(assembly, null);
        if (result.HasValue)
        {
            AddReferences(result.Value.asmName, result.Value.metadata, compareInfomation);
        }
        return this;
    }
    public NatashaLoadContext AddReferences<T>(AssemblyCompareInformation compareInfomation = AssemblyCompareInformation.None)
    {
        return AddReferences(typeof(T).Assembly);
    }
}