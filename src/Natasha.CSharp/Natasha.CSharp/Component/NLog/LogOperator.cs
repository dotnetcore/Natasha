using Microsoft.CodeAnalysis.CSharp;
using Natasha.Error;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Natasha.CSharpEngine.Log
{

    public static class LogOperator
    {

        public static async Task ErrorRecoderAsync(CSharpCompilation compilation, NatashaException exception)
        {

            NErrorLog log = new NErrorLog();
            log.Handler(compilation, exception.Diagnostics);
            await log.Write();

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
