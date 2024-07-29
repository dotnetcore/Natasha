using Natasha.CSharp.Compiler.Component;
using System.Collections.Generic;
using System.Reflection;

public sealed partial class NatashaLoadContext
{

    public NatashaLoadContext AddUsings(HashSet<string> usings)
    {
        UsingRecorder.Using(usings);
        return this;
    }
    public NatashaLoadContext AddUsings(Assembly assembly)
    {
        var result = MetadataHelper.GetNamespaceFromMemroy(assembly, null);
        if (result != null)
        {
            AddUsings(result);
        }
        return this;
    }
    public NatashaLoadContext AddUsings<T>()
    {
        return AddUsings(typeof(T).Assembly);
    }
}