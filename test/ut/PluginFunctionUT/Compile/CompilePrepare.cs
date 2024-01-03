using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NatashaFunctionUT.Compile
{
    public class CompilePrepare : DomainPrepare
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static (string name, string currentName, bool compileSucceed) CompileMetadataDiffCode(string code, AssemblyCompareInfomation referenceLoadBehavior)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Domain", "Reference", "1.0.0.0", _runtimeVersion, "MetadataDiff.dll");
            var name = Guid.NewGuid().ToString("N");
            var currentName = name;
            try
            {
                AssemblyCSharpBuilder builder = new();
                builder.UseNewDomain(name);
                currentName = builder.Domain.Name;
                builder.ConfigCompilerOption(opt => opt.WithLowerVersionsAssembly());
                var pAssembly = builder.Domain.LoadPlugin(path);
                var pType = pAssembly.GetTypes().Where(item => item.Name == "MetadataModel").First();
                var plugin = Activator.CreateInstance(pType);
                builder.LoadContext.AddReferenceAndUsingCode(pAssembly);
                builder.Add(code);
                try
                {
                    var assembly = builder
                        .WithCombineReferences(item => item.SetCompareInfomation(referenceLoadBehavior))
                        .ConfigDomain(item => item.WithDefaultVersionDependency())
                        .GetAssembly();
                    return (name!, currentName!, true);
                }
                catch
                {

                    return (name!, currentName!, false);
                }
            }
            finally
            {
                DomainManagement.Create(currentName!).Dispose();
            }

        }
    }
}
