using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Peachpie.CodeAnalysis.Utilities;
using Pchp.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;

namespace Natasha.CSharp.Extension.PHP
{
    internal static class PeachpieAnalysistor
    {
        internal static Func<CSharpCompilation, CSharpCompilation> Creator()
        {
            return compilation =>
            {
                
                return compilation;
            };
        }
    }
}
