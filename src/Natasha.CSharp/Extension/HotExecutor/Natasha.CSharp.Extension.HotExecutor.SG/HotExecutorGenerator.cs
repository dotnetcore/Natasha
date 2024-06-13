using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Linq;

namespace Natasha.CSharp.Extension.HotExecutor.SG
{
    [Generator]
    public class HotExecutorGenerator : ISourceGenerator
    {

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }

        void ISourceGenerator.Execute(GeneratorExecutionContext context)
        {
            string proxyMethodContent = $@"
#if DEBUG
using System.Runtime.CompilerServices;

namespace System{{


    class InterceptMain
    {{
        
        [ModuleInitializer]
        internal static void PreMain()
        {{
            ProjectDynamicProxy.SetCompileInitAction(()=>{{
                NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
                NatashaManagement.Preheating(true, true);
            }});
            ProjectDynamicProxy.Run();
        }}

    }}
}}
#endif
";

            context.AddSource($"NatashaHotExecutorProxy.g.cs", proxyMethodContent);
            ////Debugger.Launch();
            //var mainFile = string.Empty;
            //var lineNumber = 0;
            //var characterPosition = 0;
            //var syntaxTrees = context.Compilation.SyntaxTrees;
            //foreach (var tree in syntaxTrees)
            //{
            //    var methods = tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>();
            //    if (methods.Any(method => method.Identifier.Text == "Main"))
            //    {
            //        var mainMethod = methods.FirstOrDefault(method => method.Identifier.Text == "Main");
            //        mainFile = tree.FilePath;

            //        var lineSpan = mainMethod.GetLocation().GetLineSpan();
            //        lineNumber = lineSpan.StartLinePosition.Line + 1;
            //        characterPosition = lineSpan.StartLinePosition.Character + 1;
            //        //Debugger.Launch();

            //        break;
            //    }
            //}
        }
    }
}
