using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal static class WpfWriter
    {
        public static BlockSyntax? Handle(BlockSyntax blockSyntax)
        {
            var mainStatement = SyntaxFactory.ParseStatement(@"

                System.Windows.Application.Current.Dispatcher.Invoke(()=>{

                        while(!DiposeWindows()){};
                        
                        static bool DiposeWindows()
                        {
                            try{
                                   //HEProxy.ShowMessage($""当前将被注销的开放窗体个数 {System.Windows.Application.Current.Windows.Count}"");
                                   for (int i = 0; i < System.Windows.Application.Current.Windows.Count; i++)
                                   {
                                       
                                        try{
                                            var window = System.Windows.Application.Current.Windows[i];
                                            window.Dispatcher.Invoke(()=>{ window.Close(); });
                                        }catch{

                                        }
                                   }

                            }catch(System.Exception ex){

                                HEProxy.ShowMessage($""出现异常 {ex.GetType()}{ex.Message}"");
                                if(ex is not System.InvalidOperationException){
                                    return false;
                                }

                            }
                            return true;
                        }
                });
                System.Windows.Application.Current.Shutdown();
            ");

            var newStatement = SyntaxFactory.ParseStatement(@$"
           
            HEProxy.SetAftHotExecut(() => {{

{blockSyntax.Statements.ToFullString()}
                
            }});");
            Debug.WriteLine(mainStatement.ToFullString());
            return blockSyntax.WithStatements([newStatement]);
        }
    }
}
