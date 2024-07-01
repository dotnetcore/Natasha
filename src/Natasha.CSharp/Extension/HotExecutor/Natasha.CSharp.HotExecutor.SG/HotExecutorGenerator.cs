using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

            string coreScript = "HEProxy.SetProjectKind(HEProjectKind.Console);";
            string winformAndwpfLoggerScript = @"
string debugFilePath = Path.Combine(VSCSharpProjectInfomation.MainCsprojPath,""HEDebug.txt""); 
if(File.Exists(debugFilePath)){ File.Delete(debugFilePath); }
HEFileLogger logger = new HEFileLogger(debugFilePath);
HEProxy.ShowMessage = async msg => {
    await logger.WriteUtf8FileAsync(msg);
};";
            var syntaxTrees = context.Compilation.SyntaxTrees;
            var fileList = syntaxTrees.Select(item => item.FilePath).ToList();
            var classNodes = syntaxTrees
              .SelectMany(t => t.GetRoot().DescendantNodes())
              .OfType<ClassDeclarationSyntax>().ToList();
            // 检查是否有继承自 System.Windows.Forms.Form 的类
            var hasWinFormFormClass = classNodes
              .Any(c => c.BaseList != null && c.BaseList.Types.Any(t => t.Type.ToString() == "Form"));

            if (hasWinFormFormClass && fileList.Any(item=>item.EndsWith(".Designer.cs")))
            {
                // 这里可以根据判断结果执行相应的操作
                // 例如生成特定的代码或输出信息
                coreScript = $@"
{winformAndwpfLoggerScript}
HEProxy.SetProjectKind(HEProjectKind.Winform);
HEProxy.ExcludeGlobalUsing(""System.Windows.Controls"");
HEProxy.ExcludeGlobalUsing(""System.Windows"");
DelegateHelper<System.Windows.Forms.FormCollection, System.Windows.Forms.Form>.GetDelegate(""Remove"", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
";
            }
            else
            {

                var hasWPFWindowClass = classNodes
                  .Any(c => c.BaseList != null && c.BaseList.Types.Any(t => t.Type.ToString() == "Window"));

                if (hasWPFWindowClass && fileList.Any(item => item.EndsWith(".xaml.cs")))
                {
                    coreScript = $@"
{winformAndwpfLoggerScript}
HEProxy.SetProjectKind(HEProjectKind.WPF);
HEProxy.ExcludeGlobalUsing(""System.Windows.Forms"");
";
                }
            }


            var nameSapce = context.Compilation.GetEntryPoint(cancellationToken: new System.Threading.CancellationToken())!.ContainingNamespace.Name;
            string proxyMethodContent = $@"
//#if DEBUG
using System.IO;
using System.Runtime.CompilerServices;
using Natasha.CSharp.HotExecutor.Component;
using Natasha.CSharp.Extension.HotExecutor;
{(string.IsNullOrEmpty(nameSapce) ? string.Empty : "using " + nameSapce + ";")}
namespace System{{


    class InterceptMain
    {{
        
        [ModuleInitializer]
        internal static void PreMain()
        {{

            {coreScript}
            HEProxy.SetCompileInitAction(()=>{{
                NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
                NatashaManagement.Preheating((asmName, @namespace) => !string.IsNullOrWhiteSpace(@namespace) && (@namespace.StartsWith(""Microsoft.VisualBasic"")|| HEProxy.IsExcluded(@namespace)),true, true);

            }});
            HEProxy.Run();

        }}

    }}
}}
//#endif
";

            context.AddSource($"NatashaHotExecutorProxy.g.cs", proxyMethodContent);
        }
    }
}
