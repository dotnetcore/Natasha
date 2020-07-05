using Microsoft.CodeAnalysis.CSharp;
using Natasha.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Natasha.CSharpEngine
{

    public static class CompilerManagement
    {
        public static Func<CompilerBase<CSharpCompilation, CSharpCompilationOptions>> GetCompiler;
        public static void RegisterDefault<T>() where T : CompilerBase<CSharpCompilation, CSharpCompilationOptions>, new()
        {
            GetCompiler = () => { return new T();  };
        }

    }
}
