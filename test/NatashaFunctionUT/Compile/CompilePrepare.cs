using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NatashaFunctionUT.Compile
{
    public class CompilePrepare : DomainPrepare
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static (string name,string currentName,bool compileSucceed) CompileMetadataDiffCode(string code, LoadBehaviorEnum referenceLoadBehavior)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!,"Domain", "Reference", "1.0.0.0", _runtimeVersion, "MetadataDiff.dll");
            var name = Guid.NewGuid().ToString("N");
            var currentName = name;
            try
            {
                using (DomainManagement.Create(name).CreateScope())
                {

                    AssemblyCSharpBuilder builder = new();
                    currentName = builder.Domain.Name;

                    var pAssembly = builder.Domain.LoadPlugin(path);
                    var pType = pAssembly.GetTypes().Where(item => item.Name == "MetadataModel").First();
                    var plugin = Activator.CreateInstance(pType);


                    builder.Add(code);
                    try
                    {
                        var assembly = builder
                            .CompileWithReferenceLoadBehavior(referenceLoadBehavior)
                            .CompileWithAssemblyLoadBehavior(LoadBehaviorEnum.UseDefault)
                            .GetAssembly();
                        return (name!, currentName!, true);
                    }
                    catch
                    {

                        return (name!, currentName!, false);
                    }
                    
                }
            }
            finally
            {
                DomainManagement.Create(currentName!).Dispose();
            }
            
        }
    }
}
