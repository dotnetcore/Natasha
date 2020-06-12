using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Natasha.CSharpEngine.Compile;
using Natasha.CSharpEngine.Error;
using Natasha.CSharpEngine.Syntax;
using Natasha.Engine.Utils;
using Natasha.Error;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;

namespace Natasha.CSharpEngine
{
    public class NatashaCSharpEngine
    {

        public static ConcurrentDictionary<string, Action<CompilationException, Diagnostic, NatashaCSharpSyntax, Dictionary<string, string>>> ErrorHandlers;
        static NatashaCSharpEngine()
        {

            ErrorHandlers = new ConcurrentDictionary<string, Action<CompilationException, Diagnostic, NatashaCSharpSyntax, Dictionary<string, string>>>();
            ErrorHandlers["CS0104"] = (exception, diagnostic, syntax, dict) =>
            {

                var (str1, str2) = CS0104Helper.Handler(diagnostic);
                var sets = syntax.UsingCache[exception.Formatter];
                if (sets.Contains(str1))
                {

                    if (sets.Contains(str2))
                    {

                        if (str2 == "System")
                        {

                            dict[exception.Formatter] = dict[exception.Formatter].Replace($"using {str2};", "");

                        }
                        else
                        {

                            dict[exception.Formatter] = dict[exception.Formatter].Replace($"using {str1};", "");

                        }

                    }
                    else
                    {

                        dict[exception.Formatter] = dict[exception.Formatter].Replace($"using {str2};", "");

                    }


                }
                else
                {

                    dict[exception.Formatter] = dict[exception.Formatter].Replace($"using {str1};", "");

                }

            };


            ErrorHandlers["CS0234"] = (exception, diagnostic, syntax, dict) =>
            {

                var tempResult = CS0234Helper.Handler(diagnostic);
                GlobalUsing.Remove(tempResult);
                dict[exception.Formatter] = Regex.Replace(dict[exception.Formatter], $"using {tempResult}(.*?);", "");

            };


            ErrorHandlers["CS0246"] = (exception, diagnostic, syntax, dict) =>
            {

                CS0246Helper.Handler(diagnostic);
                foreach (var @using in CS0246Helper.GetUsings(diagnostic, dict[exception.Formatter]))
                {

                    GlobalUsing.Remove(@using);
                    dict[exception.Formatter] = dict[exception.Formatter].Replace($"using {@using};", "");

                }

            };

        }


        public NatashaCSharpSyntax Syntax;
        public NatashaCSharpCompiler Compiler;
        public List<CompilationException> Exceptions;


        public int RetryLimit;
        public int RetryCount;
        public bool CanRetry;
        public bool CustomReferences;
        public bool UseShareLibraries;


        public NatashaCSharpEngine(string name)
        {

            Syntax = new NatashaCSharpSyntax();
            Compiler = new NatashaCSharpCompiler();
            Compiler.AssemblyName = name;
            Compiler.StreamCompileFailedHandler += NatashaEngine_StreamCompileFailedHandler;
            Compiler.FileCompileFailedHandler += NatashaEngine_FileCompileFailedHandler; ;

        }




        private void NatashaEngine_FileCompileFailedHandler(string arg1, string arg2, ImmutableArray<Diagnostic> arg3, CSharpCompilation arg4)
        {

            if (CanRetryCompile(arg3))
            {
                Compile();
            }

        }

        private void NatashaEngine_StreamCompileFailedHandler(Stream arg1, ImmutableArray<Diagnostic> arg2, CSharpCompilation arg3)
        {

            if (CanRetryCompile(arg2))
            {
                Compile();
            }

        }




        /// <summary>
        /// 拿到异常之后进行后处理，如果处理后可以重编译，将再次编译
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        private bool CanRetryCompile(ImmutableArray<Diagnostic> errors)
        {

            Exceptions = NatashaException.GetCompileException(Compiler.AssemblyName, errors);
            Dictionary<string, string> codeMappings = new Dictionary<string, string>();
            foreach (var item in Exceptions)
            {

                if (item.Formatter != default)
                {

                    codeMappings[item.Formatter] = item.Formatter;
                    if (item.HasError)
                    {

                        foreach (var error in item.Diagnostics)
                        {


                            if (ErrorHandlers.ContainsKey(error.Id))
                            {

                                CanRetry = true;
                                ErrorHandlers[error.Id](item, error, Syntax, codeMappings);

                            }

                        }
                    }

                }
                
            }
            if (CanRetry)
            {

                CanRetry = false;
                RetryCount += 1;
                if (RetryCount < RetryLimit)
                {

                    foreach (var item in codeMappings)
                    {

                        Syntax.Update(item.Key, item.Value);

                    }
                    return true;
                }
                

            }
            return false;

        }




        /// <summary>
        /// 对语法树进行编译
        /// </summary>
        internal virtual void Compile()
        {

            if (CustomReferences)
            {
                ((AssemblyDomain)Compiler.Domain).CustomReferences();
            }
            if (UseShareLibraries)
            {
                ((AssemblyDomain)Compiler.Domain).UseShareLibraries();
            }
            Exceptions = null;
            Compiler.CompileTrees = Syntax.TreeCache.Values;
            Compiler.Compile();

        }
    }
}
