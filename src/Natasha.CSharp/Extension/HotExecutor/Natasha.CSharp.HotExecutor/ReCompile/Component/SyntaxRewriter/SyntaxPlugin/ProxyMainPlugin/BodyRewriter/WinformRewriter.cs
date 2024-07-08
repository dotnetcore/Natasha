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
            System.Threading.Tasks.Task.Run(() => 
            {{
                System.Windows.Forms.Application.ExitThread();
                var __heProxInstance = {runArgumentScript};
                System.Windows.Forms.Form tempForm;
                if (__heProxInstance is System.Windows.Forms.Form)
	            {{
                    tempForm = ((object)__heProxInstance as System.Windows.Forms.Form)!;
                }}
                else
                {{
                   tempForm = (System.Windows.Forms.Form)(typeof(System.Windows.Forms.ApplicationContext).GetProperty(""MainForm"")!.GetValue(__heProxInstance)!);
                }}
                System.Diagnostics.Debug.WriteLine($""当前主窗体程序集为 {{tempForm.GetType().Assembly.FullName}}!"");  
                System.Diagnostics.Debug.WriteLine($""当前主窗体为 {{tempForm.Name}}!""); 
                tempForm.FormClosed += (s, e) =>
                {{
                   if (!HEProxy.IsHotCompiling)
                   {{
                        var result = System.Windows.Forms.MessageBox.Show(""请确认是否退出主程序？"", ""HotExecutor 提醒"", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result.HasFlag(System.Windows.Forms.DialogResult.OK))
                        {{
                           System.Windows.Forms.Application.Exit();
	                    }}
                   }}
                }};
                Action? removeHandler = null;
                System.EventHandler handler = (s, e) =>
                {{
                    while(!DiposeWindows(tempForm)){{}};
                    removeHandler?.Invoke();

                }};
                removeHandler = () => {{

                    tempForm.Shown -= handler;

                }};

                tempForm.Shown += handler;
                try{{

                   System.Diagnostics.Debug.WriteLine(tempForm.button4.Text);
                   System.Windows.Forms.Application.Run(__heProxInstance); 

                }}catch(System.Exception ex)
                {{
                    System.Diagnostics.Debug.WriteLine(""启动出错"" + ex.Message);
                }}
                
                static bool DiposeWindows(System.Windows.Forms.Form aliveForm)
                {{
                        System.Collections.Generic.HashSet<System.Windows.Forms.Form> forms = [];
                        System.Collections.Generic.HashSet<System.Windows.Forms.Form> removeForms = [];
                        if(HEProxy.ObjectInstance!=null)
                        {{
                            forms.Add((System.Windows.Forms.Form)HEProxy.ObjectInstance);
                        }}  
                        forms.Add(aliveForm);
                        System.Diagnostics.Debug.WriteLine(System.Windows.Forms.Application.OpenForms.Count);

                        int i = 0;
                        do
                        {{
                            try
                            {{

                                var form = System.Windows.Forms.Application.OpenForms[i];
                                if(forms.Contains(form))
                                {{
                                    i+=1;
                                    continue;
                                }}
                                
                                if (form != null && form != aliveForm)
                                {{

                                    if(HEProxy.ObjectInstance==null){{
                                        HEProxy.ObjectInstance = form;
                                        form.BeginInvoke(new Action(() => {{
                                            System.Diagnostics.Debug.WriteLine($""当前将被隐藏的开放窗体 {{form.Name}}!"");
                                            form.Hide();   
                                            removeForms.Add(form);
                                        }})); 
                                        forms.Add(form);
                                    }}else {{
                                        form.BeginInvoke(new Action(() => {{
                                            System.Diagnostics.Debug.WriteLine($""当前将被注销的开放窗体 {{form.Name}}!"");
                                            form.Dispose();
                                            removeForms.Add(form);  
                                        }})); 
                                        forms.Add(form); 
                                    }}
                                }}

                            }}
                            catch (System.Exception ex)
                            {{
                                 System.Diagnostics.Debug.WriteLine($""注销窗体出错 {{ex.Message}}!"");
                                 return false;
                            }}
                        }} while (System.Windows.Forms.Application.OpenForms.Count > i);

                    foreach(var removeForm in removeForms)
                    {{
                        Natasha.CSharp.HotExecutor.Component.HEDelegateHelper<System.Windows.Forms.FormCollection, System.Windows.Forms.Form>.Execute.Invoke(System.Windows.Forms.Application.OpenForms, removeForm);
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
