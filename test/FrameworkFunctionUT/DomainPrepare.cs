using FrameworkFunctionUT.Special;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class DomainPrepare
{

    protected static readonly int DefaultAssembliesCount;
    protected static readonly int DefaultUsingCount;
    protected static string _runtimeVersion;
    static DomainPrepare()
    {
        NatashaInitializer.Preheating((item, name) => name!.Contains("IO"));
        NatashaManagement.AddGlobalReference(typeof(object));
        NatashaManagement.AddGlobalReference(typeof(Unsafe));
        NatashaManagement.AddGlobalReference(typeof(Task));
        NatashaManagement.AddGlobalReference(typeof(Attribute));
        NatashaManagement.AddGlobalReference(typeof(DefaultReadolyInterface));
        NatashaManagement.AddGlobalReference(typeof(Natasha.CSharp.FakeMethodOperator));
        NatashaManagement.AddGlobalReference(typeof(Cs0104Model1.Cs0104Model));
        NatashaManagement.AddGlobalReference(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Library", "netstandard.dll"));
        DefaultUsingCount = DefaultUsing.Count;
    }

}

