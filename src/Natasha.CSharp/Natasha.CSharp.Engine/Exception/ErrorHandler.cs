using Microsoft.CodeAnalysis;
using Natasha;
using Natasha.CSharp.Engine.Utils;
using Natasha.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;


public static class ErrorHandler
{
    private readonly static ConcurrentDictionary<string, Func<Diagnostic, SyntaxBase, string, string>> _errorHandlers;
    static ErrorHandler()
    {

        _errorHandlers = new ConcurrentDictionary<string, Func<Diagnostic, SyntaxBase, string, string>>();
        _errorHandlers["CS0104"] = (diagnostic, syntax, sourceCode) =>
        {

            var (str1, str2) = CS0104Helper.Handler(diagnostic);
            var sets = syntax.ReferenceCache[diagnostic.Location.SourceTree.ToString()];
            if (sets.Contains(str1))
            {

                if (sets.Contains(str2))
                {

                    if (str2 == "System")
                    {

                        return sourceCode.Replace($"using {str2};", "");

                    }
                    else
                    {

                        return sourceCode.Replace($"using {str1};", "");

                    }

                }
                else
                {

                    return sourceCode.Replace($"using {str2};", "");

                }


            }
            else
            {

                return sourceCode.Replace($"using {str1};", "");

            }

        };


        _errorHandlers["CS0234"] = (diagnostic, syntax, sourceCode) =>
        {

            var tempResult = CS0234Helper.Handler(diagnostic);
            DefaultUsing.Remove(tempResult);
            return Regex.Replace(sourceCode, $"using {tempResult}(.*?);", "");

        };


        _errorHandlers["CS0246"] = (diagnostic, syntax, sourceCode) =>
        {

            CS0246Helper.Handler(diagnostic);
            foreach (var @using in CS0246Helper.GetUsings(diagnostic, sourceCode))
            {

                DefaultUsing.Remove(@using);
                sourceCode = sourceCode.Replace($"using {@using};", "");

            }
            return sourceCode;

        };

    }

    public static void Add(string code, Func<Diagnostic, SyntaxBase, string, string> func)
    {
        _errorHandlers[code] = func;
    }

    public static bool CanHandle(string errorCode)
    {
        return _errorHandlers.ContainsKey(errorCode);
    }

    public static string GetHandlerCode(string errorCode, Diagnostic diagnostic, SyntaxBase syntax, string sourceCode)
    {
        return _errorHandlers[errorCode](diagnostic, syntax, sourceCode);
    }
}

