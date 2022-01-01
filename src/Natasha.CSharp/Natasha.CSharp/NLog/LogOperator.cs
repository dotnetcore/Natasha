using Microsoft.CodeAnalysis.CSharp;
using Natasha.Error;
using System;
using System.Collections.Generic;

namespace Natasha.CSharpEngine.Log
{

    public static class LogOperator
    {

        public static void ErrorRecoder(CSharpCompilation compilation, List<NatashaException> exceptions)
        {

            NErrorLog log = new NErrorLog();
            foreach (var item in exceptions)
            {
                log.Handler(compilation, item.Diagnostics);
            }
            log.Write();

        }
        public static void ErrorRecoder(params NatashaException[] exceptions)
        {

            NErrorLog log = new NErrorLog();
            foreach (var item in exceptions)
            {
                log.Handler(item.Diagnostics);
            }
            log.Write();

        }
        public static void SucceedRecoder(CSharpCompilation compilation)
        {

            NSucceedLog log = new NSucceedLog();
            log.Handler(compilation);
            log.Write();

        }

    }

}
