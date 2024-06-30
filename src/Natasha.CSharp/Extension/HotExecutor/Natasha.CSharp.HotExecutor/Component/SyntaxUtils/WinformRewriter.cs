using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal static class WinformRewriter
    {
        public static BlockSyntax? Handle(BlockSyntax blockSyntax)
        {
            string runArgumentScript = string.Empty;
            StatementSyntax? runNode = null;
            foreach (var statement in blockSyntax.Statements)
            {
                if (statement is ExpressionStatementSyntax node)
                {
                    var invoke = node.DescendantNodes().OfType<InvocationExpressionSyntax>().FirstOrDefault();
                    if (invoke != null)
                    {
                        var invokeCaller = invoke.DescendantNodes().OfType<MemberAccessExpressionSyntax>().FirstOrDefault();
                        if (invokeCaller != null)
                        {
                            var memberList = invoke.DescendantNodes().OfType<IdentifierNameSyntax>().ToList();
                            if (memberList.Count > 1)
                            {
                                if (memberList[0].ToString() == "Application" && memberList[1].ToString() == "Run")
                                {
                                    if (invoke.ArgumentList != null && invoke.ArgumentList.Arguments != null && invoke.ArgumentList.Arguments.Count > 0)
                                    {
                                        runNode = statement;
                                        runArgumentScript = invoke.ArgumentList!.Arguments[0].ToString();
                                        break;
                                    }
                                    else
                                    {
                                        HEProxy.ShowMessage("Application.Run() 方法参数不正确！HE 目前只能针对 Application.Run(Form) 做处理！");
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (runNode != null)
            {

                return blockSyntax.ReplaceNode(runNode, [SyntaxFactory.ParseStatement(@$"
        HEProxy.SetAftHotExecut(() => 
        {{
            Task.Run(() => 
            {{
                Application.ExitThread();
                while(!DiposeWindows()){{}};
                var __heProxInstance = {runArgumentScript};
                Form tempForm;
                if (__heProxInstance is Form)
	            {{
                    tempForm = ((object)__heProxInstance as Form)!;
                }}
                else
                {{
                   tempForm = (Form)(typeof(ApplicationContext).GetProperty(""MainForm"")!.GetValue(__heProxInstance)!);
                }}   
                tempForm.FormClosed += (s, e) =>
                {{
                   if (!HEProxy.IsHotCompiling)
                   {{
                        var result = MessageBox.Show(""请确认是否退出主程序？"", ""HotExecutor 提醒"", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result.HasFlag(DialogResult.OK))
                        {{
                           Application.Exit();
	                    }}
                   }}
                }};
               
                try{{
                   Application.Run(__heProxInstance); 
                }}catch(Exception ex)
                {{
                    HEProxy.ShowMessage(ex.Message);
                }}

                static bool DiposeWindows()
                {{
                    try{{
                        for (int i = 0; i < Application.OpenForms.Count; i++)
                        {{
                            try{{
                                var form = Application.OpenForms[i];
                                if (form!=null)
                                {{
                                    HEProxy.ShowMessage($""当前将被注销的开放窗体 {{form.Name}}"");
                                    form.Dispose();
                                    DelegateHelper<FormCollection, Form>.Execute.Invoke(Application.OpenForms, form);
                                }}
                            }}catch{{
                            }}
                        }}
                    }}catch{{
                        return false;
                    }}
                    return true;
                }}
            }});
        }});")]);
            }
            else
            {
                HEProxy.ShowMessage("HE Winform 目前只能针对 Application.Run(Form/ApplicationContext) 做处理！请务在 Main 中使用 Application.Run 方法及参数！");
            }
            return null;
        }
    }
}
