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
string debugFilePath = Path.Combine(VSCSProjectInfoHelper.HEOutputPath,""Debug.txt""); 
if (Directory.Exists(VSCSProjectInfoHelper.HEOutputPath))
{
    var files = Directory.GetFiles(VSCSProjectInfoHelper.HEOutputPath);
    foreach (var file in files)
    {
        File.Delete(file);
    }
}
HEFileLogger logger = new HEFileLogger(debugFilePath);
HEProxy.ShowMessage = async msg => {
    await logger.WriteUtf8FileAsync(msg);
};";
            var syntaxTrees = context.Compilation.SyntaxTrees;
            var fileList = syntaxTrees.Select(item => item.FilePath).ToList();
            var classNodes = syntaxTrees
              .SelectMany(t => t.GetRoot().DescendantNodes())
              .OfType<ClassDeclarationSyntax>().ToList();
            if (classNodes != null && classNodes.Count>0)
            {
                // 检查是否有继承自 System.Windows.Forms.Form 的类
                var hasWinFormFormClass = classNodes
                  .Any(c => c.BaseList != null && c.BaseList.Types.Any(t => t.Type.ToString() == "Form"));

                if (hasWinFormFormClass && fileList.Any(item => item.EndsWith(".Designer.cs")))
                {
                    // 这里可以根据判断结果执行相应的操作
                    // 例如生成特定的代码或输出信息
                    coreScript = $@"
{winformAndwpfLoggerScript}
HEProxy.SetProjectKind(HEProjectKind.Winform);
HEProxy.ExcludeGlobalUsing(""System.Windows.Controls"");
HEProxy.ExcludeGlobalUsing(""System.Windows"");
HEDelegateHelper<System.Windows.Forms.FormCollection, System.Windows.Forms.Form>.GetDelegate(""Remove"", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
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
            }
            var nameSpace = context.Compilation.GetEntryPoint(cancellationToken: new System.Threading.CancellationToken())!.ContainingNamespace.Name;
            string proxyMethodContent = $@"
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Natasha.CSharp.HotExecutor;
using Natasha.CSharp.HotExecutor.Utils;
{(string.IsNullOrEmpty(nameSpace) ? string.Empty : "using " + nameSpace + ";")}
namespace System{{


    class InterceptMain
    {{
        
        [ModuleInitializer]
        internal static void PreMain()
        {{
            
            try{{
                HEProxy.SGCompleted();
                Debug.WriteLine(""Run HE SG!"");
                {coreScript}
                HEProxy.SetCompileInitAction(()=>{{

                    NatashaManagement.RegistDomainCreator<NatashaDomainCreator>();
                    NatashaManagement.Preheating((asmName, @namespace) => !string.IsNullOrWhiteSpace(@namespace) && (@namespace.StartsWith(""Microsoft.VisualBasic"")|| HEProxy.IsExcluded(@namespace)),true, true);

                }});
                HEProxy.Run();

            }}catch(Exception ex)
            {{
                Debug.WriteLine(""--------------------------"");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(""--------------------------"");
            }}
        }}

    }}
}}
";

            context.AddSource($"NatashaHotExecutorProxy.g.cs", proxyMethodContent);
        }
    }
}
