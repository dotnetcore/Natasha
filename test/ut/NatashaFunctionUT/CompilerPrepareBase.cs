using System;
using System.Linq;
using System.Runtime.Loader;

namespace NatashaFunctionUT
{
    public class CompilerPrepareBase
    {
        protected static readonly int DefaultAssembliesCount;
        protected static readonly int DefaultUsingCount;
        protected static string _runtimeVersion;
        static CompilerPrepareBase()
        {

            NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
#if NETCOREAPP3_1
            _runtimeVersion = "netcoreapp3.1";
#elif NET5_0
        _runtimeVersion = "net5.0";
#elif NET6_0
        _runtimeVersion = "net6.0";
#elif NET8_0
        _runtimeVersion = "net8.0";
#endif
            DefaultAssembliesCount = AssemblyLoadContext.Default.Assemblies.Count();
            NatashaInitializer.Preheating((item, name) => name!.Contains("IO"), true, false);
            DefaultUsingCount = NatashaLoadContext.DefaultContext.UsingRecorder.Count;
        }
    }
}
