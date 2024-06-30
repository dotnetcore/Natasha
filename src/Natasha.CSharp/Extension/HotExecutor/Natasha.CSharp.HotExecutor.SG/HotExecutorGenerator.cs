using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
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

            string coreScript = "HEProxy.SetProjectKind(HEProjectKind.Console);";
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
                coreScript = @"
string debugFilePath = Path.Combine(VSCSharpProjectInfomation.MainCsprojPath,""HEDebug.txt""); 
if(File.Exists(debugFilePath)){ File.Delete(debugFilePath); }
HEProxy.ShowMessage = msg => {
    HEProxy.WriteUtf8File(debugFilePath, msg + Environment.NewLine);
};
HEProxy.SetProjectKind(HEProjectKind.Winform);
HEProxy.ExtGlobalUsing.Add(""System.Windows.Controls"");
HEProxy.ExtGlobalUsing.Add(""System.Windows"");
";
            }
            else
            {

                var hasWPFWindowClass = classNodes
                  .Any(c => c.BaseList != null && c.BaseList.Types.Any(t => t.Type.ToString() == "Window"));

                if (hasWPFWindowClass && fileList.Any(item => item.EndsWith(".xaml.cs")))
                {
                    coreScript = @"
string debugFilePath = Path.Combine(VSCSharpProjectInfomation.MainCsprojPath,""HEDebug.txt""); 
if(File.Exists(debugFilePath)){ File.Delete(debugFilePath); }
HEProxy.ShowMessage = msg => {
    HEProxy.WriteUtf8File(debugFilePath, msg + Environment.NewLine);
};
HEProxy.SetProjectKind(HEProjectKind.WPF);
HEProxy.ExtGlobalUsing.Add(""System.Windows.Forms"");
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
                NatashaManagement.Preheating((asmName, @namespace) => !string.IsNullOrWhiteSpace(@namespace) && (@namespace.StartsWith(""Microsoft.VisualBasic"")|| HEProxy.ExtGlobalUsing.Contains(@namespace)),true, true);

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
