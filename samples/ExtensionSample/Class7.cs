
//#if DEBUG
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Natasha.CSharp.HotExecutor.Component;
using Natasha.CSharp.Extension.HotExecutor;
using ExtensionSample;
namespace System
{


    class InterceptMain1
    {

        [ModuleInitializer]
        internal static void PreMain()
        {

            HEProxy.SetProjectKind(HEProjectKind.Console);
            HEProxy.SetCompileInitAction(() => {

                NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
                NatashaManagement.Preheating((asmName, @namespace) => !string.IsNullOrWhiteSpace(@namespace) && (@namespace.StartsWith("Microsoft.VisualBasic") || HEProxy.IsExcluded(@namespace)), true, true);

            });
            HEProxy.Run();

        }

    }
}
//#endif
