using Microsoft.CodeAnalysis.CSharp;
using Natasha.Error;
using System;
using System.Collections.Generic;

namespace Natasha.CSharpEngine.Log
{

    public static class LogOperator
    {

        public static void ErrorRecoder(CSharpCompilation compilation, List<CompilationException> exceptions)
        {

            if (NErrorLog.Enabled)
            {

                NErrorLog log = new NErrorLog();
                foreach (var item in exceptions)
                {
                    log.Handler(compilation, item.Diagnostics);
                }
                log.Write();

            }

        }
        public static void ErrorRecoder(params CompilationException[] exceptions)
        {

            if (NErrorLog.Enabled)
            {

                NErrorLog log = new NErrorLog();
                foreach (var item in exceptions)
                {
                    log.Handler(item.Diagnostics);
                }
                log.Write();

            }

        }
        public static void SucceedRecoder(CSharpCompilation compilation)
        {

            if (NSucceedLog.Enabled)
            {

                NSucceedLog log = new NSucceedLog();
                log.Handler(compilation);
                log.Write();

            }

        }

    }

}
