using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharp.HotExecutor.Component.SyntaxUtils
{
    internal static class WpfWriter
    {
        public static BlockSyntax? Handle(BlockSyntax blockSyntax)
        {
            return blockSyntax.WithStatements([SyntaxFactory.ParseStatement(@$"HEProxy.SetAftHotExecut(() => {{
                    Task.Run(() => {{
                       
                       while(!DiposeWindows()){{}};
                       Application.Current.Run();

                        static bool DiposeWindows()
                        {{
                            try{{
                                   for (int i = 0; i < Application.Current.Windows.Count; i++)
                                   {{
                                        var window = Application.Current.Windows[i];
                                        try{{
                                            window.Close();
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
    }
}
